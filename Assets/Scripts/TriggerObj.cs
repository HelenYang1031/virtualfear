using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObj : MonoBehaviour
{
    public GameObject hit;
    public GameObject unhideObj;
    public bool destroyTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        unhideObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == hit)
        {
            if (unhideObj != null) unhideObj.SetActive(true);
            if (destroyTrigger) Destroy(this.gameObject);
        }
    }
}
