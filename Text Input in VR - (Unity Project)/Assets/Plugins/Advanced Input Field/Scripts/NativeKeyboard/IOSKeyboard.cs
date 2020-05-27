//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Class that acts as a bridge for the Native iOS Keyboard</summary>
	public class IOSKeyboard: NativeKeyboard
	{
		[DllImport("__Internal")]
        private static extern void _nativeKeyboard_initialize(string gameObjectName);
		[DllImport("__Internal")]
        private static extern void _nativeKeyboard_changeText(string text);
		[DllImport("__Internal")]
        private static extern void _nativeKeyboard_changeSelection(int start, int end);
		[DllImport("__Internal")]
        private static extern void _nativeKeyboard_showKeyboard(string text, int keyboardType, int characterValidation, int lineType, int autocapitalizationType, bool autocorrection, bool secure, bool emojisAllowed, int characterLimit, string characterValidatorJSON);
		[DllImport("__Internal")]
        private static extern void _nativeKeyboard_hideKeyboard();
		[DllImport("__Internal")]
        private static extern void _nativeKeyboard_enableUpdates();
		[DllImport("__Internal")]
        private static extern void _nativeKeyboard_disableUpdates();
		[DllImport("__Internal")]
        private static extern void _nativeKeyboard_enableHardwareKeyboardUpdates();
		[DllImport("__Internal")]
		private static extern void _nativeKeyboard_disableHardwareKeyboardUpdates();

		internal override void Setup()
		{
			_nativeKeyboard_initialize(gameObjectName);
		}

		public override void EnableUpdates()
		{	
			_nativeKeyboard_enableUpdates();
		}

		public override void DisableUpdates()
		{
			_nativeKeyboard_disableUpdates();
		}
	
		public override void EnableHardwareKeyboardUpdates()
		{
			_nativeKeyboard_enableHardwareKeyboardUpdates();
		}

		public override void DisableHardwareKeyboardUpdates()
		{
            _nativeKeyboard_disableHardwareKeyboardUpdates();
		}

		public override void ShowKeyboard(string text, KeyboardType keyboardType, CharacterValidation characterValidation, LineType lineType, AutocapitalizationType autocapitalizationType, bool autocorrection, bool secure, bool emojisAllowed, int characterLimit, string characterValidatorJSON)
		{
            _nativeKeyboard_showKeyboard(text, (int)keyboardType, (int)characterValidation, (int)lineType, (int)autocapitalizationType, autocorrection, secure, emojisAllowed, characterLimit, characterValidatorJSON);
		}

		public override void HideKeyboard()
		{
			_nativeKeyboard_hideKeyboard();
		}

		public override void ChangeText(string text)
		{
			_nativeKeyboard_changeText(text);
		}

		public override void ChangeSelection(int selectionStartPosition, int selectionEndPosition)
		{
			_nativeKeyboard_changeSelection(selectionStartPosition, selectionEndPosition);
		}
	}
}
#endif