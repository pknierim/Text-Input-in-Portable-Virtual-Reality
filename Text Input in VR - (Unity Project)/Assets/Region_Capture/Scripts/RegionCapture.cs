using UnityEngine;
using UnityEngine.Events;
using System.Collections;

#if UNITY_EDITOR
#pragma warning disable 0414
#endif

public class RegionCapture : MonoBehaviour
{
	public bool UseBackgroundPlane = true;
	public Texture VideoBackgroundTexure;
	public GameObject BackgroundPlane;
	private bool InitializeComplete;

	public UnityEvent OutOfBounds;
	public UnityEvent ReturnInBounds;

	private bool PlaneIsOutOfBounds;
	private bool OutOfBounds_State;
	public bool Check_OutOfBounds;

	public bool HideFromARCamera;
	public bool FlipX, FlipY;
	public Camera ARCamera;
    public bool Ready = false;

	Mesh RegionMesh;
	Vector3[] vertices;
	Vector2[] uvs, uvs_tmp;
	float KX, KY;


	void Start()
	{
		KX = 1.0f;
		KY = 1.0f;
		StartCoroutine(Start_Initialize());
	}


	private IEnumerator Start_Initialize()
	{
		yield return new WaitForEndOfFrame();
		Initialize();
	}


	private void Initialize()
	{
		if (GetComponent<MeshFilter>())
		{
			RegionMesh = GetComponent<MeshFilter>().mesh;
			vertices = RegionMesh.vertices;
			uvs = new Vector2[vertices.Length];
			uvs_tmp = new Vector2[vertices.Length];
		}
		else Debug.Log("MeshFilter could not be found");

		if (!ARCamera && GameObject.Find("ARCamera")) ARCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
		if (!ARCamera && GameObject.FindWithTag("MainCamera")) ARCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

		if (!ARCamera) Debug.Log("ARCamera could not be found");
		else if (HideFromARCamera) ARCamera.cullingMask &= ~(1 << gameObject.layer);

		if (UseBackgroundPlane)
		{
			if (!BackgroundPlane) BackgroundPlane = GameObject.Find("BackgroundPlane");

			if (BackgroundPlane && BackgroundPlane.GetComponent<MeshFilter>() && BackgroundPlane.GetComponent<MeshRenderer>())
			{
				VideoBackgroundTexure = BackgroundPlane.GetComponent<MeshRenderer>().material.mainTexture;
			}
		}

		if (!ARCamera || !RegionMesh || (UseBackgroundPlane && !BackgroundPlane) || !VideoBackgroundTexure || VideoBackgroundTexure.width == 0)
		{
			StartCoroutine(Start_Initialize());
		}
		else
		{
			GetComponent<MeshRenderer>().material.mainTexture = VideoBackgroundTexure;
			InitializeComplete = true;

			if (UseBackgroundPlane && BackgroundPlane)
			{
				StartCoroutine(OutOfBounds_Check_Timer());
			}
			StartCoroutine(MeshUpdate_Check_Timer());
		}
	}


	private IEnumerator OutOfBounds_Check_Timer()           // 10 frames per second
	{
		yield return new WaitForSeconds(0.1f);
		OutOfBounds_Dispatcher();
	}

	private void OutOfBounds_Dispatcher()
	{
		if (InitializeComplete)
		{
			if (UseBackgroundPlane && BackgroundPlane)
			{
				FindBackgroundPlaneBounds(BackgroundPlane);
			}

			if (Check_OutOfBounds)
			{
				On_OutOfBounds();
			}

			StartCoroutine(OutOfBounds_Check_Timer());
		}
	}


	private IEnumerator MeshUpdate_Check_Timer()                // 100 frames per second
	{
		yield return new WaitForSeconds(0.01f);
		MeshUpdate();
	}

