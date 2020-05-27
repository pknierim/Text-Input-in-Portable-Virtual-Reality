//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Base class for processing input events</summary>
	public abstract class TextInputHandler
	{
		/// <summary>Threshold between 2 taps to count as Double Tap</summary>
		private const float DOUBLE_TAP_THRESHOLD = 0.5f;

		/// <summary>The time input needs to be pressed and be kept at same position to count as Hold</summary>
		private const float HOLD_THRESHOLD = 1f;

		/// <summary>The InputField</summary>
		public AdvancedInputField InputField { get; private set; }

		/// <summary>The Canvas</summary>
		public Canvas Canvas { get { return InputField.Canvas; } }

		/// <summary>The TextNavigator</summary>
		public abstract TextNavigator TextNavigator { get; protected set; }

		/// <summary>The TextManipulator</summary>
		public virtual TextManipulator TextManipulator { get; protected set; }

		/// <summary>The character position when press started</summary>
		private int pressCharPosition;

		/// <summary>The time of last tap</summary>
		private float lastTapTime;

		/// <summary>The time input is currently holding</summary>
		private float holdTime;

		/// <summary>Indicates whether input is currently holding</summary>
		private bool holding;

		/// <summary>Character position of last event</summary>
		private Vector2 lastPosition;

		public TextInputHandler()
		{
		}

		/// <summary>Initializes this class</summary>
		internal virtual void Initialize(AdvancedInputField inputField, TextNavigator textNavigator, TextManipulator textManipulator)
		{
			InputField = inputField;
			TextNavigator = textNavigator;
			TextManipulator = textManipulator;
		}

		/// <summary>Method called when InputField gets destroyed</summary>
		internal virtual void OnDestroy()
		{
		}

		internal virtual void OnCanvasScaleChanged(float canvasScaleFactor)
		{
		}

		/// <summary>Processes input events</summary>
		internal virtual void Process()
		{
			if(holding && holdTime < HOLD_THRESHOLD)
			{
				holdTime += Time.deltaTime;
				if(holdTime >= HOLD_THRESHOLD)
				{
					OnHold(lastPosition);
				}
			}
		}

		internal virtual void BeginEditMode()
		{

		}

		/// <summary>Event callback when selection started</summary>
		internal virtual void OnSelect()
		{
		}

		/// <summary>Event callback for input press</summary>
		/// <param name="position">The position of the event</param>
		internal virtual void OnPress(Vector2 position)
		{
			if(InputField.LiveProcessing)
			{
				pressCharPosition = TextNavigator.GetCharacterIndexFromPosition(InputField.ProcessedTextRenderer, position);
			}
			else
			{
				pressCharPosition = TextNavigator.GetCharacterIndexFromPosition(InputField.TextRenderer, position);
			}
			holding = true;
			holdTime = 0;
			lastPosition = position;
		}

		/// <summary>Event callback for input drag</summary>
		/// <param name="position">The position of the event</param>
		internal virtual void OnDrag(Vector2 position)
		{
			int holdCharPosition = 0;
			if(InputField.LiveProcessing)
			{
				holdCharPosition = TextNavigator.GetCharacterIndexFromPosition(InputField.ProcessedTextRenderer, position);
			}
			else
			{
				holdCharPosition = TextNavigator.GetCharacterIndexFromPosition(InputField.TextRenderer, position);
			}

			if(holdCharPosition != pressCharPosition)
			{
				holding = false;
			}

			lastPosition = position;
		}

		/// <summary>Event callback for input release</summary>
		/// <param name="position">The position of the event</param>
		internal virtual void OnRelease(Vector2 position)
		{
			int releaseCharPosition = 0;
			if(InputField.LiveProcessing)
			{
				releaseCharPosition = TextNavigator.GetCharacterIndexFromPosition(InputField.ProcessedTextRenderer, position);
			}
			else
			{
				releaseCharPosition = TextNavigator.GetCharacterIndexFromPosition(InputField.TextRenderer, position);
			}

			if(pressCharPosition == releaseCharPosition)
			{
				if(holdTime < HOLD_THRESHOLD)
				{
					OnTap(position);
				}
			}
			else
			{
				lastTapTime = 0;
			}

			holding = false;
			lastPosition = position;
		}

		/// <summary>Event callback for input tap</summary>
		/// <param name="position">The position of th event</param>
		internal virtual void OnTap(Vector2 position)
		{
			if(lastTapTime > 0 && Time.realtimeSinceStartup - lastTapTime <= DOUBLE_TAP_THRESHOLD)
			{
				OnDoubleTap(position);
			}
			else
			{
				OnSingleTap(position);
			}

			lastTapTime = Time.realtimeSinceStartup;
		}

		/// <summary>Event callback for single tap</summary>
		/// <param name="position">The position of th event</param>
		internal virtual void OnSingleTap(Vector2 position)
		{
            Debug.Log("TextInputHandler - ON SINGLE TAP");
			TextNavigator.ResetCaret(position);
		}

		/// <summary>Event callback for double tap</summary>
		/// <param name="position">The position of th event</param>
		internal virtual void OnDoubleTap(Vector2 position)
		{
			TextNavigator.SelectCurrentWord();
		}

		/// <summary>Event callback for input hold</summary>
		/// <param name="position">The position of th event</param>
		internal virtual void OnHold(Vector2 position)
		{
		}

		/// <summary>Cancels the input</summary>
		internal virtual void CancelInput()
		{
		}
	}
}
