//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin
{
	/// <summary>The base class for text navigation using the caret and selecting text</summary>
	public class TextNavigator
	{
		/// <summary>The characters used for word separation</summary>
		protected readonly char[] WORD_SEPARATOR_CHARS = { ' ', '.', ',', '\t', '\r', '\n' };

		public RectTransform TextAreaTransform { get { return InputField.TextAreaTransform; } }

		public RectTransform TextContentTransform { get { return InputField.TextContentTransform; } }

		/// <summary>The main renderer of the text</summary>
		public TextRenderer TextRenderer { get { return InputField.TextRenderer; } }

		/// <summary>The text renderer for processed text</summary>
		public TextRenderer ProcessedTextRenderer { get { return InputField.ProcessedTextRenderer; } }

		/// <summary>The renderer of the caret</summary>
		public Image CaretRenderer { get; private set; }

		/// <summary>The renderer for text selection</summary>
		public TextSelectionRenderer SelectionRenderer { get; private set; }

		/// <summary>The InputField</summary>
		public AdvancedInputField InputField { get; private set; }

		/// <summary>The caret position</summary>
		protected int caretPosition;

		/// <summary>The caret position of the processed text when using live processing filters</summary>
		protected int processedCaretPosition;

		/// <summary>The start position of the text selection</summary>
		protected int selectionStartPosition;

		/// <summary>The start position of the processed text selection when using live processing filters</summary>
		protected int processedSelectionStartPosition;

		/// <summary>The end position of the text selection</summary>
		protected int selectionEndPosition;

		/// <summary>The end position of the processed text selection when using live processing filters</summary>
		protected int processedSelectionEndPosition;

		/// <summary>The current time for caret blink</summary>
		protected float caretBlinkTime;

		/// <summary>Indicates if edit mode is enabled</summary>
		public bool EditMode { get; protected set; }

		/// <summary>The main text string</summary>
		public string Text { get { return InputField.Text; } }

		/// <summary>The processed text string (for live processing)</summary>
		public string ProcessedText { get { return InputField.ProcessedText; } }

		/// <summary>The text currently being rendered</summary>
		public string RenderedText { get { return TextRenderer.Text; } }

		/// <summary>The processed text currently being rendered (for live processing)</summary>
		public string ProcessedRenderedText { get { return ProcessedTextRenderer.Text; } }

		/// <summary>The Canvas</summary>
		public Canvas Canvas { get { return InputField.Canvas; } }

		/// <summary>The caret position</summary>
		public virtual int CaretPosition
		{
			get { return caretPosition; }
			internal set
			{
				SetCaretPosition(value, true);
			}
		}

		/// <summary>The processed caret position (for live processing)</summary>
		public virtual int ProcessedCaretPosition
		{
			get { return processedCaretPosition; }
			set
			{
				processedCaretPosition = value;

				string text = InputField.Text;
				string processedText = InputField.ProcessedText;
				caretPosition = InputField.LiveProcessingFilter.DetermineCaret(text, processedText, processedCaretPosition);
				UpdateCaret();
			}
		}

		/// <summary>The caret position in currently rendered text</summary>
		public virtual int VisibleCaretPosition
		{
			get
			{
				if(InputField.LiveProcessing)
				{
					return ProcessedCaretPosition;
				}
				else
				{
					return CaretPosition;
				}
			}
			set
			{
				if(InputField.LiveProcessing)
				{
					ProcessedCaretPosition = value;
				}
				else
				{
					CaretPosition = value;
				}
			}
		}

		/// <summary>The start position of the text selection</summary>
		public virtual int SelectionStartPosition
		{
			get { return selectionStartPosition; }
			protected set
			{
				SetSelectionStartPosition(value, true);
			}
		}

		/// <summary>The start position of the processed text selection (for live processing)</summary>
		public virtual int ProcessedSelectionStartPosition
		{
			get { return processedSelectionStartPosition; }
			protected set
			{
				processedSelectionStartPosition = value;

				string text = InputField.Text;
				string processedText = InputField.ProcessedText;
				selectionStartPosition = InputField.LiveProcessingFilter.DetermineCaret(text, processedText, processedSelectionStartPosition);
				SelectionRenderer.UpdateSelection(VisibleSelectionStartPosition, VisibleSelectionEndPosition);
			}
		}

		/// <summary>The start position of the text selection in currently rendered text</summary>
		public virtual int VisibleSelectionStartPosition
		{
			get
			{
				if(InputField.LiveProcessing)
				{
					return ProcessedSelectionStartPosition;
				}
				else
				{
					return SelectionStartPosition;
				}
			}
			set
			{
				if(InputField.LiveProcessing)
				{
					ProcessedSelectionStartPosition = value;
				}
				else
				{
					SelectionStartPosition = value;
				}
			}
		}

		/// <summary>The end position of the text selection</summary>
		public virtual int SelectionEndPosition
		{
			get { return selectionEndPosition; }
			protected set
			{
				SetSelectionEndPosition(value, true);
			}
		}

		/// <summary>The end position of the processed text selection (for live processing)</summary>
		public virtual int ProcessedSelectionEndPosition
		{
			get { return processedSelectionEndPosition; }
			protected set
			{
				processedSelectionEndPosition = value;

				string text = InputField.Text;
				string processedText = InputField.ProcessedText;
				selectionEndPosition = InputField.LiveProcessingFilter.DetermineCaret(text, processedText, processedSelectionEndPosition);
				SelectionRenderer.UpdateSelection(VisibleSelectionStartPosition, VisibleSelectionEndPosition);
			}
		}

		/// <summary>The end position of the text selection in currently rendered text</summary>
		public virtual int VisibleSelectionEndPosition
		{
			get
			{
				if(InputField.LiveProcessing)
				{
					return ProcessedSelectionEndPosition;
				}
				else
				{
					return SelectionEndPosition;
				}
			}
			set
			{
				if(InputField.LiveProcessing)
				{
					ProcessedSelectionEndPosition = value;
				}
				else
				{
					SelectionEndPosition = value;
				}
			}
		}

		/// <summary>The currently selected text</summary>
		public string SelectedText
		{
			get
			{
				if(HasSelection)
				{
					return Text.Substring(SelectionStartPosition, SelectionEndPosition - SelectionStartPosition);
				}
				return string.Empty;
			}
		}

		/// <summary>The character count of the rendered text</summary>
		public int CharacterCount
		{
			get { return TextRenderer.CharacterCount; }
		}

		/// <summary>The character count of the whole text</summary>
		public int TotalCharacterCount
		{
			get { return InputField.Text.Length; }
		}

		/// <summary>The line count</summary>
		public int LineCount
		{
			get { return TextRenderer.LineCount; }
		}

		/// <summary>Indicates if some text is currently selected</summary>
		public bool HasSelection
		{
			get { return (SelectionEndPosition - SelectionStartPosition) != 0; }
		}

		/// <summary>Indicates whether to currrently block selection change events from being send to the native bindings</summary>
		public bool BlockNativeSelectionChange { get; set; }

		/// <summary>Initializes this class</summary>
		internal virtual void Initialize(AdvancedInputField inputField, Image caretRenderer, TextSelectionRenderer selectionRenderer)
		{
			InputField = inputField;
			CaretRenderer = caretRenderer;
			SelectionRenderer = selectionRenderer;

			CaretRenderer.rectTransform.sizeDelta = new Vector2(InputField.CaretWidth, TextRenderer.FontSize);
			CaretRenderer.color = InputField.CaretColor;
			SelectionRenderer.color = InputField.SelectionColor;

			SelectionRenderer.Initialize(inputField);
		}

		/// <summary>Sets the caret position</summary>
		/// <param name="caretPosition">The caret position value</param>
		/// <param name="invokeCaretPositonChangeEvent">Indicates whether this method should invoke the Caret Position Change event</param>
		internal virtual void SetCaretPosition(int caretPosition, bool invokeCaretPositonChangeEvent = false)
		{
			if(this.caretPosition != caretPosition)
			{
				this.caretPosition = caretPosition;
				if(invokeCaretPositonChangeEvent && InputField.OnCaretPositionChanged != null)
				{
					InputField.StartCoroutine(InputField.DelayedCaretPositionChanged());
				}
			}

			if(InputField.LiveProcessing)
			{
				string text = InputField.Text;
				string processedText = InputField.ProcessedText;
				ProcessedCaretPosition = InputField.LiveProcessingFilter.DetermineProcessedCaret(text, this.caretPosition, processedText);
			}
			else
			{
				UpdateCaret();
			}
		}

		/// <summary>Sets the selection start position</summary>
		/// <param name="selectionStartPosition">The selection start position value</param>
		/// <param name="invokeTextSelectionChangeEvent">Indicates whether this method should invoke the Text Selection Change event</param>
		internal virtual void SetSelectionStartPosition(int selectionStartPosition, bool invokeTextSelectionChangeEvent = false)
		{
			if(this.selectionStartPosition != selectionStartPosition)
			{
				this.selectionStartPosition = selectionStartPosition;
				if(invokeTextSelectionChangeEvent && InputField.OnTextSelectionChanged != null)
				{
					InputField.StartCoroutine(InputField.DelayedTextSelectionChanged());
				}
			}

			if(InputField.LiveProcessing)
			{
				string text = InputField.Text;
				string processedText = InputField.ProcessedText;
				processedSelectionStartPosition = InputField.LiveProcessingFilter.DetermineProcessedCaret(text, this.selectionStartPosition, processedText);
			}

			SelectionRenderer.UpdateSelection(VisibleSelectionStartPosition, VisibleSelectionEndPosition);
		}

		/// <summary>Sets the selection end position</summary>
		/// <param name="selectionEndPosition">The selection end position value</param>
		/// <param name="invokeTextSelectionChangeEvent">Indicates whether this method should invoke the Text Selection Change event</param>
		internal virtual void SetSelectionEndPosition(int selectionEndPosition, bool invokeTextSelectionChangeEvent = false)
		{
			if(this.selectionEndPosition != selectionEndPosition)
			{
				this.selectionEndPosition = selectionEndPosition;
				if(invokeTextSelectionChangeEvent && InputField.OnTextSelectionChanged != null)
				{
					InputField.StartCoroutine(InputField.DelayedTextSelectionChanged());
				}
			}

			if(InputField.LiveProcessing)
			{
				string text = InputField.Text;
				string processedText = InputField.ProcessedText;
				processedSelectionEndPosition = InputField.LiveProcessingFilter.DetermineProcessedCaret(text, this.selectionEndPosition, processedText);
			}

			SelectionRenderer.UpdateSelection(VisibleSelectionStartPosition, VisibleSelectionEndPosition);
		}

		internal void UpdateSelection()
		{
			SelectionRenderer.UpdateSelection(VisibleSelectionStartPosition, VisibleSelectionEndPosition, true);
		}

		internal virtual void OnCanvasScaleChanged(float canvasScaleFactor)
		{
			UpdateSelection();
		}

		/// <summary>Gets the character index from position</summary>
		/// <param name="position">The position to use</param>
		internal int GetCharacterIndexFromPosition(TextRenderer textRenderer, Vector2 position)
		{
			position.x -= TextContentTransform.anchoredPosition.x;

			TextAlignment alignment = textRenderer.TextAlignment;
			if(alignment == TextAlignment.BOTTOM || alignment == TextAlignment.CENTER || alignment == TextAlignment.TOP)
			{
				position.x -= (TextAreaTransform.rect.width * 0.5f);
				position.x += (TextContentTransform.rect.width * 0.5f);
			}
			else if(alignment == TextAlignment.BOTTOM_RIGHT || alignment == TextAlignment.RIGHT || alignment == TextAlignment.TOP_RIGHT)
			{
				position.x -= TextAreaTransform.rect.width;
				position.x += TextContentTransform.rect.width;
			}

			if(alignment == TextAlignment.LEFT || alignment == TextAlignment.CENTER || alignment == TextAlignment.RIGHT)
			{
				position.y += (TextAreaTransform.rect.height * 0.5f);
				position.y -= (TextContentTransform.rect.height * 0.5f);
			}
			else if(alignment == TextAlignment.BOTTOM_LEFT || alignment == TextAlignment.BOTTOM || alignment == TextAlignment.BOTTOM_RIGHT)
			{
				position.y += (TextAreaTransform.rect.height * 1f);
				position.y -= (TextContentTransform.rect.height * 1f);
			}

			if(textRenderer.LineCount == 0)
			{
				return 0;
			}

			int line = GetUnclampedCharacterLineFromPosition(textRenderer, position);
			if(line < 0)
			{
				return 0;
			}
			else if(line >= textRenderer.LineCount)
			{
				return textRenderer.CharacterCountVisible;
			}

			int startCharIndex = textRenderer.GetLineInfo(line).startCharIdx;
			int endCharIndex = GetLineEndCharIndex(textRenderer, line);

			for(int i = startCharIndex; i < endCharIndex; i++)
			{
				if(i >= textRenderer.CharacterCountVisible)
				{
					break;
				}

				CharacterInfo charInfo = textRenderer.GetCharacterInfo(i);

				float distToCharStart = position.x - charInfo.position.x;
				float distToCharEnd = charInfo.position.x + charInfo.width - position.x;
				if(distToCharStart < distToCharEnd)
				{
					return i;
				}
			}

			return endCharIndex;
		}

		/// <summary>Gets the unclamped character line from position</summary>
		/// <param name="position">The position to use</param>
		/// <param name="textGenerator">The text generator to use</param>
		internal int GetUnclampedCharacterLineFromPosition(TextRenderer textRenderer, Vector2 position)
		{
			position.y -= TextContentTransform.anchoredPosition.y;
			if(!InputField.Multiline)
			{
				return 0;
			}

			float y = position.y;
			float lastBottomY = 0.0f;

			for(int i = 0; i < textRenderer.LineCount; ++i)
			{
				LineInfo lineInfo = textRenderer.GetLineInfo(i);
				float topY = lineInfo.topY;
				float bottomY = topY - lineInfo.height;

				if(y > topY)
				{
					float leading = topY - lastBottomY;
					if(y > topY - (0.5f * leading))
					{
						return i - 1;
					}
					else
					{
						return i;
					}
				}

				if(y > bottomY)
				{
					return i;
				}

				lastBottomY = bottomY;
			}

			return textRenderer.LineCount;
		}

		/// <summary>Refreshes the rendered text</summary>
		internal void RefreshRenderedText(bool forceRefresh = false)
		{
			string text = null;
			if(InputField.LiveProcessing)
			{
				text = ProcessedText;

				if(ProcessedRenderedText != text)
				{
					InputField.RenderedText = text;
				}
			}
			else
			{
				if(Text.Length > 0)
				{
					text = Text;

					if(InputField.Secure)
					{
						text = new string('*', text.Length);
					}
				}
				else
				{
					text = Text;
				}

				if(RenderedText != text || forceRefresh)
				{
					if(!InputField.LiveProcessing)
					{
						TextRenderer textRenderer = InputField.TextRenderer;
						textRenderer.Text = text;

						if(InputField.LineType == LineType.SingleLine)
						{
							if(!textRenderer.IsReady())
							{
								InputField.StartCoroutine(DelayedRefreshRenderedText(textRenderer));
								return;
							}

							textRenderer.UpdateImmediately(true);
						}
						else
						{
							textRenderer.UpdateImmediately(false);
						}
					}

					InputField.RenderedText = text;
					UpdateCaret();
				}
			}

			InputField.UpdateActiveTextRenderer();
		}

		internal IEnumerator DelayedRefreshRenderedText(TextRenderer textRenderer)
		{
			yield return null;
			textRenderer.UpdateImmediately("", true);
			RefreshRenderedText(true);
		}

		/// <summary>Updates the horizontal scroll position for given TextRenderer</summary>
		internal void UpdateHorizontalScrollPosition(TextRenderer textRenderer, float scrollSensitivity = 1)
		{
			Vector2 areaSize = TextAreaTransform.rect.size;
			Vector2 contentSize = TextContentTransform.rect.size;
			Vector2 contentPosition = TextContentTransform.anchoredPosition;
			Vector2 caretSize = CaretRenderer.rectTransform.rect.size;
			Vector2 caretPosition = CaretRenderer.rectTransform.anchoredPosition;

			Vector2 position = contentPosition + caretPosition;
			if(position.x < 0)
			{
				contentPosition.x += Mathf.Abs(position.x);
			}
			else if(position.x + caretSize.x > areaSize.x)
			{
				contentPosition.x -= Mathf.Abs((position.x + caretSize.x) - areaSize.x);
			}
			TextAreaTransform.GetComponent<ScrollArea>().MoveContentHorizontally(contentPosition, scrollSensitivity * InputField.ScrollSpeed);
		}

		/// <summary>Updates the vertical scroll position for given TextRenderer</summary>
		internal void UpdateVerticalScrollPosition(TextRenderer textRenderer, float scrollSensitivity = 1)
		{
			Vector2 areaSize = TextAreaTransform.rect.size;
			Vector2 contentSize = TextContentTransform.rect.size;
			Vector2 contentPosition = TextContentTransform.anchoredPosition;
			Vector2 caretSize = CaretRenderer.rectTransform.rect.size;
			Vector2 caretPosition = CaretRenderer.rectTransform.anchoredPosition;

			Vector2 position = contentPosition + caretPosition;
			if(position.y > 0)
			{
				contentPosition.y -= Mathf.Abs(position.y);
			}
			else if(position.y - caretSize.y < -areaSize.y)
			{
				contentPosition.y += Mathf.Abs(-areaSize.y - (position.y - caretSize.y));
			}
			TextAreaTransform.GetComponent<ScrollArea>().MoveContentVertically(contentPosition, scrollSensitivity * InputField.ScrollSpeed);
		}

		/// <summary>Gets the character index  of the line start</summary>
		/// <param name="line">The line to check</param>
		internal int GetLineStartCharIndex(TextRenderer textRenderer, int line)
		{
			line = Mathf.Clamp(line, 0, textRenderer.LineCount - 1);
			return textRenderer.GetLineInfo(line).startCharIdx;
		}

		/// <summary>Gets the character index  of the line end</summary>
		/// <param name="line">The line to check</param>
		internal int GetLineEndCharIndex(TextRenderer textRenderer, int line)
		{
			return textRenderer.GetLineEndCharIndex(line);
		}

		/// <summary>Updates the caret position</summary>
		internal virtual void UpdateCaret()
		{
			TextRenderer activeTextRenderer = InputField.GetActiveTextRenderer();
			UpdateCaret(activeTextRenderer);
		}

		internal virtual void UpdateCaret(TextRenderer textRenderer)
		{
			bool fixEmptyCaret = false;
			if(textRenderer.CharacterCount == 0 || Text.Length == 0) //Workaround to make sure the text generator will give a correct position for the first character
			{
				fixEmptyCaret = true;
				textRenderer.UpdateImmediately(" ");
			}

			int charIndex = Mathf.Clamp(VisibleCaretPosition, 0, textRenderer.CharacterCount - 1);
			CharacterInfo charInfo = textRenderer.GetCharacterInfo(charIndex);
			int lineIndex = DetermineCharacterLine(textRenderer, charIndex);
			LineInfo lineInfo = textRenderer.GetLineInfo(lineIndex);

			Vector2 cursorPosition = new Vector2(charInfo.position.x, lineInfo.topY);
			if(VisibleCaretPosition >= textRenderer.CharacterCountVisible)
			{
				cursorPosition.x += charInfo.width;
			}

			CaretRenderer.rectTransform.anchoredPosition = cursorPosition;
			CaretRenderer.rectTransform.sizeDelta = new Vector2(InputField.CaretWidth, lineInfo.height);

			if(fixEmptyCaret)
			{
				textRenderer.UpdateImmediately(string.Empty);
			}

			if(InputField.Multiline)
			{
				UpdateVerticalScrollPosition(textRenderer);
			}
			else
			{
				UpdateHorizontalScrollPosition(textRenderer);
			}
		}

		/// <summary>Begins Edit mode</summary>
		internal virtual void BeginEditMode()
		{
			if(!EditMode)
			{
				EditMode = true;
				caretBlinkTime = 0;
				CaretRenderer.enabled = true;

				ScrollArea scrollArea = TextAreaTransform.GetComponent<ScrollArea>();
				scrollArea.EditMode = true;
			}
		}

		/// <summary>Ends Edit mode</summary>
		internal virtual void EndEditMode()
		{
			EditMode = false;
			caretBlinkTime = InputField.CaretBlinkRate;
			CaretRenderer.enabled = false;
			CaretPosition = 0;
			CancelSelection();

			ScrollArea scrollArea = TextAreaTransform.GetComponent<ScrollArea>();
			switch(InputField.ScrollBehaviourOnEndEdit)
			{
				case ScrollBehaviourOnEndEdit.START_OF_TEXT: scrollArea.MoveContentImmediately(Vector2.zero); break;
			}
			scrollArea.EditMode = false;
		}

		/// <summary>Update method when selected</summary>
		internal virtual void UpdateSelected()
		{
			if(EditMode)
			{
				if(!HasSelection)
				{
					UpdateCaretBlink();
				}
				else
				{
					CaretRenderer.enabled = false; //Don't show caret if we have text selected
				}
			}
		}

		/// <summary>Updates the caret blink</summary>
		internal void UpdateCaretBlink()
		{
			caretBlinkTime += Time.deltaTime;
			caretBlinkTime = Mathf.Min(caretBlinkTime, InputField.CaretBlinkRate);
			float progress = caretBlinkTime / InputField.CaretBlinkRate;
			if(progress <= 0.5f)
			{
				CaretRenderer.enabled = true;
			}
			else
			{
				CaretRenderer.enabled = false;
			}

			if(progress == 1)
			{
				caretBlinkTime = 0;
			}
		}

		/// <summary>Moves caret to start of the text</summary>
		internal void MoveToStart()
		{
			CaretPosition = 0;
		}

		/// <summary>Moves caret to end of the text</summary>
		internal void MoveToEnd()
		{
			CaretPosition = Text.Length;
		}

		/// <summary>Select current word at caret position</summary>
		internal void SelectCurrentWord()
		{
			string renderedText = null;
			if(InputField.LiveProcessing)
			{
				renderedText = ProcessedRenderedText;
			}
			else
			{
				renderedText = RenderedText;
			}

			VisibleSelectionStartPosition = FindPreviousWordStart(VisibleCaretPosition, renderedText);
			VisibleSelectionEndPosition = FindNextWordStart(VisibleCaretPosition, renderedText);

			string selectedString = renderedText.Substring(VisibleSelectionStartPosition, VisibleSelectionEndPosition - VisibleSelectionStartPosition);
			int separatorIndex = selectedString.IndexOfAny(WORD_SEPARATOR_CHARS);
			if(separatorIndex != -1) //There a 2 words selected
			{
				int word1EndIndex = VisibleSelectionStartPosition + separatorIndex;
				int word2StartIndex = VisibleSelectionStartPosition + 1 + selectedString.LastIndexOfAny(WORD_SEPARATOR_CHARS);

				if(VisibleCaretPosition - word1EndIndex < word2StartIndex - VisibleCaretPosition) //Previous word is closer
				{
					VisibleSelectionEndPosition = word1EndIndex;
				}
				else //Next word is closer
				{
					VisibleSelectionStartPosition = word2StartIndex;
				}
			}
		}

		/// <summary>Selects all text</summary>
		internal void SelectAll()
		{
			SelectionStartPosition = 0;
			SelectionEndPosition = Text.Length;
		}

		/// <summary>Finds the start of previous word</summary>
		/// <param name="position">The character position to start checking from</param>
		/// <param name="text">The text to use</param>
		/// <returns>The start position of previous word</returns>
		internal int FindPreviousWordStart(int position, string text)
		{
			if(position - 2 < 0)
			{
				return 0;
			}

			int wordSeparatorPosition = text.LastIndexOfAny(WORD_SEPARATOR_CHARS, position - 2);
			if(wordSeparatorPosition == -1)
			{
				wordSeparatorPosition = 0;
			}
			else
			{
				wordSeparatorPosition++;
			}

			return wordSeparatorPosition;
		}

		/// <summary>Finds the start of next word</summary>
		/// <param name="position">The character position to start checking from</param>
		/// <param name="text">The text to use</param>
		/// <returns>The start position of next word</returns>
		internal int FindNextWordStart(int position, string text)
		{
			if(position + 1 >= text.Length)
			{
				return text.Length;
			}

			int wordSeparatorPosition = text.IndexOfAny(WORD_SEPARATOR_CHARS, position + 1);
			if(wordSeparatorPosition == -1)
			{
				wordSeparatorPosition = text.Length;
			}
			else
			{
				wordSeparatorPosition++;
			}

			return wordSeparatorPosition;
		}

		/// <summary>Finds the character position of next new line</summary>
		/// <param name="position">The character position to start checking from</param>
		/// <returns>The character position of next new line</returns>
		internal int NewLineDownPosition(int position)
		{
			if(InputField.LineType == LineType.SingleLine)
			{
				return Text.Length;
			}

			if(position + 1 >= Text.Length)
			{
				return Text.Length - 1;
			}

			int newLinePosition = Text.IndexOf('\n', position + 1);
			if(newLinePosition == -1)
			{
				return Text.Length - 1;
			}

			return newLinePosition;
		}

		/// <summary>Finds the character position of next line</summary>
		/// <param name="position">The character position to start checking from</param>
		/// <returns>The character position of next line</returns>
		internal int LineDownPosition(int position)
		{
			int visiblePosition = position;
			if(InputField.LineType == LineType.SingleLine)
			{
				return Text.Length;
			}

			CharacterInfo originChar = TextRenderer.GetCharacterInfo(visiblePosition);
			int originLine = DetermineCharacterLine(TextRenderer, visiblePosition);

			if(originLine + 1 >= TextRenderer.LineCount) // We are on the last line return last character
			{
				return (CharacterCount - 1);
			}

			int endCharIdx = GetLineEndCharIndex(TextRenderer, originLine + 1); // Need to determine end line for next line.

			for(int i = TextRenderer.GetLineInfo(originLine + 1).startCharIdx; i < endCharIdx; ++i)
			{
				if(TextRenderer.GetCharacterInfo(i).position.x >= originChar.position.x)
				{
					return i;
				}
			}
			return endCharIdx;
		}

		/// <summary>Finds the character position of previous new line</summary>
		/// <param name="position">The character position to start checking from</param>
		/// <returns>The character position of previous new line</returns>
		internal int NewLineUpPosition(int position)
		{
			if(InputField.LineType == LineType.SingleLine)
			{
				return 0;
			}

			if(position - 1 <= 0)
			{
				return 0;
			}

			int newLinePosition = Text.LastIndexOf('\n', position - 1, position);
			if(newLinePosition == -1)
			{
				return 0;
			}

			return newLinePosition;
		}

		/// <summary>Finds the character position of previous line</summary>
		/// <param name="position">The character position to start checking from</param>
		/// <returns>The character position of previous line</returns>
		internal int LineUpPosition(int position)
		{
			int visiblePosition = position;
			if(InputField.LineType == LineType.SingleLine)
			{
				return 0;
			}

			CharacterInfo originChar = TextRenderer.GetCharacterInfo(visiblePosition);
			int originLine = DetermineCharacterLine(TextRenderer, visiblePosition);

			if(originLine <= 0) // We are on the first line return first character
			{
				return 0;
			}

			int endCharIdx = TextRenderer.GetLineInfo(originLine).startCharIdx - 1;

			for(int i = TextRenderer.GetLineInfo(originLine - 1).startCharIdx; i < endCharIdx; ++i)
			{
				if(TextRenderer.GetCharacterInfo(i).position.x >= originChar.position.x)
				{
					return i;
				}
			}
			return endCharIdx;
		}

		/// <summary>Cancels current text selection</summary>
		internal void CancelSelection()
		{
			SelectionStartPosition = 0;
			SelectionEndPosition = 0;
		}

		/// <summary>Determines the character line of character position</summary>
		/// <param name="textGenerator">The TextGenerator to use</param>
		/// <param name="charPosition">The character position to check</param>
		/// <returns>The character line</returns>
		internal int DetermineCharacterLine(TextRenderer textRenderer, int charPosition)
		{
			for(int i = 0; i < textRenderer.LineCount - 1; ++i)
			{
				if(textRenderer.GetLineInfo(i + 1).startCharIdx > charPosition)
				{
					return i;
				}
			}

			return textRenderer.LineCount - 1;
		}

		/// <summary>Moves caret</summary>
		/// <param name="amount">The amount to move the caret</param>
		internal void MoveCaret(int amount)
		{
			CaretPosition += amount;
		}

		/// <summary>Sets caret position to selection start</summary>
		internal void CaretToSelectionStart()
		{
			CaretPosition = SelectionStartPosition;
		}

		/// <summary>Sets caret position to selection end</summary>
		internal void CaretToSelectionEnd()
		{
			CaretPosition = SelectionEndPosition;
		}

		/// <summary>Sets selection end position to selection start</summary>
		internal void SelectionEndToSelectionStart()
		{
			SelectionEndPosition = SelectionStartPosition;
		}

		/// <summary>Resets the caret based on position</summary>
		/// <param name="position">The position to check</param>
		internal void ResetCaret(Vector2 position)
		{
			if(CharacterCount > Text.Length + 1) //Text hasn't been updated yet, probably was disabled
			{
				VisibleCaretPosition = 0;
			}
			else
			{
				if(InputField.GetActiveTextRenderer() == InputField.PlaceholderTextRenderer)
				{
					VisibleCaretPosition = 0;
				}
				else if(InputField.LiveProcessing)
				{
					VisibleCaretPosition = GetCharacterIndexFromPosition(ProcessedTextRenderer, position);
				}
				else
				{
					VisibleCaretPosition = GetCharacterIndexFromPosition(TextRenderer, position);
				}
			}

			VisibleSelectionStartPosition = VisibleCaretPosition;
			VisibleSelectionEndPosition = VisibleCaretPosition;
		}

		/// <summary>Updates the selection area when dragging</summary>
		/// <param name="currentPosition">The current position</param>
		/// <param name="pressPosition">The position when press started</param>
		/// <param name="autoSelectWord">Indicates whether current word should be selected automatically</param>
		internal void UpdateSelectionArea(int currentPosition, int pressPosition, bool autoSelectWord)
		{
			if(currentPosition < pressPosition)
			{
				if(InputField.LiveProcessing)
				{
					ProcessedSelectionStartPosition = currentPosition;
					ProcessedSelectionEndPosition = pressPosition;
					ProcessedCaretPosition = currentPosition;
				}
				else
				{
					SelectionStartPosition = currentPosition;
					if(autoSelectWord)
					{
						SelectionEndPosition = FindNextWordStart(pressPosition, Text);
					}
					else
					{
						SelectionEndPosition = pressPosition;
					}

					CaretPosition = currentPosition;
				}
			}
			else if(currentPosition > pressPosition)
			{
				if(InputField.LiveProcessing)
				{
					ProcessedSelectionStartPosition = pressPosition;
					ProcessedSelectionEndPosition = currentPosition;
					ProcessedCaretPosition = currentPosition;
				}
				else
				{
					if(autoSelectWord)
					{
						SelectionStartPosition = FindPreviousWordStart(pressPosition, Text);
					}
					else
					{
						SelectionStartPosition = pressPosition;
					}

					SelectionEndPosition = currentPosition;
					CaretPosition = currentPosition;
				}
			}
			else
			{
				if(InputField.LiveProcessing)
				{
					ProcessedSelectionStartPosition = currentPosition;
					ProcessedSelectionEndPosition = currentPosition;
				}
				else
				{
					SelectionStartPosition = currentPosition;
					SelectionEndPosition = currentPosition;
				}
			}
		}

		/// <summary>Method called when InputField gets destroyed</summary>
		internal virtual void OnDestroy()
		{
		}
	}
}
