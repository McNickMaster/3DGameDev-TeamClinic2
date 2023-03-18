using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public static Cutscene instance;

    public Transform A, B;

    public float kp = 0.1f;

    private Vector3 targetPos;


    private bool trackStarted = false;

    void Awake()
    {
        instance = this;
        transform.position = A.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(trackStarted)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, kp * Time.deltaTime);
        }

        
    }

    void Update()
    {
        if(trackStarted)
        {
            if(Vector3.Distance(transform.position, targetPos) < 2)
            {
                StopTrack();
            }

        }
    }

    public void StartTrack()
    {
        
        targetPos = B.position;
        Invoke("FlagStart", 0.5f);
    }

    void FlagStart()
    {
        trackStarted = true;
    }

    void StopTrack()
    {
        Destroy(this.gameObject);
    }
}
