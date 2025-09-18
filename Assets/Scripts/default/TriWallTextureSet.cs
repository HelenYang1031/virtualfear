using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriWallTextureSet : MonoBehaviour
{
    public bool useBlueTex = true;
    public bool useBlueF1 = true;
    public bool useBlueB1 = true;
    [Header(" ")]
    public bool useGreenTex = true;
    public bool useGreenF1 = true;
    public bool useGreenB1 = true;
    [Header(" ")]
    public bool useRedTex = true;
    public bool useRedF1 = true;
    public bool useRedB1 = true;

    [Header("Don't change these")]
    [SerializeField] GameObject blueFrontTex1;
    [SerializeField] GameObject blueFrontTex2;
    [SerializeField] GameObject blueBackTex1;
    [SerializeField] GameObject blueBackTex2;
    [SerializeField] GameObject greenFrontTex1;
    [SerializeField] GameObject greenFrontTex2;
    [SerializeField] GameObject greenBackTex1;
    [SerializeField] GameObject greenBackTex2;
    [SerializeField] GameObject redFrontTex1;
    [SerializeField] GameObject redFrontTex2;
    [SerializeField] GameObject redBackTex1;
    [SerializeField] GameObject redBackTex2;


    // Start is called before the first frame update
    void Start()
    {
        SetupTextures();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupTextures()
    {
        blueFrontTex1.SetActive(useBlueTex && useBlueF1);
        blueFrontTex2.SetActive(useBlueTex && !useBlueF1);
        blueBackTex1.SetActive(useBlueTex && useBlueB1);
        blueBackTex2.SetActive(useBlueTex && !useBlueB1);

        greenFrontTex1.SetActive(useGreenTex && useGreenF1);
        greenFrontTex2.SetActive(useGreenTex && !useGreenF1);
        greenBackTex1.SetActive(useGreenTex && useGreenB1);
        greenBackTex2.SetActive(useGreenTex && !useGreenB1);

        redFrontTex1.SetActive(useRedTex && useRedF1);
        redFrontTex2.SetActive(useRedTex && !useRedF1);
        redBackTex1.SetActive(useRedTex && useRedB1);
        redBackTex2.SetActive(useRedTex && !useRedB1);
    }
}
