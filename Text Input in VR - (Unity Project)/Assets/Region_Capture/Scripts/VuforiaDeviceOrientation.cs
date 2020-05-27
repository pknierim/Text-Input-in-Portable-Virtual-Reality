using UnityEngine;

#if UNITY_EDITOR
#pragma warning disable 0414
#endif

public class VuforiaDeviceOrientation : MonoBehaviour {

	[Space(15)]
	public bool FrontCamera;
	[Space(15)]
	private bool FlipX, FlipX_check;
	private bool FlipY, FlipY_check;
	private ScreenOrientation Orient;

	void Start ()
	{
		FlipX = GetComponent<RegionCapture> ().FlipX;
		FlipY = GetComponent<RegionCapture> ().FlipY;
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

			if (Orient != Screen.orientation && Screen.orientation == ScreenOrientation.Portrait && Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown){
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
			GetComponent<RegionCapture> ().FlipX = FlipX;
			GetComponent<RegionCapture> ().FlipY = FlipY;
		}
		#endif
	}
}
