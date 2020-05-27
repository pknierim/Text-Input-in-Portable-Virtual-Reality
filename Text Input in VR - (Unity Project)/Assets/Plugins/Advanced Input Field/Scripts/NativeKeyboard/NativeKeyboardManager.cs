//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Access point for the NativeKeyboard for current platform</summary>
	public class NativeKeyboardManager: MonoBehaviour
	{
		/// <summary>The singleton instance of NativeKeyboardManager</summary>
		private static NativeKeyboardManager instance;

		/// <summary>The NativeKeyboard instance of current platform</summary>
		private NativeKeyboard keyboard;

		/// <summary>The singleton instance of NativeKeyboardManager</summary>
		private static NativeKeyboardManager Instance
		{
			get
			{
				if(instance == null)
				{
					instance = GameObject.FindObjectOfType<NativeKeyboardManager>();
					if(instance == null)
					{
						GameObject gameObject = new GameObject("NativeKeyboardManager");
						instance = gameObject.AddComponent<NativeKeyboardManager>();
					}
				}

				return instance;
			}
		}

		/// <summary>The TouchScreenKeyboard instance of current platform</summary>
		public static NativeKeyboard Keyboard
		{
			get { return Instance.keyboard; }
		}

		/// <summary>Indicates whether a hardware keyboard is connected</summary>
		public static bool HardwareKeyboardConnected
		{
			get { return Instance.keyboard.HardwareKeyboardConnected; }
		}

		#region UNITY
		private void Awake()
		{
#if UNITY_EDITOR
			Debug.LogWarning("Native Keyboard can't be accessed in the Editor");
#elif UNITY_ANDROID
			keyboard = gameObject.AddComponent<AndroidKeyboard>();
			keyboard.Init(name);
#elif UNITY_IOS
			keyboard = gameObject.AddComponent<IOSKeyboard>();
			keyboard.Init(name);
#elif UNITY_WSA
			keyboard = gameObject.AddComponent<UWPKeyboard>();
			keyboard.Init(name);
#else
			Debug.LogWarning("Native Keyboard is only supported on Android, iOS and UWP");
#endif
		}

		private void OnDestroy()
		{
			instance = null;
		}
		#endregion

		public static void TryDestroy()
		{
			if(instance != null && instance.gameObject != null)
			{
				Destroy(instance.gameObject);
			}
		}

		/// <summary>Checks whether the native binding should be active or not</summary>
		public static void UpdateKeyboardActiveState()
		{
			Instance.keyboard.UpdateActiveState();
		}

		/// <summary>
		/// Enables hardware keyboard connectivity checks in the native binding.
		/// Use this when you want connectivity checks when no inputfield is selected.
		/// </summary>
		public static void EnableHardwareKeyboardUpdates()
		{
			Instance.keyboard.EnableHardwareKeyboardUpdates();
		}

		/// <summary>
		/// Disables hardware keyboard connectivity checks in the native binding.
		/// Use this when you want to disable connectivity checks after using EnableHardwareKeyboardUpdates.
		/// </summary>
		public static void DisableHardwareKeyboardUpdates()
		{
			Instance.keyboard.DisableHardwareKeyboardUpdates();
		}

		/// <summary>Shows the TouchScreenKeyboard for current platform</summary>
		/// <param name="text">The current text of the InputField</param>
		/// <param name="keyboardType">The keyboard type to use</param>
		/// <param name="characterValidation">The characterValidation to use</param>
		/// <param name="lineType">The lineType to use</param>
		/// <param name="autocorrection">Indicates whether autocorrection is enabled</param>
		/// <param name="characterLimit">The character limit for the text</param>
		/// <param name="secure">Indicates whether input should be secure</param>
		public static void ShowKeyboard(string text, KeyboardType keyboardType = KeyboardType.Default, CharacterValidation characterValidation = CharacterValidation.None, LineType lineType = LineType.SingleLine, AutocapitalizationType autocapitalizationType = AutocapitalizationType.NONE, bool autocorrection = true, bool secure = false, bool emojisAllowed = false, int characterLimit = 0, string characterValidatorJSON = null)
		{
			Instance.keyboard.ShowKeyboard(text, keyboardType, characterValidation, lineType, autocapitalizationType, autocorrection, secure, emojisAllowed, characterLimit, characterValidatorJSON);
		}

		/// <summary>Hides the TouchScreenKeyboard for current platform</summary>
		public static void HideKeyboard()
		{
			Instance.keyboard.HideKeyboard();
		}

		/// <summary>Adds a KeyboardHeightChangedListener</summary>
		/// <param name="listener">The KeyboardHeightChangedListener to add</param>
		public static void AddKeyboardHeightChangedListener(OnKeyboardHeightChangedHandler listener)
		{
			Instance.keyboard.AddKeyboardHeightChangedListener(listener);
		}

		/// <summary>Removes a KeyboardHeightChangdeListener</summary>
		/// <param name="listener">The KeyboardHeightChangedListener to remove</param>
		public static void RemoveKeyboardHeightChangedListener(OnKeyboardHeightChangedHandler listener)
		{
			Instance.keyboard.RemoveKeyboardHeightChangedListener(listener);
		}

		/// <summary>Adds a KeyboardHeightChangedListener</summary>
		/// <param name="listener">The HardwareKeyboardChangedListener to add</param>
		public static void AddHardwareKeyboardChangedListener(OnHardwareKeyboardChangedHandler listener)
		{
			Instance.keyboard.AddHardwareKeyboardChangedListener(listener);
		}

		/// <summary>Removes a KeyboardHeightChangedListener</summary>
		/// <param name="listener">The KeyboardHeightChangedListener to remove</param>
		public static void RemoveHardwareKeyboardChangedListener(OnHardwareKeyboardChangedHandler listener)
		{
			Instance.keyboard.RemoveHardwareKeyboardChangedListener(listener);
		}
	}
}
