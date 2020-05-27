using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using Vuforia;
using Rect = UnityEngine.Rect;
using DigitalRuby.Threading;

public class RC_UI_Texture : MonoBehaviour, ITrackableEventHandler
{

    private Texture2D inputTexture;
    private Texture2D finalTexture;
    private Mat inputMat;
    private Mat hsvMat;
    private Mat rgbaMat;
    private Mat outputMat1;
    private Mat outputMat2;
    private Mat outputMat3;
    private Mat gray;
    private Mat alpha;
    private Mat kernel;
    private Mat finalMat;
    Mat[] rgb;
    Mat[] rgba = new Mat[4];
    private Scalar minHSV = new Scalar(0, 48, 80);
    private Scalar maxHSV = new Scalar(20, 255, 255);
    private int counter;
    private bool tracked;
    private TrackableBehaviour mTrackableBehaviour;

    public Camera RenderCamera;
    public GameObject imageTarget;
    public GameObject PlaneVideo;
    public GameObject Plane3D;
    public bool is3DModus;
    public bool highFiltering;
    private Texture2D KeyboardImageTexture;

    Texture2D tex;

    void Start () {
        mTrackableBehaviour = imageTarget.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        Vuforia.VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
        if (is3DModus)
        {
            Vuforia.VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
            // TODO: Try threading for OpenCV operations
            //EZThread.BeginThread(ExtractHandsFromTexture, true);
        }
        else 
        {
            StartCoroutine(WaitForTexture());
        }
        tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        KeyboardImageTexture = new Texture2D(0, 0, TextureFormat.RGB24, false);
        counter = 0;
    }

    private void Update()
    {
        if (RenderCamera && RenderCamera.targetTexture && is3DModus && tracked)
        {
            Resources.UnloadUnusedAssets();
            ExtractHandsFromTexture();
        }
    }

    private IEnumerator WaitForTexture() 
    {
        yield return new WaitForEndOfFrame ();

        if (RenderCamera && RenderCamera.targetTexture && !is3DModus && tracked)
        {
            PlaneVideo.GetComponent<MeshRenderer>().material.mainTexture = RenderCamera.targetTexture;
            KeyboardImageTexture = ToTexture2D(PlaneVideo.GetComponent<MeshRenderer>().material.mainTexture);
        }

        else StartCoroutine(WaitForTexture());
    }

    private void ExtractHandsFromTexture()
    {
        //Resources.UnloadUnusedAssets();
        //Debug.Log("Render Texture Resolution: " + RenderCamera.targetTexture.width + "/" + RenderCamera.targetTexture.height + " px" + "\n" +
        //RenderCamera.targetTexture.depth);
        if (is3DModus && RenderCamera && RenderCamera.targetTexture && tracked && counter % 1 == 0) //&& counter%0 == 0
        {
            inputTexture = ToTexture2D(RenderCamera.targetTexture);
            inputMat = OpenCvSharp.Unity.TextureToMat(inputTexture);
            hsvMat = new Mat();
            rgbaMat = new Mat();
            outputMat1 = new Mat();
            outputMat2 = new Mat();
            outputMat3 = new Mat();
            gray = new Mat();
            alpha = new Mat();
            kernel = new Mat();
            finalMat = new Mat();

            Cv2.CvtColor(inputMat, hsvMat, ColorConversionCodes.BGR2HSV);

            Cv2.InRange(hsvMat, minHSV, maxHSV, outputMat1);

            // Possible additional filtering
            //if (highFiltering)
            //{
            //    Mat addMat1 = new Mat();
            //    Mat addMat2 = new Mat();
            //    Mat addMat3 = new Mat();
            //    Mat addFinalMat = new Mat();
            //    Size size = new Size(11, 11);
            //    kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, size);
            //    Cv2.Erode(outputMat1, addMat1, kernel, iterations: 2);
            //    Cv2.Dilate(addMat1, addMat2, kernel, iterations: 2);
            //    Size sizeKernel = new Size(3, 3);
            //    Cv2.GaussianBlur(addMat2, addMat3, sizeKernel, 0);
            //    Cv2.BitwiseAnd(inputMat, inputMat, addFinalMat, addMat3);
            //}

            Cv2.BitwiseAnd(inputMat, inputMat, outputMat2, outputMat1);

            Cv2.CvtColor(outputMat2, outputMat3, ColorConversionCodes.HSV2BGR);
            Cv2.CvtColor(outputMat3, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(gray, alpha, 0, 255, ThresholdTypes.Binary);

            rgb = new Mat[3];

            Cv2.Split(outputMat2, out rgb);
            rgba[0] = rgb[0];
            rgba[1] = rgb[1];
            rgba[2] = rgb[2];
            rgba[3] = alpha;
            Cv2.Merge(rgba, finalMat);

            finalTexture = OpenCvSharp.Unity.MatToTexture(finalMat);
            Plane3D.GetComponent<MeshRenderer>().material.mainTexture = finalTexture;
        }
        counter += 1;
    }

    private void OnTrackablesUpdated()
    {

    }

    Texture2D ToTexture2D(RenderTexture rTex)
    {
        tex.Resize(rTex.width, rTex.height);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    Texture2D ToTexture2D(Texture rTex)
    {
        tex.Resize(rTex.width, rTex.height);
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public Texture2D GetKeyboardTexture()
    {
        return KeyboardImageTexture;
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        tracked = newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED
            ? true
            : false;
    }
}