	private void MeshUpdate()
	{
		if (InitializeComplete)
		{
			bool CheckComplete = false;

			for (int i = 0; i < uvs.Length; i++)
			{
				uvs[i] = ARCamera.WorldToViewportPoint(transform.TransformPoint(vertices[i]));

				uvs[i].x = (uvs[i].x - 0.5f) * KX + 0.5f;
				uvs[i].y = (uvs[i].y - 0.5f) * KY + 0.5f;

				if (FlipX)
					uvs[i].x = 1.0f - uvs[i].x;
				if (FlipY)
					uvs[i].y = 1.0f - uvs[i].y;

#if !UNITY_EDITOR && !UNITY_STANDALONE

				if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
				{
					uvs_tmp[i].x = uvs[i].y;
					uvs_tmp[i].y = uvs[i].x;

					uvs[i].x = uvs_tmp[i].x;
					uvs[i].y = uvs_tmp[i].y;
				}

#endif

				if (Check_OutOfBounds && !CheckComplete)
				{
                    if (uvs[i].x > 1.0f || uvs[i].y > 1.0f || uvs[i].x < 0.0f || uvs[i].y < 0.0f)
					{
						PlaneIsOutOfBounds = true;
						CheckComplete = true;
                        //GlobalSettings.VideoOutOfBounds = true;
                    }
					else
					{
						PlaneIsOutOfBounds = false;
                        //GlobalSettings.VideoOutOfBounds = false;
                    }
                    float yCompare = GlobalSettings.CurrentTracked ? 1.5f : 1.0f;
                    float xCompare = GlobalSettings.CurrentTracked ? 1.0f : 0.5f;
                    if (uvs[i].x > xCompare || uvs[i].y > yCompare || uvs[i].x < 0.0f || uvs[i].y < 0.0f)
                    {
                        GlobalSettings.VideoOutOfBounds = true;
                    }
                    else
                    {
                        GlobalSettings.VideoOutOfBounds = false;
                    }
                }
			}
			RegionMesh.uv = uvs;

			StartCoroutine(MeshUpdate_Check_Timer());
		}
	}


	private void On_OutOfBounds()
	{
		if (OutOfBounds_State != PlaneIsOutOfBounds)
		{
            if (PlaneIsOutOfBounds)
			{
				if (this.enabled == false) return;
				if (OutOfBounds != null)
				{
					OutOfBounds.Invoke();
				}
			}

			else
			{
				if (this.enabled == false) return;
				if (ReturnInBounds != null)
				{
					ReturnInBounds.Invoke();
				}
			}
		}

		OutOfBounds_State = PlaneIsOutOfBounds;
	}


	private void FindBackgroundPlaneBounds(GameObject plane)
	{
        Vector3[] vertices_bg_tmp = plane.GetComponent<MeshFilter>().mesh.vertices;
		Vector2[] uvs_bg_tmp = new Vector2[vertices_bg_tmp.Length];

        float max_x_tmp = 0;
		float max_y_tmp = 0;
		float min_x_tmp = 0;
		float min_y_tmp = 0;

		for (int i = 0; i < uvs_bg_tmp.Length; i++)
		{
			uvs_bg_tmp[i] = ARCamera.WorldToViewportPoint(plane.transform.TransformPoint(vertices_bg_tmp[i]));

			if (uvs_bg_tmp[i].x > max_x_tmp) max_x_tmp = uvs_bg_tmp[i].x;
			if (uvs_bg_tmp[i].y > max_y_tmp) max_y_tmp = uvs_bg_tmp[i].y;
			if (uvs_bg_tmp[i].x < min_x_tmp) min_x_tmp = uvs_bg_tmp[i].x;
			if (uvs_bg_tmp[i].y < min_y_tmp) min_y_tmp = uvs_bg_tmp[i].y;
		}

		KX = (1.0f / (((max_x_tmp - 1.0f) * 2.0f) + 1.0f));
		KY = (1.0f / (((max_y_tmp - 1.0f) * 2.0f) + 1.0f));
	}
}
