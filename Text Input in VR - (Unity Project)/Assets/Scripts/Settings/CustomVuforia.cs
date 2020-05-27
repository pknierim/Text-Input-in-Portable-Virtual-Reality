using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CustomVuforia : MonoBehaviour
{
    public void BackgroundVidOnOff()
    {
        VideoBackgroundManager.Instance.SetVideoBackgroundEnabled(!VideoBackgroundManager.Instance.VideoBackgroundEnabled);
    }
    public void VROnOff()
    {
        if (VuforiaConfiguration.Instance.DeviceTracker.TrackingMode == DeviceTracker.TRACKING_MODE.POSITIONAL)
        {
            VuforiaConfiguration.Instance.DeviceTracker.TrackingMode = DeviceTracker.TRACKING_MODE.ROTATIONAL;
            VideoBackgroundManager.Instance.SetVideoBackgroundEnabled(false);

            MixedRealityController.Instance.SetMode(MixedRealityController.Mode.VIEWER_VR);
        }
        else
        {
            VuforiaConfiguration.Instance.DeviceTracker.TrackingMode = DeviceTracker.TRACKING_MODE.POSITIONAL;
            VideoBackgroundManager.Instance.SetVideoBackgroundEnabled(true);

            MixedRealityController.Instance.SetMode(MixedRealityController.Mode.VIEWER_AR_DEVICETRACKER);
        }
    }
}
