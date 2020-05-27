using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackRotation : MonoBehaviour
{
    private Vector3 rot;
    // Start is called before the first frame update
    void Start()
    {
        rot = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.eulerAngles != rot)
        {
            Debug.Log("WORLD ROTATION TRACK: " + transform.rotation.eulerAngles);
        }
    }
}
