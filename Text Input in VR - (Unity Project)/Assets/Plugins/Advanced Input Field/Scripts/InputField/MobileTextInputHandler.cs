//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
using UnityEngine;
using UnityEngine.EventSystems;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Subclass of TextInputHandler for mobile platforms</summary>
	public class MobileTextInputHandler: TextInputHandler
	{
		/// <summary>The TouchScreenKeyboard</summary>
		private NativeKeyboard keyboard;

		/// <summary>The MobileTextNavigator</summary>
		private MobileTextNavigator mobileTextNavigator;

		/// <summary>The MobileTextManipulator</summary>
		private MobileTextManipulator mobileTextManipulator;

		/// <summary>The ActionBar</summary>
		protected ActionBar actionBar;

		/// <summary>The caret position when calling the keyboard</summary>
		private int startCaretPosition;

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

		public override TextManipulator TextManipulator
		{
			get { return mobileTextManipulator; }
			protected set { mobileTextManipulator = (MobileTextManipulator)value; }
		}

		public MobileTextInputHandler()
		{
		}

		internal override void Initialize(AdvancedInputField inputField, TextNavigator textNavigator, TextManipulator textManipulator)
		{
			base.Initialize(inputField, textNavigator, textManipulator);

			mobileTextNavigator.ActionBar = actionBar;
		}

		/// <summary>Initializes the ActionBar</summary>
		/// <param name="textRenderer">The text renderer to attach the ActionBar to</param>
		internal void InitActionBar(AdvancedInputField inputField, TextRenderer textRenderer)
		{
			actionBar = inputField.transform.root.GetComponentInChildren<ActionBar>(true);
			if(actionBar == null)
			{
				actionBar = GameObject.Instantiate(Settings.MobileActionBarPrefab);
			}

			if (mobileTextNavigator != null && mobileTextNavigator.ActionBar == null)
			{
				mobileTextNavigator.ActionBar = actionBar;
			}

			actionBar.transform.SetParent(textRenderer.transform);
			actionBar.transform.localScale = Vector3.one;
			actionBar.transform.localPosition = Vector3.zero;

			actionBar.Initialize(inputField, this);
		}

		internal override void OnDestroy()
		{
			base.OnDestroy();

			if(actionBar != null)
			{
				GameObject.Destroy(actionBar.gameObject);
				actionBar = null;
			}
		}

		internal override void OnCanvasScaleChanged(float canvasScaleFactor)
		{
			base.OnCanvasScaleChanged(canvasScaleFactor);

			if(actionBar != null)
			{
				actionBar.UpdateSize(canvasScaleFactor);
				actionBar.UpdateButtons();
			}
		}

		internal override void Process()
		{
			base.Process();

			if(InputField.ReadOnly)
			{
				return;
			}

			if(Keyboard.State == KeyboardState.HIDDEN)
			{
				startCaretPosition = TextNavigator.CaretPosition; //Workaround for setting correct caret position when starting to show keyboard

				string text = InputField.Text;
				KeyboardType mobileKeyboardType = InputField.KeyboardType;
				CharacterValidation characterValidation = InputField.CharacterValidation;
				LineType lineType = InputField.LineType;
				string characterValidatorJSON = null;
				if(characterValidation == CharacterValidation.Custom && InputField.CharacterValidator != null)
				{
					characterValidatorJSON = JsonUtility.ToJson(InputField.CharacterValidator);
				}
				AutocapitalizationType autocapitalizationType = InputField.AutocapitalizationType;
				bool autocorrection = InputField.AutoCorrection;
				bool secure = InputField.Secure;
				bool emojisAllowed = InputField.EmojisAllowed;
				int characterLimit = InputField.CharacterLimit;

				Keyboard.State = KeyboardState.PENDING_SHOW;
				NativeKeyboardManager.ShowKeyboard(text, mobileKeyboardType, characterValidation, lineType, autocapitalizationType, autocorrection, secure, emojisAllowed, characterLimit, characterValidatorJSON);
			}

			NativeKeyboard.Event keyboardEvent = null;
			while(Keyboard.PopEvent(out keyboardEvent))
			{
				switch(keyboardEvent.Type)
				{
					case NativeKeyboard.EventType.SHOW: ProcessShow(keyboardEvent); break;
					case NativeKeyboard.EventType.HIDE: ProcessHide(keyboardEvent); break;
					case NativeKeyboard.EventType.TEXT_CHANGE: ProcessTextChange(keyboardEvent); break;
					case NativeKeyboard.EventType.SELECTION_CHANGE: ProcessSelectionChange(keyboardEvent); break;
					case NativeKeyboard.EventType.DONE: ProcessDone(keyboardEvent); break;
					case NativeKeyboard.EventType.NEXT: ProcessDone(keyboardEvent); break;
					case NativeKeyboard.EventType.CANCEL: ProcessCancel(keyboardEvent); break;
				}
			}
		}

		internal override void BeginEditMode()
		{
			base.BeginEditMode();

			Keyboard.State = KeyboardState.HIDDEN; //Flag keyboard as inactive, so this inputfield will load it's settings

			if(actionBar != null)
			{
				if(Canvas != null)
				{
					actionBar.UpdateSize(Canvas.scaleFactor);
					actionBar.UpdateButtons();
				}
			}
		}

		/// <summary>Processes keyboard show event</summary>
		internal void ProcessShow(NativeKeyboard.Event keyboardEvent)
		{
			TextNavigator.CaretPosition = TextNavigator.CaretPosition; //Workaround for setting correct caret position when starting to show keyboard
		}

		/// <summary>Processes keyboard hide event</summary>
		internal void ProcessHide(NativeKeyboard.Event keyboardEvent)
		{
		}

		/// <summary>Processes keyboard text change event</summary>
		internal void ProcessTextChange(NativeKeyboard.Event keyboardEvent)
		{
			int textLengthBefore = InputField.Text.Length;

			TextManipulator.BlockNativeTextChange = true;
			TextNavigator.BlockNativeSelectionChange = true;
			InputField.SetText(keyboardEvent.Value, true);
			TextManipulator.BlockNativeTextChange = false;
			TextNavigator.BlockNativeSelectionChange = false;

			int textLengthAfter = InputField.Text.Length;
			if(textLengthBefore != textLengthAfter)
			{
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
		}

		/// <summary>Processes keyboard caret position change event</summary>
		internal void ProcessSelectionChange(NativeKeyboard.Event keyboardEvent)
		{
			string[] values = keyboardEvent.Value.Split(',');
			int selectionStartPosition = int.Parse(values[0].Trim());
			int selectionEndPosition = int.Parse(values[1].Trim());

			TextManipulator.BlockNativeTextChange = true;
			TextNavigator.BlockNativeSelectionChange = true;
			mobileTextNavigator.UpdateSelection(selectionStartPosition, selectionEndPosition);
			TextManipulator.BlockNativeTextChange = false;
			TextNavigator.BlockNativeSelectionChange = false;
		}

		/// <summary>Processes keyboard done event</summary>
		internal void ProcessDone(NativeKeyboard.Event keyboardEvent)
		{
			Keyboard.ClearEventQueue(); //Should be last event to process, so clear queue

			if(InputField.NextInputField != null)
			{
				Keyboard.State = KeyboardState.HIDDEN; //Flag keyboard as inactive, so next inputfield will load it's settings
				InputField.Deselect(EndEditReason.KEYBOARD_NEXT);
				InputField.NextInputField.ManualSelect();
			}
			else
			{
				Keyboard.State = KeyboardState.PENDING_HIDE;
				Keyboard.HideKeyboard();
				InputField.Deselect(EndEditReason.KEYBOARD_DONE);
			}
		}

		/// <summary>Processes keyboard cancel event</summary>
		internal void ProcessCancel(NativeKeyboard.Event keyboardEvent)
		{
			Keyboard.ClearEventQueue(); //Should be last event to process, so clear queue

			Keyboard.State = KeyboardState.PENDING_HIDE;
			Keyboard.HideKeyboard();
			InputField.Deselect(EndEditReason.KEYBOARD_CANCEL);
		}

		internal override void OnSelect()
		{
			base.OnSelect();

			if(InputField.ReadOnly)
			{
				return;
			}

			Keyboard.ChangeSelection(TextNavigator.CaretPosition, TextNavigator.CaretPosition);
		}

		internal override void OnHold(Vector2 position)
		{
			base.OnHold(position);
			if(InputField.Text.Length > 0)
			{
				TextNavigator.SelectCurrentWord();
			}
			else
			{
				mobileTextNavigator.ShowActionBar();
			}
		}

		internal override void OnSingleTap(Vector2 position)
		{
			base.OnSingleTap(position);

			if(InputField.ActionBarEnabled)
			{
				mobileTextNavigator.HasInsertedCharAfterClick = false;
			}
		}

		/// <summary>Event callback when cut button has been clicked</summary>
		public void OnCut()
		{
			TextManipulator.Cut();
		}

		/// <summary>Event callback when copy button has been clicked</summary>
		public void OnCopy()
		{
			TextManipulator.Copy();
		}

		/// <summary>Event callback when paste button has been clicked</summary>
		public void OnPaste()
		{
			TextManipulator.Paste();
		}

		/// <summary>Event callback when select all button has been clicked</summary>
		public void OnSelectAll()
		{
			TextNavigator.SelectAll();
		}

		internal override void CancelInput()
		{
			base.CancelInput();

			if(Keyboard != null)
			{
				GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
				if(currentSelectedGameObject != null)
				{
					AdvancedInputField currentSelectedInputField = currentSelectedGameObject.GetComponentInParent<AdvancedInputField>();
					if(currentSelectedInputField != null && !currentSelectedInputField.ReadOnly)
					{
						if(currentSelectedInputField != InputField.NextInputField)
						{
							BeginEditReason beginEditReason = BeginEditReason.PROGRAMMATIC_SELECT;
							if(currentSelectedInputField.UserPressing)
							{
								beginEditReason = BeginEditReason.USER_SELECT;
							}

							currentSelectedInputField.ManualSelect(beginEditReason);
						}

						return; //Don't hide keyboard, next inputfield is selected
					}
				}

				Keyboard.State = KeyboardState.PENDING_HIDE;
				Keyboard.HideKeyboard();
				Keyboard.ClearEventQueue(); //Should be last event to process, so clear queue
			}
		}
	}
}
#endif