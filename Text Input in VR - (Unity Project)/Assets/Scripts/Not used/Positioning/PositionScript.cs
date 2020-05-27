using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionScript : MonoBehaviour
{
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        //transform.LookAt(camera.transform);
        transform.rotation.SetLookRotation(new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
       //transform.rotation.SetLookRotation(new Vector3(0, 0, 0));
        //transform.LookAt(camera.transform);
        //transform.position = new Vector3(0, -137, 720);
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        //transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
        //camera.transform.rotation * Vector3.up);
    }
}
