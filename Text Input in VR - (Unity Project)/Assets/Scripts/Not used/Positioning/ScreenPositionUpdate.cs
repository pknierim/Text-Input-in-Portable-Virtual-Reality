using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ScreenPositionUpdate : MonoBehaviour, ITrackableEventHandler
{
    public GameObject screen;
    public GameObject imageTarget;
    private TrackableBehaviour mTrackableBehaviour;
    private TrackableBehaviour.Status status;


    // Start is called before the first frame update
    void Start()
    {
        mTrackableBehaviour = imageTarget.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (imageTarget)
        {
            Debug.Log("Setting screen: " + imageTarget.transform.position.x + ", " + (imageTarget.transform.position.y - 100) + ", " + imageTarget.transform.position.z);
            screen.transform.position = new Vector3(imageTarget.transform.position.x, imageTarget.transform.position.y-100, imageTarget.transform.position.z);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        Debug.Log("Screen Marker: " + newStatus);
        status = newStatus;
    }
}
