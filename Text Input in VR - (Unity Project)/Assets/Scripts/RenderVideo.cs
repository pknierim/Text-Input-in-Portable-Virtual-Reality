using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderVideo : MonoBehaviour
{

    public Camera RenderCamera;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForTexture());
    }

    private IEnumerator WaitForTexture()
    {
        yield return new WaitForEndOfFrame();

        if (RenderCamera && RenderCamera.targetTexture)
        {
            transform.GetComponent<MeshRenderer>().material.mainTexture = RenderCamera.targetTexture;
        }

        else StartCoroutine(WaitForTexture());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
