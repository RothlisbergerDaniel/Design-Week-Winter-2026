using System;
using UnityEngine;

public class FanPlatform : MonoBehaviour
{
    public GameObject platform;
    public Vector3 start; // starting position of platform relative to the fan
    public Vector3 target; // target position of platform relative to the fan
    public float travelTime; // how long it takes the platform to reach the target after being activated
    public float returnTime; // how long it takes the platform to return to start (either when sucked or automatically)
    public bool autoReturn; // if the platform should return on its own

    public Animator fanAnimator;

    [NonSerialized]
    public float travelTimer;
    [NonSerialized]
    public float returnTimer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platform.transform.localPosition = start;
        travelTimer = 0;
        returnTimer = 0;
        fanAnimator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void FixedUpdate()
    {
        if (autoReturn && travelTimer <= 0)
        {
            returnTimer = returnTime * getTravelTimer();
        }
        if (autoReturn) { returnTimer = Mathf.Max(0, returnTimer - Time.fixedDeltaTime); }

        if (returnTimer > 0)
        {
            doTravel(Mathf.Clamp(returnTimer / returnTime, 0, 1));
        }

        if (travelTimer > 0)
        {
            doTravel(1 - (Mathf.Clamp(travelTimer / travelTime, 0, 1)));
            travelTimer = travelTimer - Time.fixedDeltaTime;
        }
        
    }

    public void doTravel(float percent)
    {
        Vector3 tPos = new Vector3();
        tPos.x = start.x + ((target.x - start.x) * percent);
        tPos.y = start.y + ((target.y - start.y) * percent);
        tPos.z = start.z + ((target.z - start.z) * percent); // set target position
        tPos += transform.position;
        platform.transform.position = tPos;

        fanAnimator.speed = percent;
    }

    public float getTravelTimer()
    {
        Vector3 cPos = platform.transform.localPosition;
        float percent = Vector3.Magnitude((cPos - start)) / Vector3.Magnitude((target - start)); // we only need to check x since platforms move linearly


        return percent;
    }

    /*public void doReturn(float percent)
    {

    }*/
}
