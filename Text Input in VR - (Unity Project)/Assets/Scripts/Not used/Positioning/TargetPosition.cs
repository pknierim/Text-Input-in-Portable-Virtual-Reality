using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosition : MonoBehaviour
{

    public GameObject Reference;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(Reference.transform.position.x, Reference.transform.position.y + 0.156f, Reference.transform.position.z);
    }
}
