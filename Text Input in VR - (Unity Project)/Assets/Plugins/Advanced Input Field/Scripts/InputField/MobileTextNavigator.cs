//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin
{
	/// <summary>The TextNavigator for mobile platforms</summary>
	public class MobileTextNavigator: TextNavigator
	{
		/// <summary>The thumb size multiplier used for selection cursors size calculations</summary>
		private const float THUMB_SIZE_SCALE = 0.5f;

		/// <summary>The TouchScreenKeyboard</summary>
		private NativeKeyboard keyboard;

		/// <summary>The Transform of the selection cursors</summary>
		private Transform selectionCursorsTransform;

		/// <summary>The start cursor renderer</summary>
		private MobileCursor startCursor;

		/// <summary>The end cursor renderer</summary>
		private MobileCursor endCursor;

		/// <summary>The current cursor renderer</summary>
		private MobileCursor currentCursor;

		/// <summary>The size of the cursors</summary>
		private float cursorSize;

		/// <summary>Indicates whether at least one character has been inserted (or deleted) afer last click</summary>
		public bool hasInsertedCharAfterClick;

		/// <summary>Indicates whether at least one character has been inserted (or deleted) afer last click</summary>
		public bool HasInsertedCharAfterClick
		{
			get { return hasInsertedCharAfterClick; }
			set
			{
				hasInsertedCharAfterClick = value;
				currentCursor.enabled = (!hasInsertedCharAfterClick && !HasSelection);
			}
		}

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

		public override int ProcessedCaretPosition
		{
			get { return base.ProcessedCaretPosition; }
			set
			{
				base.ProcessedCaretPosition = value;

				if(Keyboard.State == KeyboardState.VISIBLE && !InputField.ReadOnly && !BlockNativeSelectionChange)
				{
					UpdateNativeSelection();
				}
			}
		}

		public override int SelectionStartPosition
		{
			get { return base.SelectionStartPosition; }
			protected set
			{
				int originalValue = base.SelectionStartPosition;
				base.SelectionStartPosition = value;

				if(InputField.SelectionCursorsEnabled)
				{
					if(Canvas != null)
					{
						UpdateSelectionCursors();
					}
				}

				if(InputField.ActionBarEnabled && base.SelectionStartPosition != originalValue)
				{
					UpdateSelectionCursorsActionBar();
				}

				if(Keyboard.State == KeyboardState.VISIBLE && !InputField.ReadOnly && !BlockNativeSelectionChange)
				{
					UpdateNativeSelection();
				}
			}
		}

		public override int ProcessedSelectionStartPosition
		{
			get { return base.ProcessedSelectionStartPosition; }
			protected set
			{
				int originalValue = base.ProcessedSelectionStartPosition;
				base.ProcessedSelectionStartPosition = value;

				if(InputField.SelectionCursorsEnabled)
				{
					if(Canvas != null)
					{
						UpdateSelectionCursors();
					}
				}

				if(InputField.ActionBarEnabled && base.ProcessedSelectionStartPosition != originalValue)
				{
					UpdateSelectionCursorsActionBar();
				}

				if(Keyboard.State == KeyboardState.VISIBLE && !InputField.ReadOnly && !BlockNativeSelectionChange)
				{
					UpdateNativeSelection();
				}
			}
		}

		public override int SelectionEndPosition
		{
			get { return base.SelectionEndPosition; }
			protected set
			{
				int originalValue = base.selectionEndPosition;
				base.SelectionEndPosition = value;

				if(InputField.SelectionCursorsEnabled)
				{
					if(Canvas != null)
					{
						UpdateSelectionCursors();
					}
				}

				if(InputField.ActionBarEnabled && base.SelectionEndPosition != originalValue)
				{
					UpdateSelectionCursorsActionBar();
				}

				if(Keyboard.State == KeyboardState.VISIBLE && !InputField.ReadOnly && !BlockNativeSelectionChange)
				{
					UpdateNativeSelection();
				}
			}
		}

		public override int ProcessedSelectionEndPosition
		{
			get { return base.ProcessedSelectionEndPosition; }
			protected set
			{
				int originalValue = base.ProcessedSelectionEndPosition;
				base.ProcessedSelectionEndPosition = value;

				if(InputField.SelectionCursorsEnabled)
				{
					if(Canvas != null)
					{
						UpdateSelectionCursors();
					}
				}

				if(InputField.ActionBarEnabled && base.ProcessedSelectionEndPosition != originalValue)
				{
					UpdateSelectionCursorsActionBar();
				}

				if(Keyboard.State == KeyboardState.VISIBLE && !InputField.ReadOnly && !BlockNativeSelectionChange)
				{
					UpdateNativeSelection();
				}
			}
		}

		public bool StartCursorVisible
		{
			get
			{
				return Util.RectTransformIntersects(InputField.TextAreaTransform, startCursor.RectTransform);
			}
		}

		public bool EndCursorVisible
		{
			get
			{
				return Util.RectTransformIntersects(InputField.TextAreaTransform, endCursor.RectTransform);
			}
		}

		/// <summary>The ActionBar</summary>
		public ActionBar ActionBar { get; set; }

		internal override void Initialize(AdvancedInputField inputField, Image caretRenderer, TextSelectionRenderer selectionRenderer)
		{
			base.Initialize(inputField, caretRenderer, selectionRenderer);

			selectionCursorsTransform = GameObject.Instantiate(Settings.MobileSelectionCursorsPrefab);
			selectionCursorsTransform.SetParent(TextContentTransform);
			selectionCursorsTransform.localScale = Vector3.one;
			selectionCursorsTransform.localPosition = Vector3.zero;
			selectionCursorsTransform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

			startCursor = selectionCursorsTransform.Find("StartCursor").GetComponent<MobileCursor>();
			endCursor = selectionCursorsTransform.Find("EndCursor").GetComponent<MobileCursor>();
			currentCursor = selectionCursorsTransform.Find("CurrentCursor").GetComponent<MobileCursor>();

			if(Canvas != null)
			{
				UpdateCursorSize(Canvas.scaleFactor);
			}

			startCursor.enabled = false;
			endCursor.enabled = false;
			currentCursor.enabled = false;

			PointerHandler startCursorHandler = startCursor.CursorRenderer.GetComponent<PointerHandler>();
			startCursorHandler.Press += OnStartCursorDown;
			startCursorHandler.Release += OnStartCursorUp;
			startCursorHandler.BeginDrag += OnStartCursorBeginDrag;
			startCursorHandler.Drag += OnStartCursorDrag;

			PointerHandler endCursorHandler = endCursor.CursorRenderer.GetComponent<PointerHandler>();
			endCursorHandler.Press += OnEndCursorDown;
			endCursorHandler.Release += OnEndCursorUp;
			endCursorHandler.BeginDrag += OnEndCursorBeginDrag;
			endCursorHandler.Drag += OnEndCursorDrag;

			PointerHandler currentCursorHandler = currentCursor.CursorRenderer.GetComponent<PointerHandler>();
			currentCursorHandler.Press += OnCurrentCursorDown;
			currentCursorHandler.Release += OnCurrentCursorUp;
		}

		internal override void SetCaretPosition(int caretPosition, bool invokeCaretPositonChangeEvent = false)
		{
			base.SetCaretPosition(caretPosition, invokeCaretPositonChangeEvent);

			if(Keyboard.State == KeyboardState.VISIBLE && !InputField.ReadOnly && !BlockNativeSelectionChange)
			{
				UpdateNativeSelection();
			}
		}

		internal override void OnCanvasScaleChanged(float canvasScaleFactor)
		{
			base.OnCanvasScaleChanged(canvasScaleFactor);

			UpdateCursorSize(canvasScaleFactor);
		}

		internal void UpdateCursorSize(float canvasScaleFactor)
		{
			int thumbSize = Util.DetermineThumbSize();
			if(thumbSize <= 0)
			{
				Debug.LogWarning("Unknown DPI");
				if(InputField.TextRenderer.ResizeTextForBestFit)
				{
					cursorSize = InputField.TextRenderer.FontSizeUsedForBestFit / canvasScaleFactor;
				}
				else
				{
					cursorSize = InputField.TextRenderer.FontSize / canvasScaleFactor;
				}
			}
			else
			{
				cursorSize = (thumbSize * THUMB_SIZE_SCALE) / canvasScaleFactor;
			}

			cursorSize *= Settings.MobileSelectionCursorsScale;

			startCursor.UpdateSize(new Vector2(cursorSize, cursorSize));
			endCursor.UpdateSize(new Vector2(cursorSize, cursorSize));
			currentCursor.UpdateSize(new Vector2(cursorSize, cursorSize));
		}

		internal override void BeginEditMode()
		{
			base.BeginEditMode();

			if(Canvas != null)
			{
				UpdateCursorSize(Canvas.scaleFactor);
			}
		}

		internal override void EndEditMode()
		{
            Debug.Log("MOBILE - END EDIT MODE");
			EditMode = false;
			caretBlinkTime = InputField.CaretBlinkRate;
			CaretRenderer.enabled = false;
			UpdateSelection(0, 0);

			startCursor.enabled = false;
			endCursor.enabled = false;
			currentCursor.enabled = false;

			if(ActionBar != null)
			{
				ActionBar.Hide();
			}

			ScrollArea scrollArea = TextAreaTransform.GetComponent<ScrollArea>();
			switch(InputField.ScrollBehaviourOnEndEdit)
			{
				case ScrollBehaviourOnEndEdit.START_OF_TEXT: scrollArea.MoveContentImmediately(Vector2.zero); break;
			}
			scrollArea.EditMode = false;
		}

		internal void HideCurrentMobileCursor()
		{
			currentCursor.enabled = false;
		}

		internal void UpdateNativeSelection()
		{
			if(HasSelection)
			{
				Keyboard.ChangeSelection(selectionStartPosition, selectionEndPosition);
			}
			else
			{
				Keyboard.ChangeSelection(caretPosition, caretPosition);
			}
		}

		/// <summary>Event callback when start cursor is pressed</summary>
		internal void OnStartCursorDown(PointerEventData eventData)
		{
			InputField.ShouldBlockDeselect = true;
			InputField.CurrentStartDragMode = StartDragMode.FROM_SELECTION_END; //We're going to move the start cursor, so start from end cursor
			InputField.DragOffset = new Vector2(startCursor.RectTransform.rect.width * 0.5f, startCursor.RectTransform.rect.height);
		}

		/// <summary>Event callback when start cursor is released</summary>
		internal void OnStartCursorUp(PointerEventData eventData)
		{
			InputField.ShouldBlockDeselect = false;
			InputField.DragOffset = Vector2.zero;
		}

		internal void OnStartCursorBeginDrag(PointerEventData eventData)
		{
			InputField.OnBeginDrag(eventData);
		}

		internal void OnStartCursorDrag(PointerEventData eventData)
		{
			InputField.OnDrag(eventData);
		}

		/// <summary>Event callback when end cursor is pressed</summary>
		internal void OnEndCursorDown(PointerEventData eventData)
		{
			InputField.ShouldBlockDeselect = true;
			InputField.CurrentStartDragMode = StartDragMode.FROM_SELECTION_START; //We're going to move the end cursor, so start from start cursor
			InputField.DragOffset = new Vector2(-endCursor.RectTransform.rect.width * 0.5f, endCursor.RectTransform.rect.height);
		}

		/// <summary>Event callback when end cursor is released</summary>
		internal void OnEndCursorUp(PointerEventData eventData)
		{
			InputField.ShouldBlockDeselect = false;
			InputField.DragOffset = Vector2.zero;
		}

		internal void OnEndCursorBeginDrag(PointerEventData eventData)
		{
			InputField.OnBeginDrag(eventData);
		}

		internal void OnEndCursorDrag(PointerEventData eventData)
		{
			InputField.OnDrag(eventData);
		}

		/// <summary>Event callback when current cursor is pressed</summary>
		internal void OnCurrentCursorDown(PointerEventData eventData)
		{
			InputField.ShouldBlockDeselect = true;
			if(!InputField.ActionBarEnabled)
			{
				return;
			}

			if(ActionBar.Visible)
			{
				ActionBar.Hide();
			}
			else
			{
				bool paste = !InputField.ReadOnly && InputField.ActionBarPaste;
				bool selectAll = InputField.ActionBarSelectAll;
				ActionBar.Show(false, false, paste, selectAll);
			}
		}

		/// <summary>Event callback when current cursor is released</summary>
		internal void OnCurrentCursorUp(PointerEventData eventData)
		{
			InputField.ShouldBlockDeselect = false;
		}

		internal void ShowActionBar()
		{
			bool paste = !InputField.ReadOnly && InputField.ActionBarPaste;
			bool selectAll = InputField.ActionBarSelectAll && InputField.Text.Length > 0;
			ActionBar.Show(false, false, paste, selectAll);
		}

		/// <summary>Updates the selection cursors</summary>
		internal void UpdateSelectionCursors()
		{
			if(SelectionEndPosition > SelectionStartPosition)
			{
				float lineHeight = TextRenderer.GetLineInfo(0).height;

				if(SelectionStartPosition >= 0)
				{
					int charIndex = Mathf.Clamp(SelectionStartPosition, 0, TextRenderer.CharacterCount - 1);
					CharacterInfo charInfo = TextRenderer.GetCharacterInfo(charIndex);
					int lineIndex = DetermineCharacterLine(TextRenderer, charIndex);
					LineInfo lineInfo = TextRenderer.GetLineInfo(lineIndex);

					Vector2 startCursorPosition = new Vector2(charInfo.position.x, lineInfo.topY - lineInfo.height);
					startCursor.UpdatePosition(startCursorPosition);
					startCursor.enabled = StartCursorVisible;
				}
				else
				{
					startCursor.enabled = false;
				}

				if(SelectionEndPosition >= 0)
				{
					int charIndex = Mathf.Clamp(SelectionEndPosition, 0, TextRenderer.CharacterCount - 1);
					CharacterInfo charInfo = TextRenderer.GetCharacterInfo(charIndex);
					int lineIndex = DetermineCharacterLine(TextRenderer, charIndex);
					LineInfo lineInfo = TextRenderer.GetLineInfo(lineIndex);

					Vector2 endCursorPosition = new Vector2(charInfo.position.x, lineInfo.topY - lineInfo.height);
					endCursor.UpdatePosition(endCursorPosition);
					endCursor.enabled = EndCursorVisible;
				}
				else
				{
					endCursor.enabled = false;
				}
			}
			else
			{
				startCursor.enabled = false;
				endCursor.enabled = false;
			}
		}

		internal void UpdateSelectionCursorsActionBar()
		{
			if(SelectionEndPosition > SelectionStartPosition)
			{
				int startIndex = 0;

				if(SelectionStartPosition >= 0)
				{
					startIndex = Mathf.Clamp(SelectionStartPosition, 0, TextRenderer.CharacterCount - 1);
				}

				ActionBar.transform.SetParent(InputField.transform);
				ActionBar.transform.localScale = Vector3.one;
				bool cut = !InputField.Secure && !InputField.ReadOnly && InputField.ActionBarCut;
				bool copy = !InputField.Secure && InputField.ActionBarCopy;
				bool paste = !InputField.ReadOnly && InputField.ActionBarPaste;
				bool selectAll = InputField.ActionBarSelectAll;
				ActionBar.Show(cut, copy, paste, selectAll);
				Vector2 actionBarPosition = TextRenderer.GetCharacterInfo(startIndex).position;
				actionBarPosition += TextContentTransform.anchoredPosition;
				actionBarPosition.y = Mathf.Min(actionBarPosition.y, 0);
				ActionBar.UpdatePosition(actionBarPosition);

				float topY = GetAbsoluteTopY(ActionBar.RectTransform);
				if(topY > Canvas.pixelRect.height) //Out of bounds, move to bottom of InputField
				{
					actionBarPosition.y -= (InputField.Size.y + ActionBar.RectTransform.rect.height);
					ActionBar.UpdatePosition(actionBarPosition);
				}
			}
			else
			{
				ActionBar.Hide();
			}
		}

		public float GetAbsoluteTopY(RectTransform rectTransform)
		{
			Vector3[] corners = new Vector3[4];
			rectTransform.GetWorldCorners(corners);

			float topY = corners[1].y;
			float normalizedTopY = 0;
			if(Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				normalizedTopY = topY / Screen.height;
			}
			else
			{
				Camera camera = Canvas.worldCamera;
				normalizedTopY = (topY + camera.orthographicSize) / (camera.orthographicSize * 2);
			}

			return normalizedTopY * Canvas.pixelRect.height;
		}

		/// <summary>Updates the current cursor</summary>
		internal void UpdateCurrentCursor(TextRenderer textRenderer)
		{
			bool fixEmptyCaret = false;
			if(textRenderer.CharacterCount == 0 || Text.Length == 0) //Workaround to make sure the text generator will give a correct position for the first character
			{
				fixEmptyCaret = true;
				textRenderer.UpdateImmediately(" ");
			}

			int charIndex = Mathf.Clamp(CaretPosition, 0, textRenderer.CharacterCount - 1);
			CharacterInfo charInfo = textRenderer.GetCharacterInfo(charIndex);
			int lineIndex = DetermineCharacterLine(textRenderer, charIndex);
			LineInfo lineInfo = textRenderer.GetLineInfo(lineIndex);

			Vector2 currentCursorPosition = new Vector2(charInfo.position.x, lineInfo.topY - lineInfo.height);
			if(CaretPosition >= textRenderer.CharacterCountVisible)
			{
				currentCursorPosition.x += charInfo.width;
			}

			currentCursor.UpdatePosition(currentCursorPosition);

			if(HasSelection)
			{
				currentCursor.enabled = false;
			}
			else if(!HasInsertedCharAfterClick)
			{
				currentCursor.enabled = true;
			}

			if(InputField.ActionBarEnabled && !HasSelection)
			{
				ActionBar.transform.SetParent(InputField.transform);
				ActionBar.transform.localScale = Vector3.one;
				Vector2 actionBarPosition = textRenderer.GetCharacterInfo(charIndex).position;
				actionBarPosition += TextContentTransform.anchoredPosition;
				actionBarPosition.y = Mathf.Min(actionBarPosition.y, 0);
				ActionBar.UpdatePosition(actionBarPosition);

				float topY = GetAbsoluteTopY(ActionBar.RectTransform);
				if(topY > Canvas.pixelRect.height) //Out of bounds, move to bottom of InputField
				{
					actionBarPosition.y -= (InputField.Size.y + ActionBar.RectTransform.rect.height);
					ActionBar.UpdatePosition(actionBarPosition);
				}
			}

			if(fixEmptyCaret)
			{
				textRenderer.UpdateImmediately(string.Empty);
			}
		}

		internal override void UpdateCaret()
		{
			base.UpdateCaret();

			if(EditMode && (InputField.ActionBarEnabled || InputField.SelectionCursorsEnabled))
			{
				if(Canvas != null)
				{
					TextRenderer activeTextRenderer = InputField.GetActiveTextRenderer();
					UpdateCurrentCursor(activeTextRenderer);
				}
			}
		}

		internal override void UpdateSelected()
		{
			base.UpdateSelected();

			if(EditMode && (InputField.ActionBarEnabled || InputField.SelectionCursorsEnabled))
			{
				currentCursor.enabled = !HasSelection && !HasInsertedCharAfterClick;
			}
		}

		internal override void OnDestroy()
		{
			base.OnDestroy();

			if(selectionCursorsTransform != null)
			{
				GameObject.Destroy(selectionCursorsTransform.gameObject);
				selectionCursorsTransform = null;
			}
		}

		/// <summary>Updates the selection without updating selection in native text editor</summary>
		/// <param name="position">The new caret position</param>
		internal void UpdateSelection(int startPosition, int endPosition)
		{
			if(startPosition + 1 <= selectionStartPosition)
			{
				base.SelectionStartPosition = startPosition;
				base.SelectionEndPosition = endPosition;
				base.CaretPosition = startPosition;
			}
			else
			{
				base.SelectionStartPosition = startPosition;
				base.SelectionEndPosition = endPosition;
				base.CaretPosition = endPosition;
			}

			if(InputField.SelectionCursorsEnabled)
			{
				if(Canvas != null)
				{
					UpdateSelectionCursors();
				}
			}

			if(InputField.ActionBarEnabled)
			{
				UpdateSelectionCursorsActionBar();
			}
		}
	}
}
#endif