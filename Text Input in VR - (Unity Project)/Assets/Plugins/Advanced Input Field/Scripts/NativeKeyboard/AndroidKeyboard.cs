
#if !UNITY_EDITOR && UNITY_ANDROID
using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Class that acts as a bridge for the Native Android Keyboard</summary>
	public class AndroidKeyboard: NativeKeyboard
	{
		/// <summary>The main Android class</summary>
		private AndroidJavaClass mainClass;

		internal override void Setup()
		{
			mainClass = new AndroidJavaClass("com.jeroenvanpienbroek.nativekeyboard.NativeKeyboard");
			mainClass.CallStatic("initialize", gameObjectName);
		}

		public override void EnableUpdates()
		{
			mainClass.CallStatic("enableUpdates");
		}

		public override void DisableUpdates()
		{
			mainClass.CallStatic("disableUpdates");
		}

		public override void EnableHardwareKeyboardUpdates()
		{
			mainClass.CallStatic("enableHardwareKeyboardUpdates");
		}

		public override void DisableHardwareKeyboardUpdates()
		{
			mainClass.CallStatic("disableHardwareKeyboardUpdates");
		}

		public override void ShowKeyboard(string text, KeyboardType keyboardType, CharacterValidation characterValidation, LineType lineType, AutocapitalizationType autocapitalizationType, bool autocorrection, bool secure, bool emojisAllowed, int characterLimit, string characterValidatorJSON)
		{
			mainClass.CallStatic("showKeyboard", text, (int)keyboardType, (int)characterValidation, (int)lineType, (int)autocapitalizationType, autocorrection, secure, emojisAllowed, characterLimit, characterValidatorJSON);
		}

		public override void HideKeyboard()
		{
			mainClass.CallStatic("hideKeyboard");
		}

		public override void ChangeText(string text)
		{
			mainClass.CallStatic("changeText", text);
		}

		public override void ChangeSelection(int selectionStartPosition, int selectionEndPosition)
		{
			mainClass.CallStatic("changeSelection", selectionStartPosition, selectionEndPosition);
		}
	}
}
#endif