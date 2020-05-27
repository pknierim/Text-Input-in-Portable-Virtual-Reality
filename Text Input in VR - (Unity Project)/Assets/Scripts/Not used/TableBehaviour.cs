using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TableBehaviour : MonoBehaviour, ITrackableEventHandler
{
    public GameObject Table;
    public GameObject Target;
    private TrackableBehaviour mTrackableBehaviour;
    private TrackableBehaviour.Status status;

    private void Start()
    {
        mTrackableBehaviour = Target.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    private void Update()
    {
        if (status == TrackableBehaviour.Status.DETECTED ||
            status == TrackableBehaviour.Status.TRACKED ||
            status == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Table.transform.position = new Vector3(Table.transform.position.x, Target.transform.position.y, Table.transform.position.z);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        status = newStatus;
    }
}
