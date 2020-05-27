using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Vuforia;
using Image = Vuforia.Image;

public class CameraAccess : MonoBehaviour
{
    public GameObject imageTarget;
    public Camera cam;
    public GameObject Plane;
    public bool TestMode;
    private Texture2D texture;
    private bool ParamsSet = false;

    // Only for Testing
    private float timeLeft = 10;

#if UNITY_EDITOR
    private Image.PIXEL_FORMAT mPixelFormat = Vuforia.Image.PIXEL_FORMAT.GRAYSCALE;
#elif UNITY_ANDROID
       private Image.PIXEL_FORMAT mPixelFormat =  Vuforia.Image.PIXEL_FORMAT.RGB888;
#elif UNITY_IOS
        private Image.PIXEL_FORMAT mPixelFormat =  Vuforia.Image.PIXEL_FORMAT.RGB888;
#endif

    private bool mFormatRegistered = false;
    private bool mAccessCameraImage = true;

    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Vuforia.VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        Vuforia.VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
    }

    /// <summary>
    /// Called when Vuforia is started
    /// </summary>
    private void OnVuforiaStarted()
    {
        // Try register camera image format
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            Debug.Log("Successfully registered pixel format " + mPixelFormat.ToString());
            mFormatRegistered = true;
        }
        else
        {
            Debug.LogError("Failed to register pixel format " + mPixelFormat.ToString() +
                "\n the format may be unsupported by your device;" +
                "\n consider using a different pixel format.");
            mFormatRegistered = false;
        }
    }

    /// <summary>
    /// Called each time the Vuforia state is updated
    /// </summary>
    private void OnTrackablesUpdated()
    {
        if (mFormatRegistered)
        {
            if (mAccessCameraImage)
            {
                Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
                if (image != null && image.IsValid())
                {
                    //Debug.Log("Resolution of original image: " + image.Width + " / " + image.Height);

                    //string imageInfo = mPixelFormat + " image: \n";
                    //imageInfo += " size: " + image.Width + " x " + image.Height + "\n";
                    //imageInfo += " bufferSize: " + image.BufferWidth + " x " + image.BufferHeight + "\n";
                    //imageInfo += " stride: " + image.Stride;

                    //Debug.Log(imageInfo);

                    byte[] pixels = image.Pixels;

                    if (pixels != null && pixels.Length > 0)
                    {
                        texture.Resize(image.Width, image.Height);
                        texture.LoadRawTextureData(pixels);
                        texture.Apply();

                        if (!ParamsSet)
                        {
                            Plane.transform.position = GameObject.Find("BackgroundPlane").transform.position;
                            Plane.transform.localScale = GameObject.Find("BackgroundPlane").transform.localScale;
                            Plane.transform.rotation = GameObject.Find("BackgroundPlane").transform.rotation;
                            Plane.GetComponent<MeshFilter>().sharedMesh = GameObject.Find("BackgroundPlane").GetComponent<MeshFilter>().sharedMesh;
                            Plane.GetComponent<MeshRenderer>().sharedMaterial = GameObject.Find("BackgroundPlane").GetComponent<MeshRenderer>().sharedMaterial;
                            ParamsSet = true;
                            //VideoBackgroundManager.Instance.SetVideoBackgroundEnabled(false);
                        }

                        Plane.GetComponent<MeshRenderer>().material.mainTexture = texture;

                        //if (TestMode)
                        //{
                        //    timeLeft -= Time.deltaTime;
                        //    if (timeLeft < 0)
                        //    {
                        //        //MixedRealityController.Instance.SetMode(MixedRealityController.Mode.VIEWER_VR);
                        //    } 
                        //    else 
                        //    {
                        //        Plane.transform.position = GameObject.Find("BackgroundPlane").transform.position;
                        //        Plane.transform.localScale = GameObject.Find("BackgroundPlane").transform.localScale;
                        //        Plane.transform.rotation = GameObject.Find("BackgroundPlane").transform.rotation;
                        //        Plane.GetComponent<MeshFilter>().mesh = GameObject.Find("BackgroundPlane").GetComponent<MeshFilter>().mesh;
                        //        Plane.GetComponent<MeshRenderer>().materials = GameObject.Find("BackgroundPlane").GetComponent<MeshRenderer>().materials;
                        //    }
                        //}

                        if (GameObject.FindWithTag("RegionCapture")) 
                        {
                            GameObject.FindWithTag("RegionCapture").GetComponent<RegionCapture>().Ready = true;
                            //GameObject.FindWithTag("RegionCapture").GetComponent<RegionCapture>().VideoBackgroundTexure = texture;
                        }
                        //renderTexture = new RenderTexture(texture.width / 2, texture.height / 2, 0);
                        //RenderTexture.active = renderTexture;
                        //Graphics.Blit(texture, renderTexture);
                    }
                }
            }
        }
    }

    // Testing: Turn on VR Mode after 10 sec to see what happens when BG Plane Behaviour is turned off
    private void Update()
    {
        
    }
}
