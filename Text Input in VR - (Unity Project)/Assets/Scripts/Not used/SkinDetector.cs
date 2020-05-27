using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class SkinDetector : MonoBehaviour
{
    public GameObject surface;
    public bool is3DModus;

    private Texture2D texture;

    void Start()
    {
        texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        if (is3DModus)
        {
            //StartCoroutine(WaitForTexture());
        }
    }

    void Update()
    {
        if (is3DModus && surface.GetComponent<MeshRenderer>().material.mainTexture) 
        {
            Debug.Log("Skin Detection started....");
            if(texture)
            {
                Texture source = surface.GetComponent<MeshRenderer>().material.mainTexture;
                texture.Resize(source.width, source.height);
                texture.ReadPixels(new UnityEngine.Rect(0, 0, source.width, source.height), 0, 0);
                texture.Apply();
                Debug.Log("Skin Detection active....");
                Mat inputMat = OpenCvSharp.Unity.TextureToMat(texture);
                Mat resultMap = new Mat(inputMat.Rows, inputMat.Cols, MatType.CV_8U, 4);
                Mat tmp = new Mat(inputMat.Rows, inputMat.Cols, MatType.CV_8U, 1);
                Mat alpha = new Mat(inputMat.Rows, inputMat.Cols, MatType.CV_8U, 1);
                Mat hsvMat = new Mat();
                Mat rgbaMat = new Mat();
                Cv2.CvtColor(inputMat, hsvMat, ColorConversionCodes.BGR2HSV);
                Mat outputMat1 = new Mat();
                Mat outputMat2 = new Mat();
                Mat outputMat3 = new Mat();
                Mat outputMat4 = new Mat();
                Mat kernel = new Mat();
                Mat finalMat = new Mat();

                Scalar minHSV = new Scalar(0, 48, 80);
                Scalar maxHSV = new Scalar(20, 255, 255);

                Cv2.InRange(hsvMat, minHSV, maxHSV, outputMat1);

                Cv2.BitwiseAnd(inputMat, inputMat, finalMat, outputMat1);

                OpenCvSharp.Unity.MatToTexture(finalMat, texture);

                surface.GetComponent<MeshRenderer>().material.mainTexture = texture;
            }
        }
    }

    //private IEnumerator WaitForTexture()
    //{
    //    yield return new WaitForEndOfFrame();
    //    if (surface.GetComponent<MeshRenderer>().material.mainTexture)
    //    {
    //        Debug.Log("Skin Detection started....");
    //        texture = (Texture2D)surface.GetComponent<MeshRenderer>().material.mainTexture;
    //        Mat inputMat = OpenCvSharp.Unity.TextureToMat(texture);
    //        Mat resultMap = new Mat(inputMat.Rows, inputMat.Cols, MatType.CV_8U, 4);
    //        Mat tmp = new Mat(inputMat.Rows, inputMat.Cols, MatType.CV_8U, 1);
    //        Mat alpha = new Mat(inputMat.Rows, inputMat.Cols, MatType.CV_8U, 1);
    //        Mat hsvMat = new Mat();
    //        Mat rgbaMat = new Mat();
    //        Cv2.CvtColor(inputMat, hsvMat, ColorConversionCodes.BGR2HSV);
    //        Mat outputMat1 = new Mat();
    //        Mat outputMat2 = new Mat();
    //        Mat outputMat3 = new Mat();
    //        Mat outputMat4 = new Mat();
    //        Mat kernel = new Mat();
    //        Mat finalMat = new Mat();

    //        Scalar minHSV = new Scalar(0, 48, 80);
    //        Scalar maxHSV = new Scalar(20, 255, 255);

    //        Cv2.InRange(hsvMat, minHSV, maxHSV, outputMat1);

    //        Cv2.BitwiseAnd(inputMat, inputMat, finalMat, outputMat1);

    //        OpenCvSharp.Unity.MatToTexture(finalMat, texture);

    //        surface.GetComponent<MeshRenderer>().material.mainTexture = texture;
    //    }
    //}
}
