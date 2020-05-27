using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	[RequireComponent(typeof(RectTransform))]
	public class MobileCursor: MonoBehaviour
	{
		private const float VISIBLE_IMAGE_RATIO = 0.6f;

		/// <summary>The RectTransform</summary>
		private RectTransform rectTransform;

		[SerializeField]
		private CanvasFrontRenderer cursorRenderer;

		public RectTransform RectTransform { get { return rectTransform; } }
		public CanvasFrontRenderer CursorRenderer { get { return cursorRenderer; } }

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			enabled = false;
		}

		private void OnEnable()
		{
			CursorRenderer.Show();
		}

		private void OnDisable()
		{
			CursorRenderer.Hide();
		}

		public void UpdateSize(Vector2 size)
		{
			rectTransform.sizeDelta = size;
			Sync();
		}

		public void UpdatePosition(Vector2 position)
		{
			rectTransform.anchoredPosition = position;
			Sync();
		}

		private void Sync()
		{
			cursorRenderer.RefreshCanvas(GetComponentInParent<Canvas>());
			cursorRenderer.SyncTransform(rectTransform);

			RectTransform cursorRendererTransform = cursorRenderer.GetComponent<RectTransform>();
			RectTransform cursorImageRenderer = cursorRendererTransform.GetChild(0).GetComponent<RectTransform>(); //The actual visible image
			cursorImageRenderer.sizeDelta = cursorRendererTransform.sizeDelta * VISIBLE_IMAGE_RATIO;
		}
	}
}
