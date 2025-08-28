using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnStart : MonoBehaviour
{
    public GameObject[] removed;
    public GameObject[] hidden;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        foreach (GameObject go in hidden) go.SetActive(false);

        foreach (GameObject go in removed) Destroy(go,1);
  
    }
}
