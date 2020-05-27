﻿//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;
using UnityEngine.UI;
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
using UnityEngine.EventSystems;
using System.Collections;
#endif

namespace AdvancedInputFieldPlugin
{
	/// <summary>Determines when to scroll when keyboard appears</summary>
	public enum KeyboardScrollMode { ONLY_SCROLL_IF_INPUTFIELD_BLOCKED, ALWAYS_SCROLL }

	/// <summary>Class to scroll to the currently selected InputField when te keyboard appears & hides, making sure that the InputField is not behind the keyboard</summary>
	[RequireComponent(typeof(ScrollArea))]
	public class KeyboardScroller: MonoBehaviour
	{
		private float SCROLL_FIX_SPEED = 0.1f;

		/// <summary>The KeyboardScrollMode</summary>
		[Tooltip("Determines when to scroll when keyboard appears")]
		[SerializeField]
		private KeyboardScrollMode keyboardScrollMode;

		/// <summary>The transition time to scroll to the target InputField</summary>
		[SerializeField]
		private float transitionTime = 0.5f;

		/// <summary>The normalized offset y used to add additional spacing between keyboard and target</summary>
		[Tooltip("The normalized offset y used to add additional spacing between keyboard and target")]
		[SerializeField]
		[Range(0f, 1f)]
		private float normalizedOffsetY = 0.025f;

		/// <summary>The ScrollRect</summary>
		private ScrollRect scrollRect;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
		/// <summary>The Canvas</summary>
		private Canvas canvas;

		/// <summary>The original size of the scrollable content</summary>
		private Vector2 originalContentSize;

		/// <summary>The start size of the scrollable content when starting the transition</summary>
		private Vector2 startContentSize;

		/// <summary>The end size of the scrollable content when starting the transition</summary>
		private Vector2 endContentSize;

		/// <summary>The start position of the scrollable content when starting the transition</summary>
		private Vector2 startContentPosition;

		/// <summary>The end position of the scrollable content when starting the transition</summary>
		private Vector2 endContentPosition;

		/// <summary>The current time in the transition</summary>
		private float currentTime;

		/// <summary>The last selected object</summary>
		private GameObject lastSelectedObject;

		/// <summary>The last known keyboard height</summary>
		private int lastKeyboardHeight;

		/// <summary>The Canvas</summary>
		public Canvas Canvas
		{
			get
			{
				if(canvas == null)
				{
					canvas = GetComponentInParent<Canvas>();
				}

				return canvas;
			}
		}

		#region UNITY
		private void Awake()
		{
			scrollRect = GetComponent<ScrollRect>();
		}

		private void Start()
		{
			NativeKeyboardManager.AddKeyboardHeightChangedListener(OnKeyboardHeightChanged);
			originalContentSize = scrollRect.content.sizeDelta;
			currentTime = transitionTime;
		}

		private void Update()
		{
			if(currentTime < transitionTime)
			{
				currentTime += Time.deltaTime;
				if(currentTime >= transitionTime)
				{
					currentTime = transitionTime;
					scrollRect.vertical = true; //Enable user scroll again
					if(lastKeyboardHeight == 0)
					{
						StartCoroutine(EnsureScrollWithinRange());
					}
				}

				float progress = currentTime / transitionTime;
				scrollRect.content.sizeDelta = Vector2.Lerp(startContentSize, endContentSize, progress);
				scrollRect.content.anchoredPosition = Vector2.Lerp(startContentPosition, endContentPosition, progress);
				TryUpdateCaretPosition();
			}

			if(lastKeyboardHeight > 0)
			{
				GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
				if(selectedObject != null && selectedObject != lastSelectedObject)
				{
					lastSelectedObject = selectedObject;

					if(selectedObject.GetComponent<AdvancedInputField>()) //Handle Next on mobile keyboard
					{
						ScrollContent(lastKeyboardHeight);
					}
				}
			}
		}
		#endregion

		private void TryUpdateCaretPosition()
		{
			if(lastSelectedObject == null) { return; }

			AdvancedInputField inputField = lastSelectedObject.GetComponent<AdvancedInputField>();
			if(inputField != null)
			{
				inputField.UpdateCaretPosition();
			}
		}

