using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchYawRoll : MonoBehaviour
{
    protected int FrameCounter;
    private ILogging logging;

    void Start()
    {
        FrameCounter = 0;
    }

    public void SetListener(ILogging l)
    {
        this.logging = l;
    }

    void Update()
    {
        FrameCounter++;
        if (GlobalSettings.IsCurrentSceneVR && (FrameCounter == 10))
        {
            // yaw (Z), pitch (Y), roll (X) --> OnLogPitchYawRoll(float pitch, float yaw, float roll)
            logging.OnLogPitchYawRoll(transform.eulerAngles.y, transform.eulerAngles.z, transform.eulerAngles.x);
            FrameCounter = 0;
            // Debug.Log("YAW (Z): " + transform.eulerAngles.z + ", PITCH (Y): " + transform.eulerAngles.y + "ROLL (X): " + transform.eulerAngles.x);
        }
    }
}
