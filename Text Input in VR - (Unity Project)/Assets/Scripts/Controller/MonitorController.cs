using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MonitorController : MonoBehaviour, ITrackableEventHandler
{
    public GameObject cam;
    public GameObject imageTarget;
    public bool isCurved;

    private float initRotX;
    private Vector3 initPos;
    private Quaternion initRot;
    private TrackableBehaviour mTrackableBehaviour;
    private bool monitorSet;

    void Start()
    {
        monitorSet = false;
        mTrackableBehaviour = imageTarget.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        initRotX = transform.rotation.x;
        initPos = transform.position;
        initRot = transform.rotation;
        //transform.rotation = Quaternion.Euler(0, 180, 0);
        //transform.LookAt(cam.transform);
    }

    void Update()
    {
        //Debug.Log("Rot of Image Target: " + imageTarget.transform.rotation.x + ", " +
        //imageTarget.transform.rotation.y + ", " + imageTarget.transform.rotation.z);
        //transform.rotation = Quaternion.Euler(90, 180, 0);
        //transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
        //transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        //transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
        if(isCurved)
        {
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            //transform.position = initPos;
            //transform.rotation = initRot;
        }
        else 
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        //transform.LookAt(imageTarget.transform);
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED && !monitorSet)
        {
            transform.LookAt(cam.transform);
            monitorSet = true;
        }
    }
}
