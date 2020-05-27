using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
#pragma warning disable 0414
#endif

public class VuforiaDeviceOrientationDebug : MonoBehaviour {

	[Space(15)]
	public bool FrontCamera;
	[Space(15)]
	private bool FlipX, FlipX_check;
	private bool FlipY, FlipY_check;
	private ScreenOrientation Orient;
	private ColorBlock cb_wht, cb_grn;
	private string str_x, str_y;

	public Text text_xy;
	public Button NormalX, NormalY, NegativeX, NegativeY;

	void Start ()
	{
		FlipX = GetComponent<RegionCapture> ().FlipX;
		FlipY = GetComponent<RegionCapture> ().FlipY;

		cb_wht = new ColorBlock();
		cb_grn = new ColorBlock();

		cb_wht.normalColor = Color.white;
		cb_wht.pressedColor = Color.white;
		cb_wht.highlightedColor = Color.white;
		cb_wht.colorMultiplier = 1.0f;

		cb_grn.normalColor = Color.green;
		cb_grn.pressedColor = Color.green;
		cb_grn.highlightedColor = Color.green;
		cb_grn.colorMultiplier = 1.0f;

		if (FlipX) 
		{
			NormalX.colors = cb_wht;
			NegativeX.colors = cb_grn;
		}
		else 
		{
			NormalX.colors = cb_grn;
			NegativeX.colors = cb_wht;
		}
			
		if (FlipY)
		{
			NormalY.colors = cb_wht;
			NegativeY.colors = cb_grn;
		} 
		else
		{
			NormalY.colors = cb_grn;
			NegativeY.colors = cb_wht;
		}
	}


	public void ManualyFlipX (bool x)
	{
		FlipX = x;
		FlipX_check = x;
		GetComponent<RegionCapture> ().FlipX = x;


		if (FlipX) 
		{
			NormalX.colors = cb_wht;
			NegativeX.colors = cb_grn;
		}

		else 
		{
			NormalX.colors = cb_grn;
			NegativeX.colors = cb_wht;
		}
	}


	public void ManualyFlipY (bool y)
	{
		FlipY = y;
		FlipY_check = y;
		GetComponent<RegionCapture> ().FlipY = y;


		if (FlipY) 
		{
			NormalY.colors = cb_wht;
			NegativeY.colors = cb_grn;
		}

		else 
		{
			NormalY.colors = cb_grn;
			NegativeY.colors = cb_wht;
		}
	}


	void Update ()
	{
		#if !UNITY_EDITOR && !UNITY_STANDALONE

		FlipX_check = FlipX;
		FlipY_check = FlipY;


		if (FrontCamera)		// Front-Facing Camera
		{
			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.LandscapeRight) {
				FlipX = false;
				FlipY = false;
				Orient = Screen.orientation;
			}

			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.LandscapeLeft) {
				FlipX = true;
				FlipY = true;
				Orient = Screen.orientation;
			}

			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.Portrait) {
				FlipX = true;
				FlipY = false;
				Orient = Screen.orientation;
			}

			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
				FlipX = false;
				FlipY = true;
				Orient = Screen.orientation;
			}
			
			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.Portrait && Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
				FlipX = true;
				FlipY = false;
				Orient = Screen.orientation;
			}
		}

		else				// Back-Facing Camera (Rear-Facing)
		{
			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.LandscapeRight) {
				FlipX = true;
				FlipY = false;
				Orient = Screen.orientation;
			}

			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.LandscapeLeft) {
				FlipX = false;
				FlipY = true;
				Orient = Screen.orientation;
			}

			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.Portrait) {
				FlipX = true;
				FlipY = true;
				Orient = Screen.orientation;
			}

			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
				FlipX = false;
				FlipY = false;
				Orient = Screen.orientation;
			}

			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.Portrait && Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
				FlipX = true;
				FlipY = true;
				Orient = Screen.orientation;
			}
		}


		if (FlipX_check != FlipX || FlipY_check != FlipY)
		{

			if (FlipX_check != FlipX) 
			{
				if (FlipX) 
				{
					NormalX.colors = cb_wht;
					NegativeX.colors = cb_grn;
				}
				else 
				{
					NormalX.colors = cb_grn;
					NegativeX.colors = cb_wht;
				}
			}

			if (FlipY_check != FlipY)
			{
				if (FlipY)
				{
					NormalY.colors = cb_wht;
					NegativeY.colors = cb_grn;
				} 
				else
				{
					NormalY.colors = cb_grn;
					NegativeY.colors = cb_wht;
				}
			}

			GetComponent<RegionCapture> ().FlipX = FlipX;
			GetComponent<RegionCapture> ().FlipY = FlipY;

		}
		#endif

		if (FlipX)
			str_x = "X FLIPPED";
		else str_x = "X NORMAL";

		if (FlipY)
			str_y = "Y FLIPPED";
		else str_y = "Y NORMAL";

		text_xy.text = str_x + "  |  " + str_y;
	}
}
