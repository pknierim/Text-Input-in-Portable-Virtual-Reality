#if ADVANCEDINPUTFIELD_TEXTMESHPRO
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin.Editor
{
	public static class TMProInputFieldExtensions
	{
		public static RectTransform ExtractData(this TMP_InputField inputField, ref InputFieldData inputFieldData, ref TextRendererData textRendererData, ref TextRendererData placeholderRendererData)
		{
			RectTransform rectTransform = inputField.GetComponent<RectTransform>();
			inputFieldData = inputField.ExtractInputFieldData();

			TMP_Text fromText = inputField.textComponent;
			if(fromText != null && fromText.GetComponent<TextMeshProUGUI>() != null)
			{
				textRendererData = fromText.GetComponent<TextMeshProUGUI>().ExtractTextRendererData();
			}

			Graphic fromPlaceholder = inputField.placeholder;
			if(fromPlaceholder != null && fromPlaceholder.GetComponent<TextMeshProUGUI>() != null)
			{
				placeholderRendererData = fromPlaceholder.GetComponent<TextMeshProUGUI>().ExtractTextRendererData();
			}

			return rectTransform;
		}

		public static void ApplyData(this TMP_InputField inputField, RectTransform sourceTransform, InputFieldData inputFieldData, TextRendererData textRendererData, TextRendererData placeholderRendererData)
		{
			RectTransform rectTransform = inputField.GetComponent<RectTransform>();
			rectTransform.CopyValues(sourceTransform);
			inputField.name = sourceTransform.name;
			inputField.ApplyInputFieldData(inputFieldData);

			TMP_Text tmpText = inputField.textComponent;
			if(tmpText != null)
			{
				TextMeshProUGUI text = tmpText.GetComponent<TextMeshProUGUI>();
				if(text != null && textRendererData != null)
				{
					text.ApplyTextRendererData(textRendererData);
				}
			}

			Graphic placeholder = inputField.placeholder;
			if(placeholder != null)
			{
				TextMeshProUGUI placeholderText = placeholder.GetComponent<TextMeshProUGUI>();
				if(placeholderText != null && placeholderRendererData != null)
				{
					placeholderText.ApplyTextRendererData(placeholderRendererData);
				}
			}
		}

		public static InputFieldData ExtractInputFieldData(this TMP_InputField inputField)
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

		public static void ApplyInputFieldData(this TMP_InputField inputField, InputFieldData data)
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

		public static string ExtractPlaceholder(this TMP_InputField inputField)
		{
			TextMeshProUGUI text = inputField.placeholder.GetComponent<TextMeshProUGUI>();
			if(text != null)
			{
				return text.text;
			}

			return null;
		}

		public static void ApplyPlaceHolder(this TMP_InputField inputField, InputFieldData data)
		{
			TextMeshProUGUI text = inputField.placeholder.GetComponent<TextMeshProUGUI>();
			if(text != null)
			{
				text.text = data.placeholder;
			}
		}

		public static ContentType ExtractContentType(this TMP_InputField inputField)
		{
			switch(inputField.contentType)
			{
				case TMP_InputField.ContentType.Alphanumeric: return ContentType.Alphanumeric;
				case TMP_InputField.ContentType.Autocorrected: return ContentType.Autocorrected;
				case TMP_InputField.ContentType.Custom: return ContentType.Custom;
				case TMP_InputField.ContentType.DecimalNumber: return ContentType.DecimalNumber;
				case TMP_InputField.ContentType.EmailAddress: return ContentType.EmailAddress;
				case TMP_InputField.ContentType.IntegerNumber: return ContentType.IntegerNumber;
				case TMP_InputField.ContentType.Name: return ContentType.Name;
				case TMP_InputField.ContentType.Password: return ContentType.Password;
				case TMP_InputField.ContentType.Pin: return ContentType.Pin;
				case TMP_InputField.ContentType.Standard: return ContentType.Standard;
				default: return ContentType.Standard;
			}
		}

		public static void ApplyContentType(this TMP_InputField inputField, InputFieldData data)
		{
			ContentType contentType = data.contentType.GetValueOrDefault();

			switch(contentType)
			{
				case ContentType.Alphanumeric: inputField.contentType = TMP_InputField.ContentType.Alphanumeric; break;
				case ContentType.Autocorrected: inputField.contentType = TMP_InputField.ContentType.Autocorrected; break;
				case ContentType.Custom: inputField.contentType = TMP_InputField.ContentType.Custom; break;
				case ContentType.DecimalNumber: inputField.contentType = TMP_InputField.ContentType.DecimalNumber; break;
				case ContentType.EmailAddress: inputField.contentType = TMP_InputField.ContentType.EmailAddress; break;
				case ContentType.IntegerNumber: inputField.contentType = TMP_InputField.ContentType.IntegerNumber; break;
				case ContentType.Name: inputField.contentType = TMP_InputField.ContentType.Name; break;
				case ContentType.Password: inputField.contentType = TMP_InputField.ContentType.Password; break;
				case ContentType.Pin: inputField.contentType = TMP_InputField.ContentType.Pin; break;
				case ContentType.Standard: inputField.contentType = TMP_InputField.ContentType.Standard; break;
			}
		}

		public static LineType ExtractLineType(this TMP_InputField inputField)
		{
			switch(inputField.lineType)
			{
				case TMP_InputField.LineType.MultiLineNewline: return LineType.MultiLineNewline;
				case TMP_InputField.LineType.MultiLineSubmit: return LineType.MultiLineSubmit;
				case TMP_InputField.LineType.SingleLine: return LineType.SingleLine;
				default: return LineType.SingleLine;
			}
		}

		public static void ApplyLineType(this TMP_InputField inputField, InputFieldData data)
		{
			LineType lineType = data.lineType.GetValueOrDefault();

			switch(lineType)
			{
				case LineType.MultiLineNewline: inputField.lineType = TMP_InputField.LineType.MultiLineNewline; break;
				case LineType.MultiLineSubmit: inputField.lineType = TMP_InputField.LineType.MultiLineSubmit; break;
				case LineType.SingleLine: inputField.lineType = TMP_InputField.LineType.SingleLine; break;
			}
		}

		public static InputType ExtractInputType(this TMP_InputField inputField)
		{
			switch(inputField.inputType)
			{
				case TMP_InputField.InputType.AutoCorrect: return InputType.AutoCorrect;
				case TMP_InputField.InputType.Password: return InputType.Password;
				case TMP_InputField.InputType.Standard: return InputType.Standard;
				default: return InputType.Standard;
			}
		}

		public static void ApplyInputType(this TMP_InputField inputField, InputFieldData data)
		{
			InputType inputType = data.inputType.GetValueOrDefault();

			switch(inputType)
			{
				case InputType.AutoCorrect: inputField.inputType = TMP_InputField.InputType.AutoCorrect; break;
				case InputType.Password: inputField.inputType = TMP_InputField.InputType.Password; break;
				case InputType.Standard: inputField.inputType = TMP_InputField.InputType.Standard; break;
			}
		}

		public static KeyboardType ExtractKeyboardType(this TMP_InputField inputField)
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

		public static void ApplyKeyboardType(this TMP_InputField inputField, InputFieldData data)
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

		public static CharacterValidation ExtractCharacterValidation(this TMP_InputField inputField)
		{
			switch(inputField.characterValidation)
			{
				case TMP_InputField.CharacterValidation.Alphanumeric: return CharacterValidation.Alphanumeric;
				case TMP_InputField.CharacterValidation.Decimal: return CharacterValidation.Decimal;
				case TMP_InputField.CharacterValidation.EmailAddress: return CharacterValidation.EmailAddress;
				case TMP_InputField.CharacterValidation.Integer: return CharacterValidation.Integer;
				case TMP_InputField.CharacterValidation.Name: return CharacterValidation.Name;
				case TMP_InputField.CharacterValidation.None: return CharacterValidation.None;
				default: return CharacterValidation.None;
			}
		}

		public static void ApplyCharacterValidation(this TMP_InputField inputField, InputFieldData data)
		{
			CharacterValidation characterValidation = data.characterValidation.GetValueOrDefault();

			switch(characterValidation)
			{
				case CharacterValidation.Alphanumeric: inputField.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric; break;
				case CharacterValidation.Decimal: inputField.characterValidation = TMP_InputField.CharacterValidation.Decimal; break;
				case CharacterValidation.EmailAddress: inputField.characterValidation = TMP_InputField.CharacterValidation.EmailAddress; break;
				case CharacterValidation.Integer: inputField.characterValidation = TMP_InputField.CharacterValidation.Integer; break;
				case CharacterValidation.Name: inputField.characterValidation = TMP_InputField.CharacterValidation.Name; break;
				case CharacterValidation.None: inputField.characterValidation = TMP_InputField.CharacterValidation.None; break;
			}
		}
	}
}
#endif