//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace AdvancedInputFieldPlugin
{
	/// <summary>The delegate for Keyboard Height Changed event</summary>
	public delegate void OnKeyboardHeightChangedHandler(int keyboardHeight);

	/// <summary>The delegate for Hardware Keyboard Changed event</summary>
	public delegate void OnHardwareKeyboardChangedHandler(bool connected);

	public enum KeyboardState
	{
		HIDDEN, PENDING_SHOW, VISIBLE, PENDING_HIDE
	}

	/// <summary>Base class that acts as a bridge for the Native Keyboard for a specific platform</summary>
	public abstract class NativeKeyboard: MonoBehaviour
	{
		/// <summary>Event type of the keyboard callbacks</summary>
		public enum EventType
		{
			SHOW,
			HIDE,
			TEXT_CHANGE,
			SELECTION_CHANGE,
			DONE,
			NEXT,
			CANCEL,
			DELETE
		}

		/// <summary>Event for keyboard callbacks</summary>
		public class Event
		{
			/// <summary>Event type</summary>
			public EventType Type { get; private set; }

			/// <summary>The value</summary>
			public string Value { get; private set; }

			public Event(EventType type, string value = null)
			{
				Type = type;
				Value = value;
			}
		}

		/// <summary>Queue with Keyboard events</summary>
		protected ThreadsafeQueue<Event> nativeEventQueue;

		/// <summary>The name of the GameObject used for callbacks</summary>
		protected string gameObjectName;

		/// <summary>The event for Keyboard Height Changed</summary>
		protected event OnKeyboardHeightChangedHandler onKeyboardHeightChanged;

		/// <summary>The event for Hardware Keyboard Changed</summary>
		protected event OnHardwareKeyboardChangedHandler onHardwareKeyboardChanged;

		/// <summary>Indicates whether the state of the keyboard</summary>
		public KeyboardState State { get; set; }

		/// <summary>Indicates whether the native binding is active or not</summary>
		public bool Active { get; private set; }

		/// <summary>Indicates whether a hardware keyboard is connected</summary>
		public bool HardwareKeyboardConnected { get; protected set; }

		/// <summary>Initializes this class</summary>
		/// <param name="gameObjectName">The name of the GameObject to use for callbacks</param>
		internal void Init(string gameObjectName)
		{
			this.gameObjectName = gameObjectName;
			nativeEventQueue = new ThreadsafeQueue<Event>(30);
			Setup();
		}

		/// <summary>Gets and removes next keyboard event</summary>
		/// <param name="keyboardEvent">The output keyboard event</param>
		internal bool PopEvent(out Event keyboardEvent)
		{
			if(nativeEventQueue.Count == 0)
			{
				keyboardEvent = null;
				return false;
			}

			keyboardEvent = nativeEventQueue.Dequeue();
			return true;
		}

		/// <summary>Clears the keyboard event queue</summary>
		internal void ClearEventQueue()
		{
			nativeEventQueue.Clear();
		}

		/// <summary>Adds a KeyboardHeightChangedListener</summary>
		/// <param name="listener">The KeyboardHeightChangedListener to add</param>
		public void AddKeyboardHeightChangedListener(OnKeyboardHeightChangedHandler listener)
		{
			onKeyboardHeightChanged += listener;
		}

		/// <summary>Removes a KeyboardHeightChangedListener</summary>
		/// <param name="listener">The KeyboardHeightChangedListener to remove</param>
		public void RemoveKeyboardHeightChangedListener(OnKeyboardHeightChangedHandler listener)
		{
			onKeyboardHeightChanged -= listener;
		}

		/// <summary>Adds a KeyboardHeightChangedListener</summary>
		/// <param name="listener">The HardwareKeyboardChangedListener to add</param>
		public void AddHardwareKeyboardChangedListener(OnHardwareKeyboardChangedHandler listener)
		{
			onHardwareKeyboardChanged += listener;
		}

		/// <summary>Removes a KeyboardHeightChangedListener</summary>
		/// <param name="listener">The KeyboardHeightChangedListener to remove</param>
		public void RemoveHardwareKeyboardChangedListener(OnHardwareKeyboardChangedHandler listener)
		{
			onHardwareKeyboardChanged -= listener;
		}

		internal void InvokeKeyboardHeightChanged(int height)
		{
			if(onKeyboardHeightChanged != null)
			{
				onKeyboardHeightChanged.Invoke(height);
			}
		}

		internal void InvokeHardwareKeyboardChanged(bool connected)
		{
			if(onHardwareKeyboardChanged != null)
			{
				onHardwareKeyboardChanged.Invoke(connected);
			}
		}

		/// <summary>Setups the bridge to the Native Keyboard</summary>
		internal virtual void Setup() { }

		/// <summary>Checks whether the native binding should be active or not</summary>
		internal void UpdateActiveState()
		{
			bool inputFieldSelected = false;
			GameObject currentSelection = EventSystem.current.currentSelectedGameObject;
			if(currentSelection != null)
			{
				inputFieldSelected = currentSelection.GetComponentInParent<AdvancedInputField>() || currentSelection.GetComponentInParent<CanvasFrontRenderer>();
			}

			if(Active != inputFieldSelected)
			{
				Active = inputFieldSelected;
				if(Active)
				{
					if(!HardwareKeyboardConnected)
					{
						EnableUpdates();
					}

					if(Settings.MobileKeyboardBehaviour == MobileKeyboardBehaviour.USE_HARDWARE_KEYBOARD_WHEN_AVAILABLE)
					{
						EnableHardwareKeyboardUpdates();
					}
				}
				else
				{
					if(!HardwareKeyboardConnected)
					{
						DisableUpdates();
					}

					DisableHardwareKeyboardUpdates();
				}
			}
		}

		/// <summary>Enables updates in the native binding</summary>
		public virtual void EnableUpdates() { }

		/// <summary>Disables updates in the native binding</summary>
		public virtual void DisableUpdates() { }

		/// <summary>Enables hardware keyboard updates in the native binding</summary>
		public virtual void EnableHardwareKeyboardUpdates() { }

		/// <summary>Disables hardware keyboard updates in the native binding</summary>
		public virtual void DisableHardwareKeyboardUpdates() { }

		/// <summary>Shows the TouchScreenKeyboard for current platform</summary>
		/// <param name="text">The current text of the InputField</param>
		/// <param param name="keyboardType">The keyboard type to use</param>
		/// <param param name="characterValidation">The character validation to use</param>
		/// <param name="lineType">The lineType to use</param>
		/// <param name="autocorrection">Indicates whether autocorrection is enabled</param>
		/// <param name="characterLimit">The character limit for the text</param>
		/// <param name="secure">Indicates whether input should be secure</param>
		public virtual void ShowKeyboard(string text, KeyboardType keyboardType, CharacterValidation characterValidation, LineType lineType, AutocapitalizationType autocapitalizationType, bool autocorrection, bool secure, bool emojisAllowed, int characterLimit, string characterValidatorJSON) { }

		/// <summary>Hides the TouchScreenKeyboard for current platform</summary>
		public virtual void HideKeyboard() { }

		/// <summary>Changes the text natively used by the keyboard</summary>
		public virtual void ChangeText(string text) { }

		/// <summary>Sets the selection for the native keyboard</summary>
		/// <param name="selectionStartPosition">The start position of the selection</param>
		/// <param name="selectionEndPosition">The end position of the selection</param>
		public virtual void ChangeSelection(int selectionStartPosition, int selectionEndPosition) { }

		/// <summary>Event callback when the keyboard gets shown</summary>
		public void OnKeyboardShow()
		{
			nativeEventQueue.Enqueue(new Event(EventType.SHOW, null));
			State = KeyboardState.VISIBLE;
		}

		/// <summary>Event callback when the keyboard gets hidden</summary>
		public void OnKeyboardHide()
		{
			nativeEventQueue.Enqueue(new Event(EventType.HIDE, null));
			State = KeyboardState.HIDDEN;
		}

		/// <summary>Event callback when the text used by the native keyboard got changed</summary>
		/// <param name="text">The changed text</param>
		public void OnTextChanged(string text)
		{
			nativeEventQueue.Enqueue(new Event(EventType.TEXT_CHANGE, text));
		}

		/// <summary>Event callback when the selection got changed natively</summary>
		public void OnSelectionChanged(string valueString)
		{
			nativeEventQueue.Enqueue(new Event(EventType.SELECTION_CHANGE, valueString));
		}

		/// <summary>Event callback when the keyboard height changes</summary>
		/// <param name="keyboardHeightString">The string value for keyboard height</param>
		public void OnKeyboardHeightChanged(string keyboardHeightString)
		{
			int keyboardHeight = 0;
			if(int.TryParse(keyboardHeightString, out keyboardHeight))
			{
				InvokeKeyboardHeightChanged(keyboardHeight);

				if(keyboardHeight == 0) //Safety check if something external caused the keyboard to hide
				{
					State = KeyboardState.HIDDEN;
				}
			}
		}

		/// <summary>Event callback when the hardware keyboard changes</summary>
		/// <param name="connectedString">The string value for hardware keyboard connected</param>
		public void OnHardwareKeyboardChanged(string connectedString)
		{
			bool connected = false;
			if(bool.TryParse(connectedString, out connected))
			{
				HardwareKeyboardConnected = connected;
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
			}
		}

		/// <summary>Event callback when the cancel key of the keyboard (back key on Android) gets pressed</summary>
		public void OnKeyboardCancel()
		{
			nativeEventQueue.Enqueue(new Event(EventType.CANCEL));
		}

		/// <summary>Event callback when the delete/backspace key of the keyboard gets pressed</summary>
		public void OnKeyboardDelete()
		{
			nativeEventQueue.Enqueue(new Event(EventType.DELETE));
		}

		/// <summary>Event callback when the done key of the keyboard gets pressed</summary>
		public void OnKeyboardDone()
		{
			nativeEventQueue.Enqueue(new Event(EventType.DONE));
		}

		/// <summary>Event callback when the next key of the keyboard gets pressed</summary>
		public void OnKeyboardNext()
		{
			nativeEventQueue.Enqueue(new Event(EventType.NEXT));
		}
	}
}
