//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using System.Collections;
using UnityEngine;

namespace AdvancedInputFieldPlugin.Samples
{
	public class ChatControl: MonoBehaviour
	{
		[SerializeField]
		private ChatView view;

		private ChatBot chatBot;

		private void Awake()
		{
			chatBot = new ChatBot();
		}

		private void Start()
		{
			view.AddMessageLeft("Hello");
			view.UpdateChatHistorySize();

			EmojiKeyboard emojiKeyboard = view.MessageInput.GetComponentInChildren<EmojiKeyboard>();
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
			NativeKeyboardManager.AddKeyboardHeightChangedListener(OnKeyboardHeightChanged);
			emojiKeyboard.gameObject.SetActive(false);
#else
			emojiKeyboard.gameObject.SetActive(true);
#endif
		}

		public void OnMessageInputBeginEdit(BeginEditReason reason)
		{
			Debug.Log("OnMessageInputBeginEdit");
			view.UpdateOriginalMessageInputPosition();
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
			OnMessageInputSizeChanged(view.MessageInput.Size); //Move to top of keyboard on mobile on begin edit
#endif
		}

		public void OnMessageInputEndEdit(string result, EndEditReason reason)
		{
			Debug.Log("OnMessageInputEndEdit");
			view.RestoreOriginalMessageInputPosition();
		}

		public void OnMessageInputSizeChanged(Vector2 size)
		{
			Debug.Log("OnMessageInputSizeChanged: " + size);
			view.UpdateMessageInputPosition();
			view.UpdateChatHistorySize();
		}

		public void OnMessageSendClick()
		{
			Debug.Log("OnMessageSendClick");
			string message = view.MessageInput.Text;
			if(!string.IsNullOrEmpty(message))
			{
				view.AddMessageRight(message);
				view.MessageInput.Clear();
				StartCoroutine(ResponseRoutine());
			}
		}

		private IEnumerator ResponseRoutine()
		{
			yield return new WaitForSeconds(Random.Range(1, 5));

			string response = chatBot.GetResponse();
			view.AddMessageLeft(response);
		}

		public void OnKeyboardHeightChanged(int keyboardHeight)
		{
			view.UpdateKeyboardHeight(keyboardHeight);
			view.UpdateChatHistorySize();
		}
	}
}
