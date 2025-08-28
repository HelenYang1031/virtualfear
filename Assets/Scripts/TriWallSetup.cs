using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriWallSetup : MonoBehaviour
{
    public bool blueVisible = true;
    public bool greenVisible = true;
    public bool redVisible = true;
    public bool postVisible = false;
    public Material matBlue;
    public Material matGreen;
    public Material matRed;
    public bool useDefaltMat = true;

    [Header("Don't change these")]
    public GameObject blueWall;
    public GameObject greenWall;
    public GameObject redWall;
    public GameObject columne;

    private Material matDefaultBlue;
    private Material matDefaultGreen;
    private Material matDefaultRed;

    // Start is called before the first frame update
    void Start()
    {
        TriWallGridSetup grid = transform.parent.parent.gameObject.GetComponent<TriWallGridSetup>();
        matDefaultBlue  = grid.blueMat;
        matDefaultGreen = grid.greenMat;
        matDefaultRed   = grid.redMat;
        SetMats();
        SetWalls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMats()
    {
        if (useDefaltMat)
        {
            blueWall.GetComponent<Renderer>().material = matDefaultBlue;
            greenWall.GetComponent<Renderer>().material = matDefaultGreen;
            redWall.GetComponent<Renderer>().material = matDefaultRed;
        }
        else
        {
            blueWall.GetComponent<Renderer>().material = matBlue;
            greenWall.GetComponent<Renderer>().material = matGreen;
            redWall.GetComponent<Renderer>().material = matRed;
        }
    }
    public void SetWalls()
    {
        blueWall.SetActive(blueVisible);
        greenWall.SetActive(greenVisible);
        redWall.SetActive(redVisible);
        if (postVisible || blueVisible || greenVisible || redVisible) columne.SetActive(true);
        else columne.SetActive(false);
    }
}