		private IEnumerator EnsureScrollWithinRange()
		{
			if(scrollRect.verticalNormalizedPosition < 0)
			{
				float scrollDifference = -scrollRect.verticalNormalizedPosition;
				while(scrollRect.verticalNormalizedPosition < 0)
				{
					float scrollMove = (scrollDifference * (Time.deltaTime / SCROLL_FIX_SPEED));
					scrollRect.verticalNormalizedPosition = scrollRect.verticalNormalizedPosition + scrollMove;
					yield return null;
				}

				scrollRect.verticalNormalizedPosition = 0; //Final scroll update
			}
			else if(scrollRect.verticalNormalizedPosition > 1)
			{
				float scrollDifference = scrollRect.verticalNormalizedPosition - 1;
				while(scrollRect.verticalNormalizedPosition > 1)
				{
					float scrollMove = (scrollDifference * (Time.deltaTime / SCROLL_FIX_SPEED));
					scrollRect.verticalNormalizedPosition = scrollRect.verticalNormalizedPosition - scrollMove;
					yield return null;
				}

				scrollRect.verticalNormalizedPosition = 1; //Final scroll update
			}


			while(scrollRect.verticalNormalizedPosition > 1)
			{
				scrollRect.verticalNormalizedPosition = scrollRect.verticalNormalizedPosition;
				yield return null;
			}
		}

		/// <summary>Sets current content size as the original content size</summary>
		public void RefreshOriginalContentSize()
		{
			originalContentSize = scrollRect.content.sizeDelta;
		}

		/// <summary>Gets the normalized vertical values of a RectTransform</summary>
		/// <param name="rectTransform">The RectTransform to use</param>
		/// <param name="normalizedY">The output normalized y</param>
		/// <param name="normalizedHeight">The output normalized height</param>
		private void GetNormalizedVertical(RectTransform rectTransform, out float normalizedY, out float normalizedHeight)
		{
			Vector3[] corners = new Vector3[4];
			rectTransform.GetWorldCorners(corners);

			float bottomY = corners[1].y;
			if(Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				normalizedY = bottomY / Screen.height;
			}
			else
			{
				Camera camera = Canvas.worldCamera;
				normalizedY = (bottomY + camera.orthographicSize) / (camera.orthographicSize * 2);
			}
			normalizedY -= normalizedOffsetY;

			Vector2 size = new Vector2(Mathf.Abs(corners[3].x - corners[1].x), Mathf.Abs(corners[3].y - corners[1].y));
			if(Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				normalizedHeight = size.y / Screen.height;
			}
			else
			{
				Camera camera = Canvas.worldCamera;
				normalizedHeight = size.y / (camera.orthographicSize * 2);
			}
		}

		/// <summary>Event callback when the keyboard height has changed</summary>
		/// <param name="keyboardHeight">The keyboard height</param>
		public void OnKeyboardHeightChanged(int keyboardHeight)
		{
			if(keyboardHeight > 0)
			{
				ScrollContent(keyboardHeight);
			}
			else
			{
				ScrollBack();
			}

			lastKeyboardHeight = keyboardHeight;
		}

		/// <summary>Start the scroll to the current InputField using keyboardHeight as the offset</summary>
		/// <param name="keyboardHeight">The keyboard height</param>
		public void ScrollContent(int keyboardHeight)
		{
			GameObject targetObject = EventSystem.current.currentSelectedGameObject;
			if(targetObject == null || targetObject.GetComponent<AdvancedInputField>() == null)
			{
				return;
			}

			if(targetObject.GetComponent<AdvancedInputField>() != null)
			{
				RectTransform scrollTarget = targetObject.GetComponent<RectTransform>();
				float normalizedY, normalizedHeight;
				GetNormalizedVertical(scrollTarget, out normalizedY, out normalizedHeight);
				float normalizedKeyboardHeight = keyboardHeight / (float)Screen.height;

				float moveY = (normalizedKeyboardHeight - (normalizedY - normalizedHeight)) * (Canvas.pixelRect.height / Canvas.scaleFactor);
				if(keyboardScrollMode == KeyboardScrollMode.ONLY_SCROLL_IF_INPUTFIELD_BLOCKED && moveY < 0)
				{
					return;
				}

				startContentPosition = scrollRect.content.anchoredPosition;
				endContentPosition = startContentPosition;
				endContentPosition.y += moveY;

				startContentSize = scrollRect.content.sizeDelta;
				endContentSize = originalContentSize * 3; //*3, so we have enough scroll space at the top and bottom
				currentTime = 0;
				scrollRect.vertical = false; //Disable user scroll when transitioning
			}
		}

		/// <summary>Scrolls and resizes content size to their original state</summary>
		public void ScrollBack()
		{
			startContentSize = scrollRect.content.sizeDelta;
			endContentSize = originalContentSize;
			startContentPosition = scrollRect.content.anchoredPosition;
			endContentPosition = scrollRect.content.anchoredPosition; //Keep forcing current position while scrollrect resizes
			currentTime = 0;
			scrollRect.vertical = false; //Disable user scroll when transitioning
		}
#endif
	}
}
