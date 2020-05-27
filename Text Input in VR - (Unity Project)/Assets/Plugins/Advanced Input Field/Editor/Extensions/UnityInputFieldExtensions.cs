using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin.Editor
{
	public static class UnityInputFieldExtensions
	{
		public static RectTransform ExtractData(this InputField inputField, ref InputFieldData inputFieldData, ref TextRendererData textRendererData, ref TextRendererData placeholderRendererData)
		{
			RectTransform rectTransform = inputField.GetComponent<RectTransform>();
			inputFieldData = inputField.ExtractInputFieldData();

			Text fromText = inputField.textComponent;
			if(fromText != null)
			{
				textRendererData = fromText.ExtractTextRendererData();
			}

			Graphic fromPlaceholder = inputField.placeholder;
			if(fromPlaceholder != null && fromPlaceholder.GetComponent<Text>() != null)
			{
				placeholderRendererData = fromPlaceholder.GetComponent<Text>().ExtractTextRendererData();
			}

			return rectTransform;
		}

		public static void ApplyData(this InputField inputField, RectTransform sourceTransform, InputFieldData inputFieldData, TextRendererData textRendererData, TextRendererData placeholderRendererData)
		{
			RectTransform rectTransform = inputField.GetComponent<RectTransform>();
			rectTransform.CopyValues(sourceTransform);
			inputField.name = sourceTransform.name;
			inputField.ApplyInputFieldData(inputFieldData);

			Text text = inputField.textComponent;
			if(text != null && textRendererData != null)
			{
				text.ApplyTextRendererData(textRendererData);
			}

			Graphic placeholder = inputField.placeholder;
			if(placeholder != null)
			{
				Text placeholderText = placeholder.GetComponent<Text>();
				if(placeholderText != null && placeholderRendererData != null)
				{
					placeholderText.ApplyTextRendererData(placeholderRendererData);
				}
			}
		}

		public static InputFieldData ExtractInputFieldData(this InputField inputField)
		{
			InputFieldData data = new InputFieldData();
			data.interactable = inputField.interactable;
			data.text = inputField.text;
			data.placeholder = ExtractPlaceholder(inputField);
			data.characterLimit = inputField.characterLimit;
			data.contentType = ExtractContentType(inputField);
			data.lineType = ExtractLineType(inputField);
			data.inputType = ExtractInputType(inputField);
			data.keyboardType = ExtractKeyboardType(inputField);
			data.characterValidation = ExtractCharacterValidation(inputField);
			data.caretBlinkRate = inputField.caretBlinkRate;
			data.caretWidth = inputField.caretWidth;
			data.caretColor = inputField.caretColor;
			data.selectionColor = inputField.selectionColor;
			data.readOnly = inputField.readOnly;

			return data;
		}

		public static void ApplyInputFieldData(this InputField inputField, InputFieldData data)
		{
			Undo.RecordObject(inputField, "Undo " + inputField.GetInstanceID());
			inputField.interactable = data.interactable.GetValueOrDefault();
			if(data.text != null)
			{
				inputField.text = data.text;
			}
			inputField.ApplyPlaceHolder(data);
			inputField.characterLimit = data.characterLimit.GetValueOrDefault();
			inputField.ApplyContentType(data);
			inputField.ApplyLineType(data);
			inputField.ApplyInputType(data);
			inputField.ApplyKeyboardType(data);
			inputField.ApplyCharacterValidation(data);
			inputField.caretBlinkRate = data.caretBlinkRate.GetValueOrDefault();
			inputField.caretWidth = Mathf.RoundToInt(data.caretWidth.GetValueOrDefault());
			inputField.caretColor = data.caretColor.GetValueOrDefault();
			inputField.selectionColor = data.selectionColor.GetValueOrDefault();
			inputField.readOnly = data.readOnly.GetValueOrDefault();
		}

		public static string ExtractPlaceholder(this InputField inputField)
		{
			Text text = inputField.placeholder.GetComponent<Text>();
			if(text != null)
			{
				return text.text;
			}

			return null;
		}

		public static void ApplyPlaceHolder(this InputField inputField, InputFieldData data)
		{
			Text text = inputField.placeholder.GetComponent<Text>();
			if(text != null)
			{
				text.text = data.placeholder;
			}
		}

		public static ContentType ExtractContentType(this InputField inputField)
		{
			switch(inputField.contentType)
			{
				case InputField.ContentType.Alphanumeric: return ContentType.Alphanumeric;
				case InputField.ContentType.Autocorrected: return ContentType.Autocorrected;
				case InputField.ContentType.Custom: return ContentType.Custom;
				case InputField.ContentType.DecimalNumber: return ContentType.DecimalNumber;
				case InputField.ContentType.EmailAddress: return ContentType.EmailAddress;
				case InputField.ContentType.IntegerNumber: return ContentType.IntegerNumber;
				case InputField.ContentType.Name: return ContentType.Name;
				case InputField.ContentType.Password: return ContentType.Password;
				case InputField.ContentType.Pin: return ContentType.Pin;
				case InputField.ContentType.Standard: return ContentType.Standard;
				default: return ContentType.Standard;
			}
		}

		public static void ApplyContentType(this InputField inputField, InputFieldData data)
		{
			ContentType contentType = data.contentType.GetValueOrDefault();

			switch(contentType)
			{
				case ContentType.Alphanumeric: inputField.contentType = InputField.ContentType.Alphanumeric; break;
				case ContentType.Autocorrected: inputField.contentType = InputField.ContentType.Autocorrected; break;
				case ContentType.Custom: inputField.contentType = InputField.ContentType.Custom; break;
				case ContentType.DecimalNumber: inputField.contentType = InputField.ContentType.DecimalNumber; break;
				case ContentType.EmailAddress: inputField.contentType = InputField.ContentType.EmailAddress; break;
				case ContentType.IntegerNumber: inputField.contentType = InputField.ContentType.IntegerNumber; break;
				case ContentType.Name: inputField.contentType = InputField.ContentType.Name; break;
				case ContentType.Password: inputField.contentType = InputField.ContentType.Password; break;
				case ContentType.Pin: inputField.contentType = InputField.ContentType.Pin; break;
				case ContentType.Standard: inputField.contentType = InputField.ContentType.Standard; break;
			}
		}

		public static LineType ExtractLineType(this InputField inputField)
		{
			switch(inputField.lineType)
			{
				case InputField.LineType.MultiLineNewline: return LineType.MultiLineNewline;
				case InputField.LineType.MultiLineSubmit: return LineType.MultiLineSubmit;
				case InputField.LineType.SingleLine: return LineType.SingleLine;
				default: return LineType.SingleLine;
			}
		}

		public static void ApplyLineType(this InputField inputField, InputFieldData data)
		{
			LineType lineType = data.lineType.GetValueOrDefault();

			switch(lineType)
			{
				case LineType.MultiLineNewline: inputField.lineType = InputField.LineType.MultiLineNewline; break;
				case LineType.MultiLineSubmit: inputField.lineType = InputField.LineType.MultiLineSubmit; break;
				case LineType.SingleLine: inputField.lineType = InputField.LineType.SingleLine; break;
			}
		}

		public static InputType ExtractInputType(this InputField inputField)
		{
			switch(inputField.inputType)
			{
				case InputField.InputType.AutoCorrect: return InputType.AutoCorrect;
				case InputField.InputType.Password: return InputType.Password;
				case InputField.InputType.Standard: return InputType.Standard;
				default: return InputType.Standard;
			}
		}

		public static void ApplyInputType(this InputField inputField, InputFieldData data)
		{
			InputType inputType = data.inputType.GetValueOrDefault();

			switch(inputType)
			{
				case InputType.AutoCorrect: inputField.inputType = InputField.InputType.AutoCorrect; break;
				case InputType.Password: inputField.inputType = InputField.InputType.Password; break;
				case InputType.Standard: inputField.inputType = InputField.InputType.Standard; break;
			}
		}

		public static KeyboardType ExtractKeyboardType(this InputField inputField)
		{
			switch(inputField.keyboardType)
			{
				case TouchScreenKeyboardType.ASCIICapable: return KeyboardType.ASCIICapable;
				case TouchScreenKeyboardType.Default: return KeyboardType.Default;
				case TouchScreenKeyboardType.EmailAddress: return KeyboardType.EmailAddress;
				case TouchScreenKeyboardType.NumberPad: return KeyboardType.NumberPad;
				case TouchScreenKeyboardType.NumbersAndPunctuation: return KeyboardType.NumbersAndPunctuation;
				case TouchScreenKeyboardType.PhonePad: return KeyboardType.PhonePad;
				case TouchScreenKeyboardType.URL: return KeyboardType.URL;
				default: return KeyboardType.Default;
			}
		}

		public static void ApplyKeyboardType(this InputField inputField, InputFieldData data)
		{
			KeyboardType keyboardType = data.keyboardType.GetValueOrDefault();

			switch(keyboardType)
			{
				case KeyboardType.ASCIICapable: inputField.keyboardType = TouchScreenKeyboardType.ASCIICapable; break;
				case KeyboardType.Default: inputField.keyboardType = TouchScreenKeyboardType.Default; break;
				case KeyboardType.EmailAddress: inputField.keyboardType = TouchScreenKeyboardType.EmailAddress; break;
				case KeyboardType.NumberPad: inputField.keyboardType = TouchScreenKeyboardType.NumberPad; break;
				case KeyboardType.NumbersAndPunctuation: inputField.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation; break;
				case KeyboardType.PhonePad: inputField.keyboardType = TouchScreenKeyboardType.PhonePad; break;
				case KeyboardType.URL: inputField.keyboardType = TouchScreenKeyboardType.URL; break;
			}
		}

		public static CharacterValidation ExtractCharacterValidation(this InputField inputField)
		{
			switch(inputField.characterValidation)
			{
				case InputField.CharacterValidation.Alphanumeric: return CharacterValidation.Alphanumeric;
				case InputField.CharacterValidation.Decimal: return CharacterValidation.Decimal;
				case InputField.CharacterValidation.EmailAddress: return CharacterValidation.EmailAddress;
				case InputField.CharacterValidation.Integer: return CharacterValidation.Integer;
				case InputField.CharacterValidation.Name: return CharacterValidation.Name;
				case InputField.CharacterValidation.None: return CharacterValidation.None;
				default: return CharacterValidation.None;
			}
		}

		public static void ApplyCharacterValidation(this InputField inputField, InputFieldData data)
		{
			CharacterValidation characterValidation = data.characterValidation.GetValueOrDefault();

			switch(characterValidation)
			{
				case CharacterValidation.Alphanumeric: inputField.characterValidation = InputField.CharacterValidation.Alphanumeric; break;
				case CharacterValidation.Decimal: inputField.characterValidation = InputField.CharacterValidation.Decimal; break;
				case CharacterValidation.EmailAddress: inputField.characterValidation = InputField.CharacterValidation.EmailAddress; break;
				case CharacterValidation.Integer: inputField.characterValidation = InputField.CharacterValidation.Integer; break;
				case CharacterValidation.Name: inputField.characterValidation = InputField.CharacterValidation.Name; break;
				case CharacterValidation.None: inputField.characterValidation = InputField.CharacterValidation.None; break;
			}
		}
	}
}
