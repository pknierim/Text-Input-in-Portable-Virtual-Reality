using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VuforiaCustomBehaviour : MonoBehaviour, ITrackableEventHandler
{
    public GameObject imageTarget;
    public GameObject Screen;
    private GameObject World;
    private GameObject Room;
    private TrackableBehaviour mTrackableBehaviour;
    private GameObject KeyboardImage;
    private GameObject TargetObject;
    private GameObject VideoPlane;
    private CameraSetup CameraSetup;
    private Vector3 initScreenPosition;
    private bool ScreenSet;
    private float TargetYPos;
    private Vector3 lastTargetPos;
    private Vector3 lastTargetRot;

    void Start()
    {
        //StartCoroutine(Test());
        ScreenSet = false;
        //initScreenPosition = new Vector3(Screen.transform.position.x, -33, Screen.transform.position.z);
        //Screen.transform.position = new Vector3(Screen.transform.position.x, Screen.transform.position.y - 200, Screen.transform.position.z);
        mTrackableBehaviour = imageTarget.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        Vuforia.VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        Vuforia.VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
    }

    private void Update()
    {
        if (GlobalSettings.KeepTargetAlive)
        {
            if (!GlobalSettings.CurrentTracked) 
            { 
                VideoPlane.gameObject.SetActive(false);
                //TargetObject.transform.rotation = World.transform.rotation;
                TargetObject.transform.rotation = Quaternion.Euler(lastTargetRot);
                TargetObject.transform.position = lastTargetPos;
            }
            else 
            {
                VideoPlane.gameObject.SetActive(true);
                TargetObject.transform.rotation = imageTarget.transform.rotation;
                TargetObject.transform.position = imageTarget.transform.position;
            }
            if (GlobalSettings.CheckAndHandleOutOfBounds)
            {
                if (GlobalSettings.VideoOutOfBounds) 
                { 
                    VideoPlane.gameObject.SetActive(false);
                }
                else 
                {
                    lastTargetPos = TargetObject.transform.position;
                    lastTargetRot = TargetObject.transform.rotation.eulerAngles;
                    VideoPlane.gameObject.SetActive(true);
                    if (GlobalSettings.InTask)
                    {
                        CameraSetup.RotateWorldToTarget();
                    }
                }
            }
        }
    }

    public IEnumerator Test()
    {
        yield return new WaitForSeconds(3);
        Screen.transform.position = initScreenPosition;
    }

    private void OnVuforiaStarted()
    {
        MixedRealityController.Instance.SetMode(MixedRealityController.Mode.VIEWER_AR_DEVICETRACKER);
        MixedRealityController.Instance.SetMode(MixedRealityController.Mode.VIEWER_VR);
        KeyboardImage = GameObject.Find("KeyboardImg");
        TargetObject = GameObject.Find("TargetComponents");
        World = GameObject.Find("World");
        Room = GameObject.Find("Room");
        VideoPlane = GameObject.Find("VideoCrop");
        CameraSetup = GetComponent<CameraSetup>();
    }

    private void OnTrackablesUpdated()
    {

    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        //Debug.Log("VUFORIA CUSTOM - " + newStatus);
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (TargetObject != null) TargetYPos = TargetObject.transform.position.y;
            MixedRealityController.Instance.SetMode(MixedRealityController.Mode.VIEWER_AR_DEVICETRACKER);
            VuforiaConfiguration.Instance.DeviceTracker.TrackingMode = DeviceTracker.TRACKING_MODE.POSITIONAL;
            Screen.SetActive(true);
            GlobalSettings.CurrentTracked = true;
            GlobalSettings.ImageTargetOnceTracked = true;
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            MixedRealityController.Instance.SetMode(MixedRealityController.Mode.VIEWER_VR);
            GlobalSettings.CurrentTracked = false;

        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            GlobalSettings.CurrentTracked = false;
        }
    }
}
