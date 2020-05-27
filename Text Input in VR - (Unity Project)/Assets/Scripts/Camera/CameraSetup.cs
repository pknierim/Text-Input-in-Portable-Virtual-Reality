using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public Camera ARCamera;
    private GameObject Room;
    private GameObject Screen;
    private GameObject KeyboardTarget;
    private float LastTargetRotY;
   //private GameObject Table;

    void Start()
    {
        Room = GameObject.Find("Room");
        Screen = GameObject.Find("ScreenComponent");
        KeyboardTarget = GameObject.Find("Magic_Keyboard_2");
        //Table = GameObject.Find("TablePlate");
    }

    void Update()
    {
        Room = GameObject.Find("Room");
        Screen = GameObject.Find("ScreenComponent");
        KeyboardTarget = GameObject.Find("Magic_Keyboard_2");
        if (Input.GetKeyDown(KeyCode.F5))
        {
            RotateWorldToCamera();
        }
        //if (GlobalSettings.CurrentTracked)
        //{
        //    LastTargetRotY = KeyboardTarget.transform.eulerAngles.y;
        //    float rot = KeyboardTarget.transform.eulerAngles.y - Room.transform.eulerAngles.y;
        //    Room.transform.RotateAround(Vector3.zero, Vector3.up, rot);
        //}
        //Debug.Log("ROTATION TARGET: " + KeyboardTarget.transform.rotation.eulerAngles.y);
        //Debug.Log("ROTATION WORLD: " + Room.transform.rotation.eulerAngles.y);
    }

    public void RotateWorldToCamera()
    {
        float rot = ARCamera.transform.eulerAngles.y - Room.transform.eulerAngles.y;
        Room.transform.RotateAround(Vector3.zero, Vector3.up, rot);
    }

    public void RotateWorldToTarget()
    {
        float rot = KeyboardTarget.transform.eulerAngles.y - Room.transform.eulerAngles.y;
        Room.transform.RotateAround(Vector3.zero, Vector3.up, rot);
    }

    public void RotateScreenToTarget()
    {
        float rot = KeyboardTarget.transform.eulerAngles.y - Screen.transform.eulerAngles.y;
        Screen.transform.RotateAround(Vector3.zero, Vector3.up, rot);
    }
}
