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

    public bool move = false;
    public bool generate = false;
    private bool exist = true;
    private List<GameObject> spawnedInstances = new List<GameObject>();


    void Start()
    {
        AddToSpline();
    }
    
    void AddToSpline()
    {
        float offset = 0f;

        for (int k = MaluList.Count-1;  k >= 0; k--)
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
                instance.GetComponent<Renderer>().material.SetFloat("_randomseed",Random.value);
                // Spline animaate
                SplineAnimate anim = instance.GetComponent<SplineAnimate>();

                if (anim == null)
                {
                    Debug.LogError($"Prefab '{entry.prefab.name}' does not have a SplineAnimate component.");
                    continue;
                }

                anim.Container = spline;            // Assign the spline path
                anim.StartOffset = offset;          // Stagger start position
                anim.Restart(false);                
                anim.AnimationMethod = SplineAnimate.Method.Speed;
                anim.MaxSpeed = speed;
                
            }
        }
    }
    void Update()
    {
        
        /*
        if (generate)
        {
            AddToSpline();
            foreach (var obj in spawnedInstances)
            {
                if (obj == null) continue;

                Destroy(obj);
            }
            exist = true;
            generate = false; //reset after triggering
        }
        */

        if (Application.isPlaying && move && exist)
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

            move = false; // reset after triggering
        }

    }
}
