using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public int maxTime = 180;
    public TMP_Text timerText;
    public GameObject[] notObjs;

    private int time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Tick()
    {
        time -= 1;
        timerText.text = "Time: " + time.ToString();
        if (time <= 0)
        {
            Debug.Log("Times up");
            foreach (GameObject go in notObjs) go.SendMessage("TimesUp");

        }
        else Invoke("Tick", 1f);
    }

    public void StartTimer()
    {
        time = maxTime;
        Invoke("Tick", 1f);
        timerText.text = "Time: " + time.ToString();
        Debug.Log("Timer started");
    }

    public void StopTimer()
    {
        CancelInvoke("Tick");
        Debug.Log("Timer stopped");
    }
}
