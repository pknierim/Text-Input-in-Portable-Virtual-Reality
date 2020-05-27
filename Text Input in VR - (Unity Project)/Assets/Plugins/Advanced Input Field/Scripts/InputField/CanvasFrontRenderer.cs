using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	[RequireComponent(typeof(RectTransform))]
	public class CanvasFrontRenderer: MonoBehaviour
	{
		private RectTransform rectTransform;
		private Canvas canvas;
		private bool initialized;

		internal void Initialize()
		{
			rectTransform = GetComponent<RectTransform>();
			canvas = GetComponentInParent<Canvas>();

			if(canvas != null)
			{
				rectTransform.SetParent(canvas.transform);
				rectTransform.localScale = Vector3.one;
				initialized = true;
			}
		}

		internal void RefreshCanvas(Canvas canvas)
		{
			if(!initialized)
			{
				Initialize();
			}

			if(this.canvas != canvas && canvas != null)
			{
				this.canvas = canvas;
				rectTransform.SetParent(canvas.transform);
				rectTransform.localScale = Vector3.one;
			}
		}

		internal void SyncTransform(RectTransform actionBarRectTransform)
		{
			if(!initialized)
			{
				Initialize();
			}

			Vector3[] corners = new Vector3[4]; //BottomLeft, TopLeft, TopRight, BottomRight
			actionBarRectTransform.GetWorldCorners(corners);

			RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
			for(int i = 0; i < 4; i++)
			{
				corners[i] = canvasRectTransform.InverseTransformPoint(corners[i]);
			}

			Vector2 size = corners[2] - corners[0];
			Vector2 center = Vector3.Lerp(corners[0], corners[2], 0.5f);

			rectTransform.anchoredPosition = center;
			rectTransform.sizeDelta = size;
			rectTransform.SetAsLastSibling();
		}

		internal void Show()
		{
			gameObject.SetActive(true);
		}

		internal void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}
