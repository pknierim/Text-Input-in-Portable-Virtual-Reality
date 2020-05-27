#if !UNITY_EDITOR && UNITY_WSA

#if ENABLE_WINMD_SUPPORT
using System.Runtime.InteropServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace AdvancedInputFieldPlugin
{
	/// <summary>Class that acts as a bridge for the Native UWP Keyboard</summary>
	public class UWPKeyboard: NativeKeyboard, NativeUWP.IUnityCallback
	{
		private NativeUWP.NativeKeyboard keyboard;

#if ENABLE_WINMD_SUPPORT
	[DllImport("__Internal")]
	extern static int GetPageContent([MarshalAs(UnmanagedType.IInspectable)]object frame, [MarshalAs(UnmanagedType.IInspectable)]out object pageContent);
#endif

		internal override void Setup()
		{
			keyboard = new NativeUWP.NativeKeyboard();

			UnityEngine.WSA.Application.InvokeOnUIThread(() =>
			{
#if ENABLE_WINMD_SUPPORT
				Panel panel = TryGetPanel();
				if(panel == null)
				{
					panel = TryGetIl2CppPanel();
				}

				keyboard.Initialize(this, panel);
#endif
			}, false);
		}

#if ENABLE_WINMD_SUPPORT
		internal Panel TryGetPanel()
		{
			Panel panel = null;

			Window window = Window.Current;
			Frame frame = window.Content as Frame;
			if(frame == null) { return null; }

			Page page = frame.Content as Page;
			if(page == null) { return null; }

			panel = page.Content as Panel;
			return panel;
		}

		internal Panel TryGetIl2CppPanel()
		{
			Panel panel = null;

			object pageContent;
			var result = GetPageContent(Window.Current.Content, out pageContent);
			if(result < 0)
			{
				Marshal.ThrowExceptionForHR(result);
			}

			var dxSwapChainPanel = pageContent as Windows.UI.Xaml.Controls.SwapChainPanel;
			panel = pageContent as Panel;
			return panel;
		}
#endif

		public override void EnableUpdates()
		{
			keyboard.EnableUpdates();
		}

		public override void DisableUpdates()
		{
			keyboard.DisableUpdates();
		}

		public override void EnableHardwareKeyboardUpdates()
		{
			keyboard.EnableHardwareKeyboardUpdates();
		}

		public override void DisableHardwareKeyboardUpdates()
		{
			keyboard.DisableHardwareKeyboardUpdates();
		}

		public override void ShowKeyboard(string text, KeyboardType keyboardType, CharacterValidation characterValidation, LineType lineType, AutocapitalizationType autocapitalizationType, bool autocorrection, bool secure, bool emojisAllowed, int characterLimit, string characterValidatorJSON)
		{
			keyboard.ShowKeyboard(text, (int)keyboardType, (int)characterValidation, (int)lineType, (int)autocapitalizationType, autocorrection, secure, emojisAllowed, characterLimit, characterValidatorJSON);
		}

		public override void HideKeyboard()
		{
			keyboard.HideKeyboard();
		}

		public override void ChangeText(string text)
		{
			keyboard.ChangeText(text);
		}

		public override void ChangeSelection(int selectionStartPosition, int selectionEndPosition)
		{
			keyboard.ChangeSelection(selectionStartPosition, selectionEndPosition);
		}

		private void OnDestroy()
		{
			keyboard.Destroy();
			keyboard = null;
		}

		public void OnSelectionChanged(int start, int end)
		{
			nativeEventQueue.Enqueue(new Event(EventType.SELECTION_CHANGE, start + ", " + end));
		}

		public void OnKeyboardHeightChanged(int height)
		{
			UWPThreadHelper.Instance.ScheduleActionOnUnityThread((keyboardHeight) =>
			{
				InvokeKeyboardHeightChanged(keyboardHeight);

				if(keyboardHeight == 0) //Safety check if something external caused the keyboard to hide
				{
					State = KeyboardState.HIDDEN;
				}
			},
			height);
		}

		public void OnHardwareKeyboardChanged(bool connected)
		{
			UWPThreadHelper.Instance.ScheduleActionOnUnityThread((keyboardConnected) =>
			{
				HardwareKeyboardConnected = keyboardConnected;
				if(HardwareKeyboardConnected)
				{
					HideKeyboard();
					DisableUpdates();
				}
				else
				{
					EnableUpdates();
				}

				InvokeHardwareKeyboardChanged(connected);
			},
			connected);
		}

		public void DebugLine(string message)
		{
			UnityEngine.Debug.Log(message);
		}
	}
}
#endif
