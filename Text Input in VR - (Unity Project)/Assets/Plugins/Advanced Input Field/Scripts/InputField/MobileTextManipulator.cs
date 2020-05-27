//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
namespace AdvancedInputFieldPlugin
{
	/// <summary>Subclass of TextManipulator for mobile platforms</summary>
	public class MobileTextManipulator: TextManipulator
	{
		/// <summary>The TouchScreenKeyboard</summary>
		private NativeKeyboard keyboard;

		/// <summary>The MobileTextNavigator</summary>
		private MobileTextNavigator mobileTextNavigator;

		/// <summary>The TouchScreenKeyboard</summary>
		private NativeKeyboard Keyboard
		{
			get
			{
				if(keyboard == null)
				{
					keyboard = NativeKeyboardManager.Keyboard;
				}

				return keyboard;
			}
		}

		public override TextNavigator TextNavigator
		{
			get { return mobileTextNavigator; }
			protected set { mobileTextNavigator = (MobileTextNavigator)value; }
		}

		internal void UpdateNativeText(string text)
		{
			if(!InputField.ReadOnly && !BlockNativeTextChange)
			{
				Keyboard.ChangeText(text);
			}
		}

		internal override void Insert(string input)
		{
			base.Insert(input);

			if(!mobileTextNavigator.HasInsertedCharAfterClick)
			{
				mobileTextNavigator.HasInsertedCharAfterClick = true;

				if(InputField.ActionBarEnabled)
				{
					mobileTextNavigator.ActionBar.Hide();
				}

				if(InputField.SelectionCursorsEnabled)
				{
					mobileTextNavigator.HideCurrentMobileCursor();
				}
			}
		}

		internal override void Copy()
		{
			base.Copy();

			if(InputField.ActionBarEnabled)
			{
				mobileTextNavigator.ActionBar.Hide();
				mobileTextNavigator.CaretToSelectionEnd();
				mobileTextNavigator.CancelSelection();
			}
		}

		internal override void Paste()
		{
			base.Paste();

			if(InputField.ActionBarEnabled)
			{
				mobileTextNavigator.ActionBar.Hide();
			}
		}

		internal override void Cut()
		{
			base.Cut();

			if(InputField.ActionBarEnabled)
			{
				mobileTextNavigator.ActionBar.Hide();
			}
		}
	}
}
#endif