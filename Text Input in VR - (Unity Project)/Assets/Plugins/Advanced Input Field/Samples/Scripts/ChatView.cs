using System.Collections.Generic;
#if ADVANCEDINPUTFIELD_TEXTMESHPRO
using TMPro;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin.Samples
{
	public class ChatView: MonoBehaviour
	{
		public const int Y_SPACING = 40;

		[SerializeField]
		private AdvancedInputField messageInput;

		[SerializeField]
		private RectTransform messageBoxLeftPrefab;

		[SerializeField]
		private RectTransform messageBoxRightPrefab;

		private ScrollRect scrollRect;
		private Text errorLabel;
		private Canvas canvas;
		private Vector2 originalMessageInputPosition;
		private float keyboardHeight;

		private List<RectTransform> messageBoxes;

		public ScrollRect ScrollRect { get { return scrollRect; } }
		public AdvancedInputField MessageInput { get { return messageInput; } }

		public Canvas Canvas
		{
			get
			{
				if(canvas == null)
				{
					canvas = Object.FindObjectOfType<Canvas>();
				}

				return canvas;
			}
		}

		private void Awake()
		{
			messageBoxes = new List<RectTransform>();
			scrollRect = GetComponentInChildren<ScrollRect>();
			errorLabel = transform.Find("ErrorLabel").GetComponent<Text>();
#if ADVANCEDINPUTFIELD_TEXTMESHPRO
			errorLabel.gameObject.SetActive(false);
#else
			errorLabel.gameObject.SetActive(true);
#endif
		}

		public void UpdateOriginalMessageInputPosition()
		{
			originalMessageInputPosition = messageInput.RectTransform.anchoredPosition;
		}

		public void RestoreOriginalMessageInputPosition()
		{
			messageInput.RectTransform.anchoredPosition = originalMessageInputPosition;
		}

		public void UpdateKeyboardHeight(int keyboardHeight)
		{
			this.keyboardHeight = keyboardHeight;
			if(messageInput.Selected)
			{
				UpdateMessageInputPosition();
			}
		}

		public void UpdateMessageInputPosition()
		{
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA) //Move to top of keyboard on mobile
			Vector2 position = messageInput.RectTransform.anchoredPosition;
			position.y = keyboardHeight / Canvas.scaleFactor;
			messageInput.RectTransform.anchoredPosition = position;
#endif
		}

		public void UpdateChatHistorySize()
		{
			RectTransform messageInputTransform = MessageInput.RectTransform;
			RectTransform chatHistoryTransform = ScrollRect.GetComponent<RectTransform>();
			float messageInputTopY = GetAbsoluteTopY(messageInputTransform);
#if UNITY_EDITOR || UNITY_STANDALONE
			EmojiKeyboard emojiKeyboard = messageInputTransform.GetComponentInChildren<EmojiKeyboard>();
			RectTransform emojiKeyboardTransform = emojiKeyboard.GetComponent<RectTransform>();
			messageInputTopY = GetAbsoluteTopY(emojiKeyboardTransform);
#endif
			float chatHistoryBottomY = GetAbsoluteBottomY(chatHistoryTransform);
			float differenceY = chatHistoryBottomY - messageInputTopY;

			Vector2 sizeDelta = chatHistoryTransform.sizeDelta;
			sizeDelta.y += differenceY;
			chatHistoryTransform.sizeDelta = sizeDelta;
		}

		public float GetAbsoluteTopY(RectTransform rectTransform)
		{
			Vector3[] corners = new Vector3[4];
			rectTransform.GetWorldCorners(corners);

			float topY = corners[1].y;
			float normalizedBottomY = 0;
			if(Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				normalizedBottomY = topY / Screen.height;
			}
			else
			{
				Camera camera = Canvas.worldCamera;
				normalizedBottomY = (topY + camera.orthographicSize) / (camera.orthographicSize * 2);
			}

			return (normalizedBottomY * Canvas.pixelRect.height) / Canvas.scaleFactor;
		}

		public float GetAbsoluteBottomY(RectTransform rectTransform)
		{
			Vector3[] corners = new Vector3[4];
			rectTransform.GetWorldCorners(corners);

			float bottomY = corners[0].y;
			float normalizedBottomY = 0;
			if(Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				normalizedBottomY = bottomY / Screen.height;
			}
			else
			{
				Camera camera = Canvas.worldCamera;
				normalizedBottomY = (bottomY + camera.orthographicSize) / (camera.orthographicSize * 2);
			}

			return (normalizedBottomY * Canvas.pixelRect.height) / Canvas.scaleFactor;
		}

		public void AddMessageLeft(string message)
		{
			AddMessage(message, messageBoxLeftPrefab);
		}

		public void AddMessageRight(string message)
		{
			AddMessage(message, messageBoxRightPrefab);
		}

		private void AddMessage(string message, RectTransform prefab)
		{
			float y = 0;
			if(messageBoxes.Count > 0)
			{
				RectTransform lastMessageBox = messageBoxes[messageBoxes.Count - 1];
				y = lastMessageBox.anchoredPosition.y - lastMessageBox.rect.height;
				y -= Y_SPACING;
			}

#if ADVANCEDINPUTFIELD_TEXTMESHPRO
			RectTransform messageBox = CreateMessageBox(prefab);
			TextMeshProUGUI label = messageBox.GetComponentInChildren<TextMeshProUGUI>();
			label.text = message;

			Vector2 preferredLabelSize = new Vector2(label.preferredWidth, label.preferredHeight);
			Vector2 currentLabelSize = label.rectTransform.rect.size;
			Vector2 sizeDifference = preferredLabelSize - currentLabelSize;
			Vector2 messageBoxSize = messageBox.sizeDelta;
			messageBoxSize += sizeDifference;
			messageBox.sizeDelta = messageBoxSize;

			Vector2 position = messageBox.anchoredPosition;
			position.y = y;
			messageBox.anchoredPosition = position;

			messageBoxes.Add(messageBox);
			scrollRect.content.sizeDelta = new Vector2(0, Mathf.Abs(y - messageBox.rect.height));
			scrollRect.verticalNormalizedPosition = 0;
#endif
		}

		public RectTransform CreateMessageBox(RectTransform messageBoxPrefab)
		{
			RectTransform messageBox = Instantiate(messageBoxPrefab);
			Vector2 size = messageBox.sizeDelta;
			messageBox.SetParent(scrollRect.content);
			messageBox.localScale = Vector3.one;
			messageBox.localRotation = Quaternion.identity;
			messageBox.anchoredPosition = Vector2.zero;
			messageBox.sizeDelta = size;

			return messageBox;
		}
	}
}
