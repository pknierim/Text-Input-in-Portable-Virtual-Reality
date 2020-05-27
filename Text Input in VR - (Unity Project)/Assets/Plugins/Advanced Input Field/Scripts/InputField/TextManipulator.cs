//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Base class for text string manipulation</summary>
	public class TextManipulator
	{
		/// <summary>The TextValidator</summary>
		private TextValidator textValidator;

		/// <summary>The InputField</summary>
		public AdvancedInputField InputField { get; private set; }

		/// <summary>The TextNavigator/summary>
		public virtual TextNavigator TextNavigator { get; protected set; }

		/// <summary>The main renderer for text</summary>
		public TextRenderer TextRenderer { get; private set; }

		/// <summary>The renderer for processed text</summary>
		public TextRenderer ProcessedTextRenderer { get; private set; }

		/// <summary>The main text string</summary>
		public virtual string Text
		{
			get { return InputField.Text; }
			set
			{
				InputField.SetText(value, true);
			}
		}

		/// <summary>The text in the clipboard</summary>
		public static string Clipboard
		{
			get
			{
				return GUIUtility.systemCopyBuffer;
			}
			set
			{
				GUIUtility.systemCopyBuffer = value;
			}
		}

		/// <summary>Indicates whether to currrently block text change events from being send to the native bindings</summary>
		public bool BlockNativeTextChange { get; set; }

		/// <summary>Initializes the class</summary>
		internal virtual void Initialize(AdvancedInputField inputField, TextNavigator textNavigator, TextRenderer textRenderer, TextRenderer processedTextRenderer)
		{
			InputField = inputField;
			TextNavigator = textNavigator;
			TextRenderer = textRenderer;
			ProcessedTextRenderer = processedTextRenderer;
			textValidator = new TextValidator(InputField.CharacterValidation, InputField.LineType, inputField.CharacterValidator);
		}

		/// <summary>Begins the edit mode</summary>
		internal void BeginEditMode()
		{
			TextNavigator.RefreshRenderedText();
			textValidator.Validator = InputField.CharacterValidator;
			if(InputField.LiveProcessing)
			{
				LiveProcessingFilter liveProcessingFilter = InputField.LiveProcessingFilter;
				string text = InputField.Text;
				int caretPosition = TextNavigator.CaretPosition;
				string processedText = liveProcessingFilter.ProcessText(text, caretPosition);
				if(processedText != null)
				{
					InputField.ProcessedText = processedText;

					int processedCaretPosition = liveProcessingFilter.DetermineProcessedCaret(text, caretPosition, processedText);
					TextNavigator.ProcessedCaretPosition = processedCaretPosition;
				}
			}
		}

		/// <summary>Ends the edit mode</summary>
		internal void EndEditMode()
		{
			if(InputField.PostProcessingFilter != null)
			{
				string processedText = null;
				if(InputField.PostProcessingFilter.ProcessText(Text, out processedText))
				{
					TextRenderer.Hide();
					InputField.ProcessedText = processedText;
				}
			}
		}

		/// <summary>Checks if character is valid</summary>
		/// <param name="c">The character to check</param>
		internal bool IsValidChar(char c)
		{
			if((int)c == 127) //Delete key on mac
			{
				return false;
			}

			if(c == '\t' || c == '\n') // Accept newline and tab
			{
				return true;
			}

			return TextRenderer.FontHasCharacter(c);
		}

		/// <summary>Insert a string at caret position</summary>
		/// <param name="input">the string to insert</param>
		internal virtual void Insert(string input)
		{
			if(InputField.ReadOnly)
			{
				return;
			}

			if(InputField.CharacterLimit > 0 && Text.Length + input.Length > InputField.CharacterLimit)
			{
				if(Text.Length < InputField.CharacterLimit)
				{
					int amountAllowed = InputField.CharacterLimit - Text.Length;
					input = input.Substring(0, amountAllowed);
				}
				else
				{
					return;
				}
			}

			if(TextNavigator.HasSelection)
			{
				DeleteSelection();
			}

			string lastText = Text;

			int selectionStartPosition = -1;
			if(TextNavigator.HasSelection)
			{
				selectionStartPosition = TextNavigator.SelectionStartPosition;
			}
			textValidator.Validate(Text, input, TextNavigator.CaretPosition, selectionStartPosition);
			string resultText = textValidator.ResultText;
			int resultCaretPosition = textValidator.ResultCaretPosition;

			ApplyCharacterLimit(ref resultText, ref resultCaretPosition);
			//TextRenderer.Text = resultText; //TODO: Reenable when line limit works on mobile too (requires partial rewrite native bindings)
			//if(InputField.LineLimit > 0 && TextRenderer.LineCount > InputField.LineLimit)
			//{
			//	if(input.Length > 1)
			//	{
			//		input = input.Substring(0, input.Length - 1);
			//		Insert(input); //Try again to stay within line limit
			//	}
			//	else
			//	{
			//		TextRenderer.Text = lastText;
			//	}

			//	return;
			//}

			Text = resultText;
			TextNavigator.CaretPosition = resultCaretPosition;
		}

		public void ApplyCharacterLimit(ref string text, ref int caretPosition)
		{
			if(InputField.CharacterLimit != 0 && text.Length > InputField.CharacterLimit)
			{
				int amountOverLimit = text.Length - InputField.CharacterLimit;
				text = text.Substring(0, InputField.CharacterLimit);
				caretPosition = Mathf.Clamp(caretPosition, 0, Text.Length);
			}
		}

		/// <summary>Tries to insert text</summary>
		/// <param name="text">The text to insert</param>
		internal void TryInsertText(string text)
		{
			if(InputField.ReadOnly)
			{
				return;
			}

			Insert(text);
		}

		/// <summary>Tries to insert a character</summary>
		/// <param name="c">The character to insert</param>
		internal void TryInsertChar(char c)
		{
			if(!IsValidChar(c))
			{
				return;
			}

			Insert(c.ToString());
		}

		/// <summary>Deletes previous character</summary>
		internal void DeletePreviousChar()
		{
			if(InputField.ReadOnly)
			{
				return;
			}

			int originalLineCount = TextNavigator.LineCount;

			if(TextNavigator.HasSelection)
			{
				DeleteSelection();
			}
			else if(TextNavigator.CaretPosition > 0)
			{
				if(InputField.EmojisAllowed)
				{
					TextRenderer activeTextRenderer = InputField.GetActiveTextRenderer();
					int caretPosition = TextNavigator.CaretPosition;
					CharacterInfo characterInfo = activeTextRenderer.GetCharacterInfo(caretPosition);
					int currentIndex = characterInfo.index; //This will always be the startIndex if surrogate pair
					if(currentIndex > 0)
					{
						CharacterInfo previousCharacterInfo = activeTextRenderer.GetCharacterInfo(currentIndex - 1);
						int amount = currentIndex - previousCharacterInfo.index;
						TextNavigator.MoveCaret(-amount);
						Text = Text.Remove(TextNavigator.CaretPosition, amount);
					}

				}
				else
				{
					TextNavigator.MoveCaret(-1);
					Text = Text.Remove(TextNavigator.CaretPosition, 1);
				}
			}
		}

		/// <summary>Deletes next character</summary>
		internal void DeleteNextChar()
		{
			if(InputField.ReadOnly)
			{
				return;
			}

			int originalLineCount = TextNavigator.LineCount;

			if(TextNavigator.HasSelection)
			{
				DeleteSelection();
			}
			else if(TextNavigator.CaretPosition < Text.Length)
			{
				if(InputField.EmojisAllowed)
				{
					TextRenderer activeTextRenderer = InputField.GetActiveTextRenderer();
					int caretPosition = TextNavigator.CaretPosition;
					CharacterInfo characterInfo = activeTextRenderer.GetCharacterInfo(caretPosition);
					int currentIndex = characterInfo.index; //This will always be the startIndex if surrogate pair
					if(currentIndex < Text.Length)
					{
						TextNavigator.CaretPosition = currentIndex;
						int amount = characterInfo.partCount;
						Text = Text.Remove(TextNavigator.CaretPosition, amount);
					}
				}
				else
				{
					Text = Text.Remove(TextNavigator.CaretPosition, 1);
				}
			}
		}

		/// <summary>Deletes current text selection</summary>
		internal void DeleteSelection()
		{
			if(InputField.ReadOnly)
			{
				return;
			}

			string text = Text.Remove(TextNavigator.SelectionStartPosition, TextNavigator.SelectionEndPosition - TextNavigator.SelectionStartPosition);

			TextNavigator.CaretToSelectionStart();
			TextNavigator.CancelSelection();
			InputField.ResetDragStartPosition(TextNavigator.CaretPosition);
			Text = text;
		}

		/// <summary>Copies current text selection</summary>
		internal virtual void Copy()
		{
			if(!InputField.IsPasswordField)
			{
				Clipboard = TextNavigator.SelectedText;
			}
			else
			{
				Clipboard = string.Empty;
			}
		}

		/// <summary>Pastes clipboard text</summary>
		internal virtual void Paste()
		{
			if(InputField.ReadOnly)
			{
				return;
			}

			string input = Clipboard;
			string processedInput = string.Empty;

			int length = input.Length;
			for(int i = 0; i < length; i++)
			{
				char c = input[i];

				if(c >= ' ' || c == '\t' || c == '\r' || c == 10 || c == '\n')
				{
					processedInput += c;
				}
			}

			if(!string.IsNullOrEmpty(processedInput))
			{
				Insert(processedInput);
			}
		}

		/// <summary>Cuts current text selection</summary>
		internal virtual void Cut()
		{
			if(!InputField.IsPasswordField)
			{
				Clipboard = TextNavigator.SelectedText;
			}
			else
			{
				Clipboard = string.Empty;
			}

			if(InputField.ReadOnly)
			{
				return;
			}

			if(TextNavigator.HasSelection)
			{
				DeleteSelection();
			}
		}
	}
}
