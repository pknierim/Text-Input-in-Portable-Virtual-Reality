using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPositioner : MonoBehaviour
{
    public GameObject camera;
    public GameObject reference;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(camera.transform.);
        //transform.LookAt(camera.transform);
        //transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
        //camera.transform.rotation * Vector3.up);
       //transform.rotation = reference.transform.rotation;
        //transform.rotation = Quaternion.Euler(new Vector3(reference.transform.rotation.x, reference.transform.rotation.y, reference.transform.rotation.z));
    }
}
