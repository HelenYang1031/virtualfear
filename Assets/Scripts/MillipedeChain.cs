using System.Collections.Generic;
using UnityEngine;

public class MillipedeChain : MonoBehaviour
{
    [Header("Chain Segments (auto-filled by builder)")]
    public List<Transform> segments = new List<Transform>(); // [0]是头

    [Header("Follow Settings")]
    public float targetSpacing = 0.25f;   // 相邻段间的目标距离
    public int solverIterations = 3;      // 每帧解算迭代次数（提高可更“硬”）
    public float rotationFollow = 12f;    // 朝向跟随（角速度，越大越快）
    public float positionStiffness = 1f;  // 位置刚度（0-1），1=立即贴合
    public float maxStepPerFrame = 0.5f;  // 每帧最多移动量，避免抖/穿模

    [Header("Optional: Dampen / Wiggle")]
    public float tailLag = 0.0f;          // 尾部延迟（0~0.5），增加“肉感”
    public float swayAmplitude = 0.0f;    // 轻微摆动（美术风格化）
    public float swayFrequency = 2.0f;

    // runtime
    private Vector3[] desired;            // 期望位置（解算中间量）

    public void InitFromBuilder(MillipedeBuilder builder)
    {
        segments = builder.segments;
        targetSpacing = builder.spacing;
        SetupRuntimeBuffers();
    }

    void OnEnable()
    {
        if (segments == null || segments.Count == 0) return;
        SetupRuntimeBuffers();
    }

    void SetupRuntimeBuffers()
    {
        if (segments == null || segments.Count == 0) return;
        desired = new Vector3[segments.Count];
        for (int i = 0; i < segments.Count; i++)
            desired[i] = segments[i].position;
    }

    void LateUpdate()
    {
        if (segments == null || segments.Count < 2) return;

        // 头：直接使用当前变换
        desired[0] = segments[0].position;

        // 迭代解算，把每个 body 拉到 leader 附近，维持 targetSpacing
        for (int iter = 0; iter < solverIterations; iter++)
        {
            for (int i = 1; i < segments.Count; i++)
            {
                Vector3 leader = (iter == 0) ? segments[i - 1].position : desired[i - 1];
                Vector3 curr = (iter == 0) ? segments[i].position : desired[i];

                Vector3 toLeader = leader - curr;
                float dist = toLeader.magnitude;
                if (dist > 1e-6f)
                {
                    Vector3 dir = toLeader / dist;
                    Vector3 targetPos = leader - dir * targetSpacing;

                    // 让尾部稍微“慢一点”
                    float lagFactor = 1f - Mathf.Lerp(0f, tailLag, i / (segments.Count - 1f));
                    float stiff = Mathf.Clamp01(positionStiffness * lagFactor);

                    Vector3 newPos = Vector3.Lerp(curr, targetPos, stiff);
                    // 限制每帧移动，防止瞬移抖动
                    Vector3 delta = newPos - segments[i].position;
                    float maxMove = maxStepPerFrame * Time.deltaTime * 60f;
                    if (delta.magnitude > maxMove) newPos = segments[i].position + delta.normalized * maxMove;

                    desired[i] = newPos;
                }
                else
                {
                    desired[i] = curr;
                }
            }
        }

        // 应用位置并更新旋转
        for (int i = 1; i < segments.Count; i++)
        {
            // 位置
            segments[i].position = desired[i];

            // 朝向：看向前一节
            Vector3 fwd = (segments[i - 1].position - segments[i].position);
            if (fwd.sqrMagnitude > 1e-6f)
            {
                Quaternion tgtRot = Quaternion.LookRotation(fwd, Vector3.up);
                segments[i].rotation = Quaternion.Slerp(segments[i].rotation, tgtRot, rotationFollow * Time.deltaTime);
            }

            // 选配：轻微蛇形/肌肉摆动
            if (swayAmplitude > 0f)
            {
                float phase = (i * 0.2f) + Time.time * swayFrequency;
                Vector3 side = Vector3.Cross(Vector3.up, fwd.normalized);
                segments[i].position += side * (Mathf.Sin(phase) * swayAmplitude * 0.01f);
            }
        }
    }
}
