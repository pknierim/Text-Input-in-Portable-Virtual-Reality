using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraSettings : MonoBehaviour
{
    public bool EnableFlash;

    private GameObject BackgroundPlane;
    private float scaleX;
    private float scaleZ;

    // Start is called before the first frame update
    void Start()
    {
        var vuforia = VuforiaARController.Instance;
        vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        vuforia.RegisterOnPauseCallback(OnPaused);
        BackgroundPlane = GameObject.Find("BackgroundPlane");
    }

    private void Update()
    {
        if(GameObject.Find("ClippingPlane_top")){
            GameObject.Find("ClippingPlane_top").SetActive(false);
            GameObject.Find("ClippingPlane_bottom").SetActive(false);
            GameObject.Find("ClippingPlane_right").SetActive(false);
            GameObject.Find("ClippingPlane_left").SetActive(false);
        }
    }

    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (EnableFlash) CameraDevice.Instance.SetFlashTorchMode(true);
    }

    private void OnPaused(bool paused)
    {
        if (!paused) // resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
            CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }
}
