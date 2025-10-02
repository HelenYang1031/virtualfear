using System.Collections.Generic;
using UnityEngine;
// 需要安装官方 Splines 包（Window > Package Manager > Unity Registry > Splines）
using UnityEngine.Splines;

public class MillipedeBuilder : MonoBehaviour
{
    [Header("Spline Setup")]
    public SplineContainer spline;         // 拖进来你的样条
    public float spacing = 0.25f;          // 邻接节段间距（米）
    public bool alignToSpline = true;      // 是否按切线朝向

    [Header("Segment Prefabs")]
    public GameObject headPrefab;          // 头的预制体
    public GameObject bodyPrefab;          // 身体段的预制体（重复使用）
    public int bodyCount = 20;             // 身体段数量（不含头）

    [Header("Build Options")]
    public bool buildOnPlay = true;
    public Transform parentForSegments;    // 可选：生成到哪个父物体下

    [Header("Output (read-only)")]
    public List<Transform> segments = new List<Transform>(); // [0]是头

    void Awake()
    {
        if (buildOnPlay) Build();
    }

    [ContextMenu("Build Now")]
    public void Build()
    {
        // 清理旧物体
        foreach (var t in segments)
        {
            if (t)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) DestroyImmediate(t.gameObject);
                else Destroy(t.gameObject);
#else
                Destroy(t.gameObject);
#endif
            }
        }
        segments.Clear();

        if (spline == null)
        {
            Debug.LogError("MillipedeBuilder: 请指定 SplineContainer。");
            return;
        }

        // 计算需要的总长度
        var splineLen = SplineUtility.CalculateLength(spline.Spline, 4f);
        var totalSegs = 1 + Mathf.Max(0, bodyCount); // 头 + 身体
        var neededLen = spacing * (totalSegs - 1);
        if (neededLen > splineLen)
        {
            Debug.LogWarning($"样条总长({splineLen:F2}m) < 需要长度({neededLen:F2}m)，将压缩取样。");
        }

        // 根据“沿弧长”的方式求每个 segment 的 t
        // 头在起点，后续按等距排
        for (int i = 0; i < totalSegs; i++)
        {
            float dist = i * spacing;
            float t = Mathf.Clamp01(SplineUtility.GetNormalizedInterpolationForDistance(spline.Spline, dist));
            SplineUtility.Evaluate(spline.Spline, t, out var pos, out var tangent, out var up);

            Quaternion rot = Quaternion.identity;
            if (alignToSpline)
            {
                // 用切线作为前向
                var forward = (Vector3)tangent.normalized;
                var upVec = ((Vector3)up).sqrMagnitude > 1e-4f ? (Vector3)up : Vector3.up;
                rot = Quaternion.LookRotation(forward, upVec);
            }

            bool isHead = (i == 0);
            var prefab = isHead ? headPrefab : bodyPrefab;
            if (prefab == null)
            {
                Debug.LogError("请指定 headPrefab / bodyPrefab。");
                return;
            }

            var go = Instantiate(prefab, pos, rot, parentForSegments ? parentForSegments : transform);
            go.name = isHead ? "Head" : $"Body_{i}";
            segments.Add(go.transform);
        }

        // 自动添加跟随控制
        var chain = gameObject.GetComponent<MillipedeChain>();
        if (!chain) chain = gameObject.AddComponent<MillipedeChain>();
        chain.InitFromBuilder(this);
        chain.targetSpacing = spacing;
    }
}
