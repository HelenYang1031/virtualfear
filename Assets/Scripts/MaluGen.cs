using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MaluGen : MonoBehaviour
{
    // 至少填 1 个 Prefab
    public GameObject[] prefabs;
    public int count = 10;
    public float spacing = 1f;
    public Vector3 localDirection = Vector3.right;

    public bool buildOnPlay = true;
    public bool clearChildren = true;

    // 要添加到每个实例上的脚本类名（支持短名或完整命名空间）
    // 例如 "MySegment" 或 "MyGame.Systems.MySegment"
    public string scriptClassName = "MySegment";

    void Awake()
    {
        if (buildOnPlay) SpawnNow();
    }

    [ContextMenu("Spawn Now")]
    public void SpawnNow()
    {
        if (clearChildren)
        {
            var list = new List<GameObject>();
            foreach (Transform c in transform) list.Add(c.gameObject);
            foreach (var go in list)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) DestroyImmediate(go);
                else Destroy(go);
#else
                Destroy(go);
#endif
            }
        }

        var dir = localDirection.normalized;

        // 解析要添加的脚本类型（找不到会直接抛异常——按你的要求不做判空）
        var addType =
            Type.GetType(scriptClassName) ??
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .First(t => t.Name == scriptClassName || t.FullName == scriptClassName);

        for (int i = 0; i < count; i++)
        {
            var prefab = prefabs[i % prefabs.Length];
            var go = Instantiate(prefab, transform);
            go.name = $"{prefab.name}_{i:D2}";
            go.transform.localPosition = dir * spacing * i;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            // 不检查是否已存在，直接添加
            go.AddComponent(addType);
        }
    }
}