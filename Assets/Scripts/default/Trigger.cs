using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject hit;
    public GameObject notObj;
    public string data;
    public bool destroyTrigger = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == hit)
        {
            if (notObj != null) notObj.SendMessage("Trigger", data);
            if (destroyTrigger) Destroy(this.gameObject);
        }
    }
}
