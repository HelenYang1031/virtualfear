using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Splines;

[System.Serializable]
public class MaluEntry
{
    public GameObject prefab; // Prefab with SplineAnimate already attached
    public int quantity;      // How many to spawn
}

public class MaluGen : MonoBehaviour
{
    [Header("Malu Settings")]
    public List<MaluEntry> MaluList;

    [Header("Spline Settings")]
    public SplineContainer spline;
    public float interval = 0.1f;
    public float speed;

    // 控制 Spline 动画是否开始（您原来的 move 变量）
    public bool move = false;

    // 用来控制是否执行 Malu 生成的代码
    public bool generate = false;

    private bool moving = false;
    private List<GameObject> spawnedInstances = new List<GameObject>();

    // 追踪是否已经生成过，防止多次生成
    private bool hasGenerated = false;


    void Start()
    {
        // Start 仅检查 generate 的初始状态
        if (generate)
        {
            TryGenerate();
        }
    }

    void Update()
    {
        // 允许在运行时通过 Inspector 切换 generate 变量来触发生成
        // 只有当 generate 为 true 且尚未生成时才执行
        if (generate && !hasGenerated)
        {
            TryGenerate();
        }

        // 您原有的控制动画移动的逻辑
        if (Application.isPlaying && move && !moving)
        {
            foreach (var obj in spawnedInstances)
            {
                if (obj == null) continue;

                SplineAnimate anim = obj.GetComponent<SplineAnimate>();
                if (anim != null)
                {
                    anim.Restart(true);
                }
            }

            moving = true; // already moving!
        }
    }

    /// <summary>
    /// 检查条件并执行生成逻辑
    /// </summary>
    void TryGenerate()
    {
        if (hasGenerated) return; // 已经生成过，不再重复

        Debug.Log("Generating Malu instances...");
        AddToSpline();
        hasGenerated = true; // 标记为已生成
        generate = true;     // 保持 generate 为 true，以防在 Update 中再次触发
    }

    void AddToSpline()
    {
        float offset = 0f;

        for (int k = MaluList.Count - 1; k >= 0; k--)
        {
            MaluEntry entry = MaluList[k];
            if (entry.prefab == null)
            {
                Debug.LogWarning("Prefab in MaluList is null. Skipping.");
                continue;
            }

            for (int i = 0; i < entry.quantity; i++)
            {
                offset += interval;

                GameObject instance = Instantiate(entry.prefab);
                instance.name = $"{entry.prefab.name}_{k}_{i}";
                instance.transform.SetParent(transform);
                spawnedInstances.Add(instance);

                //Malu Shader
                Renderer renderer = instance.GetComponent<Renderer>();
                if (renderer != null && renderer.material.HasProperty("_randomseed"))
                {
                    renderer.material.SetFloat("_randomseed", Random.value);
                }

                // Spline animaate
                SplineAnimate anim = instance.GetComponent<SplineAnimate>();

                if (anim == null)
                {
                    Debug.LogError($"Prefab '{entry.prefab.name}' does not have a SplineAnimate component.");
                    continue;
                }

                anim.Container = spline;           // Assign the spline path
                anim.StartOffset = offset;         // Stagger start position
                anim.Restart(false);
                //anim.LoopMode = SplineAnimate.LoopMode.Once;
                anim.AnimationMethod = SplineAnimate.Method.Speed;
                anim.MaxSpeed = speed;
            }
        }
    }
}