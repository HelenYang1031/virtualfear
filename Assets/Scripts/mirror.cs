using UnityEngine;

public class mirror : MonoBehaviour
{
    public Transform playerposition;
    public Transform mirrorposition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerLocal = mirrorposition.InverseTransformDirection(playerposition.position);

    }
}
