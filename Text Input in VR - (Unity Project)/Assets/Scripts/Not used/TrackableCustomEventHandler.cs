using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using AdvancedInputFieldPlugin;

public class TrackableCustomEventHandler : MonoBehaviour, ITrackableEventHandler
{
    public AdvancedInputField inputField;
    public GameObject imageTarget;
    private TrackableBehaviour mTrackableBehaviour;

    void Start()
    {
        mTrackableBehaviour = imageTarget.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED)
        {
            Debug.Log("Custom Trackable Event Handler found Target");
            inputField.Select();
        }
    }
}
