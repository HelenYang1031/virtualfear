using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriWallGridSetup : MonoBehaviour
{
    [Header("Don't change these")]
    public Material blueMat;
    public Material greenMat;
    public Material redMat;
    [Range(0,1)]
    public float alpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        SetAlpha();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAlpha()
    {
        SetMat(blueMat);
        SetMat(greenMat);
        SetMat(redMat);
    }

    void SetMat(Material mat)
    {
        Color c = mat.color;
        c.a = alpha;
        mat.color = c;
    }
}
