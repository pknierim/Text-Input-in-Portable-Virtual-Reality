//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;
using UnityEngine.UI;
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
using System.Collections.Generic;
#endif

namespace AdvancedInputFieldPlugin
{
	/// <summary>The onscreen control for cut, copy, paste and select all operations</summary>
	[RequireComponent(typeof(RectTransform))]
	public class ActionBar: MonoBehaviour
	{
		/// <summary>The multiplier for screen width to calculate action bar width</summary>
		private const float SCREEN_WIDTH_SCALE = 0.95f;

		/// <summary>The multiplier for thumb height to calculate action bar height</summary>
		private const float THUMB_SIZE_SCALE = 0.6f;

		/// <summary>The RectTransform for the ActionBar that will be rendered last in the Canvas</summary>
		[SerializeField]
		private CanvasFrontRenderer actionBarRenderer;

		/// <summary>The button for the cut operation</summary>
		[SerializeField]
		private Button cutButton;

		/// <summary>The button for the copy operation</summary>
		[SerializeField]
		private Button copyButton;

		/// <summary>The button for the paste operation</summary>
		[SerializeField]
		private Button pasteButton;

		/// <summary>The button for the select all operation</summary>
		[SerializeField]
		private Button selectAllButton;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
		/// <summary>The RectTransform</summary>
		public RectTransform RectTransform { get; private set; }

		/// <summary>The max size of the ActionBar when all buttons are enabled</summary>
		private Vector2 fullSize;

		/// <summary>The size of a button</summary>
		private Vector2 buttonSize;

		/// <summary>The TextInputHandler for mobile</summary>
		public MobileTextInputHandler TextInputHandler { get; private set; }

		/// <summary>The InputField</summary>
		public AdvancedInputField InputField { get; private set; }

		/// <summary>The Canvas</summary>
		public Canvas Canvas { get { return InputField.Canvas; } }

		/// <summary>Indicates if the ActionBar is visible</summary>
		public bool Visible { get { return gameObject.activeInHierarchy; } }

		/// <summary>Indicates if the cut operation is enabled</summary>
		private bool cut;

		/// <summary>Indicates if the copy operation is enabled</summary>
		private bool copy;

		/// <summary>Indicates if the paste operation is enabled</summary>
		private bool paste;

		/// <summary>Indicates if the select all operation is enabled</summary>
		private bool selectAll;

		/// <summary>Initializes this class</summary>
		internal void Initialize(AdvancedInputField inputField, MobileTextInputHandler textInputHandler)
		{
			InputField = inputField;
			TextInputHandler = textInputHandler;

			if(Canvas != null)
			{
				UpdateSize(Canvas.scaleFactor);
				actionBarRenderer.Initialize();
			}
		}

		#region UNITY
		private void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
			gameObject.SetActive(false);
		}

		private void OnEnable()
		{
			actionBarRenderer.Show();
		}

		private void OnDisable()
		{
			actionBarRenderer.Hide();
		}
		#endregion

		/// <summary>Determines fullSize and buttonSize</summary>
		internal void UpdateSize(float canvasScaleFactor)
		{
			if(RectTransform == null)
			{
				RectTransform = GetComponent<RectTransform>();
			}

			int thumbSize = Util.DetermineThumbSize();
			float cursorSize = 0;
			if(thumbSize <= 0)
			{
				Debug.LogWarning("Unknown DPI");
				if(InputField.TextRenderer.ResizeTextForBestFit)
				{
					cursorSize = InputField.TextRenderer.FontSizeUsedForBestFit / canvasScaleFactor;
				}
				else
				{
					cursorSize = InputField.TextRenderer.FontSize / canvasScaleFactor;
				}
			}
			else
			{
				cursorSize = (thumbSize * THUMB_SIZE_SCALE) / canvasScaleFactor;
			}


			fullSize = new Vector2((Canvas.pixelRect.width / canvasScaleFactor) * SCREEN_WIDTH_SCALE, cursorSize);
			RectTransform.sizeDelta = fullSize;

			buttonSize = new Vector2(fullSize.x / 4, 0); //Max 4 buttons

			actionBarRenderer.RefreshCanvas(Canvas);
			actionBarRenderer.SyncTransform(RectTransform);
		}

		/// <summary>Shows the ActionBar</summary>
		/// <param name="cut">Indicates if the cut button should be enabled</param>
		/// <param name="copy">Indicates if the copy button should be enabled</param>
		/// <param name="paste">Indicates if the paste button should be enabled</param>
		/// <param name="selectAll">Indicates if the select all button should be enabled</param>
		internal void Show(bool cut, bool copy, bool paste, bool selectAll)
		{
			if(Visible && this.cut == cut && this.copy == copy && this.paste == paste && this.selectAll == selectAll)
			{
				return;
			}
			else
			{
				this.cut = cut;
				this.copy = copy;
				this.paste = paste;
				this.selectAll = selectAll;
			}

			gameObject.SetActive(true);

			if(Canvas != null)
			{
				UpdateSize(Canvas.scaleFactor);
			}

			UpdateButtons();
		}

		internal void UpdateButtons()
		{
			List<Button> activeButtons = new List<Button>();
			cutButton.gameObject.SetActive(cut);
			if(cut)
			{
				activeButtons.Add(cutButton);
			}

			copyButton.gameObject.SetActive(copy);
			if(copy)
			{
				activeButtons.Add(copyButton);
			}

			pasteButton.gameObject.SetActive(paste);
			if(paste)
			{
				activeButtons.Add(pasteButton);
			}

			selectAllButton.gameObject.SetActive(selectAll);
			if(selectAll)
			{
				activeButtons.Add(selectAllButton);
			}

			int length = activeButtons.Count;
			RectTransform.sizeDelta = new Vector2(buttonSize.x * (length + 0.01f), fullSize.y); //0.01, Add some spacing for drop shadow

			for(int i = 0; i < length; i++)
			{
				RectTransform rectTransform = activeButtons[i].GetComponent<RectTransform>();
				rectTransform.sizeDelta = buttonSize;
				rectTransform.anchoredPosition = new Vector2(buttonSize.x * i, 0);
			}

			if(Canvas != null)
			{
				actionBarRenderer.RefreshCanvas(Canvas);
				actionBarRenderer.SyncTransform(RectTransform);
			}
		}

		/// <summary>Changes the position of the ActionBar</summary>
		/// <param name="position">The new position of the ActionBar</param>
		internal void UpdatePosition(Vector2 position)
		{
			if(Canvas != null)
			{
				position.x = 0;
				RectTransform.anchoredPosition = position;
				RectTransform.SetAsLastSibling();

				actionBarRenderer.RefreshCanvas(Canvas);
				actionBarRenderer.SyncTransform(RectTransform);
			}
		}

		/// <summary>Hides the ActionBar</summary>
		internal void Hide()
		{
			gameObject.SetActive(false);
			actionBarRenderer.gameObject.SetActive(false);
		}
#endif
		/// <summary>Event callback when the cut button has been clicked</summary>
		public void OnCutClick()
		{
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
			TextInputHandler.OnCut();
#endif
		}

		/// <summary>Event callback when the copy button has been clicked</summary>
		public void OnCopyClick()
		{
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
			TextInputHandler.OnCopy();
#endif
		}

		/// <summary>Event callback when the paste button has been clicked</summary>
		public void OnPasteClick()
		{
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
			TextInputHandler.OnPaste();
#endif
		}

		/// <summary>Event callback when the select all button has been clicked</summary>
		public void OnSelectAllClick()
		{
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
			TextInputHandler.OnSelectAll();
#endif
		}
	}
}