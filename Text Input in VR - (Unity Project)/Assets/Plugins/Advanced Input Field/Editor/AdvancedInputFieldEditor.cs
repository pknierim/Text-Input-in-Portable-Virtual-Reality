//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEditor;
using UnityEngine;

namespace AdvancedInputFieldPlugin.Editor
{
	[CustomEditor(typeof(AdvancedInputField), true)]
	public class AdvancedInputFieldEditor: UnityEditor.Editor
	{
		private const string DEPRECATED_FORMAT_RESOURCE_PATH = "AdvancedInputField/Images/deprecated_format";
		private const string UNITY_FORMAT_IMAGE_PATH = "AdvancedInputField/Images/unity_format";
		private const string TEXTMESHPRO_FORMAT_IMAGE_PATH = "AdvancedInputField/Images/textmeshpro_format";

		private SerializedProperty interactableProperty; //Property from Selectable base class

		private SerializedProperty modeProperty;
		private SerializedProperty textProperty;
		private SerializedProperty placeholderTextProperty;
		private SerializedProperty characterLimitProperty;
		//private SerializedProperty lineLimitProperty; //TODO: Reenable when line limit works on mobile too (requires partial rewrite native bindings)
		private SerializedProperty contentTypeProperty;
		private SerializedProperty lineTypeProperty;
		private SerializedProperty inputTypeProperty;
		private SerializedProperty keyboardTypeProperty;
		private SerializedProperty characterValidationProperty;
		private SerializedProperty characterValidatorProperty;
		private SerializedProperty emojisAllowedProperty;
		private SerializedProperty liveProcessingFilterProperty;
		private SerializedProperty postProcessingFilterProperty;
		private SerializedProperty selectionModeProperty;
		private SerializedProperty dragModeProperty;
		private SerializedProperty caretOnBeginEditProperty;
		private SerializedProperty caretBlinkRateProperty;
		private SerializedProperty caretWidthProperty;
		private SerializedProperty caretColorProperty;
		private SerializedProperty selectionColorProperty;
		private SerializedProperty readOnlyProperty;
		private SerializedProperty scrollBehaviourOnEndEditProperty;
		private SerializedProperty scrollBarsVisibilityModeProperty;
		private SerializedProperty scrollSpeedProperty;
		private SerializedProperty fastScrollSensitivityProperty;
		private SerializedProperty resizeMinWidthProperty;
		private SerializedProperty resizeMinHeightProperty;
		private SerializedProperty onSelectionChangedProperty;
		private SerializedProperty onBeginEditProperty;
		private SerializedProperty onEndEditProperty;
		private SerializedProperty onValueChangedProperty;
		private SerializedProperty onCaretPositionChangedProperty;
		private SerializedProperty onTextSelectionChangedProperty;
		private SerializedProperty onSizeChangedProperty;
		private SerializedProperty actionBarProperty;
		private SerializedProperty actionBarCutProperty;
		private SerializedProperty actionBarCopyProperty;
		private SerializedProperty actionBarPasteProperty;
		private SerializedProperty actionBarSelectAllProperty;
		private SerializedProperty selectionCursorsProperty;
		private SerializedProperty autocapitalizationTypeProperty;
		private SerializedProperty nextInputFieldProperty;

		private void OnEnable()
		{
			interactableProperty = serializedObject.FindProperty("m_Interactable");

			modeProperty = serializedObject.FindProperty("mode");
			textProperty = serializedObject.FindProperty("text");
			placeholderTextProperty = serializedObject.FindProperty("placeholderText");
			characterLimitProperty = serializedObject.FindProperty("characterLimit");
			//lineLimitProperty = serializedObject.FindProperty("lineLimit"); //TODO: Reenable when line limit works on mobile too (requires partial rewrite native bindings)
			contentTypeProperty = serializedObject.FindProperty("contentType");
			lineTypeProperty = serializedObject.FindProperty("lineType");
			inputTypeProperty = serializedObject.FindProperty("inputType");
			keyboardTypeProperty = serializedObject.FindProperty("keyboardType");
			characterValidationProperty = serializedObject.FindProperty("characterValidation");
			characterValidatorProperty = serializedObject.FindProperty("characterValidator");
			emojisAllowedProperty = serializedObject.FindProperty("emojisAllowed");
			liveProcessingFilterProperty = serializedObject.FindProperty("liveProcessingFilter");
			postProcessingFilterProperty = serializedObject.FindProperty("postProcessingFilter");
			selectionModeProperty = serializedObject.FindProperty("selectionMode");
			dragModeProperty = serializedObject.FindProperty("dragMode");
			caretOnBeginEditProperty = serializedObject.FindProperty("caretOnBeginEdit");
			caretBlinkRateProperty = serializedObject.FindProperty("caretBlinkRate");
			caretWidthProperty = serializedObject.FindProperty("caretWidth");
			caretColorProperty = serializedObject.FindProperty("caretColor");
			selectionColorProperty = serializedObject.FindProperty("selectionColor");
			readOnlyProperty = serializedObject.FindProperty("readOnly");
			scrollBehaviourOnEndEditProperty = serializedObject.FindProperty("scrollBehaviourOnEndEdit");
			scrollBarsVisibilityModeProperty = serializedObject.FindProperty("scrollBarsVisibilityMode");
			scrollSpeedProperty = serializedObject.FindProperty("scrollSpeed");
			fastScrollSensitivityProperty = serializedObject.FindProperty("fastScrollSensitivity");
			resizeMinWidthProperty = serializedObject.FindProperty("resizeMinWidth");
			resizeMinHeightProperty = serializedObject.FindProperty("resizeMinHeight");
			onSelectionChangedProperty = serializedObject.FindProperty("onSelectionChanged");
			onBeginEditProperty = serializedObject.FindProperty("onBeginEdit");
			onEndEditProperty = serializedObject.FindProperty("onEndEdit");
			onValueChangedProperty = serializedObject.FindProperty("onValueChanged");
			onCaretPositionChangedProperty = serializedObject.FindProperty("onCaretPositionChanged");
			onTextSelectionChangedProperty = serializedObject.FindProperty("onTextSelectionChanged");
			onSizeChangedProperty = serializedObject.FindProperty("onSizeChanged");
			actionBarProperty = serializedObject.FindProperty("actionBar");
			actionBarCutProperty = serializedObject.FindProperty("actionBarCut");
			actionBarCopyProperty = serializedObject.FindProperty("actionBarCopy");
			actionBarPasteProperty = serializedObject.FindProperty("actionBarPaste");
			actionBarSelectAllProperty = serializedObject.FindProperty("actionBarSelectAll");
			selectionCursorsProperty = serializedObject.FindProperty("selectionCursors");
			autocapitalizationTypeProperty = serializedObject.FindProperty("autocapitalizationType");
			nextInputFieldProperty = serializedObject.FindProperty("nextInputField");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			AdvancedInputField inputField = (AdvancedInputField)target;

			DrawHeader(inputField);
			InputFieldMode mode = DrawModeProperty(inputField);
			EditorGUILayout.PropertyField(interactableProperty);

			DrawTextProperties(inputField);
			DrawCharacterLimitProperty(inputField);
			//DrawLineLimitProperty(inputField); //TODO: Reenable when line limit works on mobile too (requires partial rewrite native bindings)
			DrawContentTypeProperties(inputField);

			EditorGUILayout.PropertyField(liveProcessingFilterProperty);
			EditorGUILayout.PropertyField(postProcessingFilterProperty);
			EditorGUILayout.PropertyField(selectionModeProperty);
			EditorGUILayout.PropertyField(dragModeProperty);
			DrawCaretProperties();

			EditorGUILayout.PropertyField(selectionColorProperty);
			EditorGUILayout.PropertyField(readOnlyProperty);
			EditorGUILayout.PropertyField(nextInputFieldProperty);

			if(mode == InputFieldMode.SCROLL_TEXT)
			{
				DrawTextScrollProperties(inputField);
			}
			else if(mode == InputFieldMode.HORIZONTAL_RESIZE_FIT_TEXT)
			{
				DrawResizeHorizontalProperties(inputField);
			}
			else if(mode == InputFieldMode.VERTICAL_RESIZE_FIT_TEXT)
			{
				DrawResizeVerticalProperties(inputField);
			}

			DrawEventProperties();
			DrawMobileOnlyProperties();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawHeader(AdvancedInputField inputField)
		{
			if(inputField.GetComponentInChildren<ScrollArea>() != null) //New format
			{
#if ADVANCEDINPUTFIELD_TEXTMESHPRO
				if(inputField.GetComponentInChildren<TMPro.TextMeshProUGUI>() != null)
				{
					Texture2D texture = Resources.Load<Texture2D>(TEXTMESHPRO_FORMAT_IMAGE_PATH);
					GUILayout.Box(texture);
				}
				else
#endif
				{
					Texture2D texture = Resources.Load<Texture2D>(UNITY_FORMAT_IMAGE_PATH);
					GUILayout.Box(texture);
				}
			}
			else //Old format
			{
				Texture2D texture = Resources.Load<Texture2D>(DEPRECATED_FORMAT_RESOURCE_PATH);
				GUILayout.Box(texture);
				GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
				labelStyle.normal.textColor = new Color(0.5f, 0, 0);
				labelStyle.fontSize = 12;
				string message = "This format is deprecated, please use the ConversionTool to convert the InputField(s) to the newer format." +
					"\n(TopBar: Advanced Input Field => ConversionTool)" +
					 "\nSet \'from\' to \'DEPRECATED_ADVANCEDINPUTFIELD\' and \'to\' to either \'ADVANCEDINPUTFIELD_UNITY_TEXT\' or \'ADVANCEDINPUTFIELD_TEXTMESHPRO_TEXT\'";
				GUILayout.Label(message, labelStyle);
				EditorGUILayout.Space();
			}
		}

		private InputFieldMode DrawModeProperty(AdvancedInputField inputField)
		{
			InputFieldMode mode = (InputFieldMode)modeProperty.enumValueIndex;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(modeProperty);
			if(EditorGUI.EndChangeCheck())
			{
				mode = (InputFieldMode)modeProperty.enumValueIndex;
				inputField.Mode = mode;
			}

			return mode;
		}

		private void DrawTextProperties(AdvancedInputField inputField)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(textProperty);
			if(EditorGUI.EndChangeCheck())
			{
				if(string.IsNullOrEmpty(textProperty.stringValue))
				{
					MarkTextRendererDirty(inputField.PlaceholderTextRenderer);
				}
				else
				{
					MarkTextRendererDirty(inputField.TextRenderer);
				}

				inputField.Text = textProperty.stringValue;
			}

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(placeholderTextProperty);
			if(EditorGUI.EndChangeCheck())
			{
				MarkTextRendererDirty(inputField.PlaceholderTextRenderer);
				inputField.PlaceHolderText = placeholderTextProperty.stringValue;
			}
		}

		private void MarkTextRendererDirty(TextRenderer textRenderer)
		{
			if(textRenderer is UnityTextRenderer)
			{
				Undo.RecordObject(((UnityTextRenderer)textRenderer).Renderer, "Undo " + textRenderer.GetInstanceID());
			}
#if ADVANCEDINPUTFIELD_TEXTMESHPRO
			else if(textRenderer is TMProTextRenderer)
			{
				Undo.RecordObject(((TMProTextRenderer)textRenderer).Renderer, "Undo " + textRenderer.GetInstanceID());
			}
#endif
		}

		private void DrawCharacterLimitProperty(AdvancedInputField inputField)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(characterLimitProperty);
			if(EditorGUI.EndChangeCheck())
			{
				inputField.ApplyCharacterLimit(characterLimitProperty.intValue);
				textProperty.stringValue = inputField.Text;
			}
		}

		//private void DrawLineLimitProperty(AdvancedInputField inputField) //TODO: Reenable when line limit works on mobile too (requires partial rewrite native bindings)
		//{
		//	EditorGUI.BeginChangeCheck();
		//	EditorGUILayout.PropertyField(lineLimitProperty);
		//	if(EditorGUI.EndChangeCheck())
		//	{
		//		inputField.ApplyLineLimit(lineLimitProperty.intValue);
		//		textProperty.stringValue = inputField.Text;
		//	}
		//}

		private void DrawContentTypeProperties(AdvancedInputField inputField)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(contentTypeProperty);
			if(EditorGUI.EndChangeCheck())
			{
				ContentType contentType = ((ContentType)contentTypeProperty.enumValueIndex);
				inputField.UpdateContentType(contentType);
				lineTypeProperty.enumValueIndex = (int)inputField.LineType;
				inputTypeProperty.enumValueIndex = (int)inputField.InputType;
				keyboardTypeProperty.enumValueIndex = (int)inputField.KeyboardType;
				characterValidationProperty.enumValueIndex = (int)inputField.CharacterValidation;
				emojisAllowedProperty.boolValue = inputField.EmojisAllowed;
			}

			EditorGUI.indentLevel = 1;
			EditorGUILayout.PropertyField(lineTypeProperty);
			if(((ContentType)contentTypeProperty.enumValueIndex) == ContentType.Custom)
			{
				EditorGUILayout.PropertyField(inputTypeProperty);
				EditorGUILayout.PropertyField(keyboardTypeProperty);
				EditorGUILayout.PropertyField(characterValidationProperty);
				if(((CharacterValidation)characterValidationProperty.enumValueIndex) == CharacterValidation.Custom)
				{
					EditorGUILayout.PropertyField(characterValidatorProperty);
				}

				EditorGUILayout.PropertyField(emojisAllowedProperty, new GUIContent("(Experimental) Emojis Allowed"));
			}
			EditorGUI.indentLevel = 0;
		}

		private void DrawCaretProperties()
		{
			EditorGUILayout.PropertyField(caretOnBeginEditProperty);
			EditorGUILayout.PropertyField(caretBlinkRateProperty);
			EditorGUILayout.PropertyField(caretWidthProperty);
			EditorGUILayout.PropertyField(caretColorProperty);
		}

		private void DrawTextScrollProperties(AdvancedInputField inputField)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Text scroll:", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(scrollBehaviourOnEndEditProperty);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(scrollBarsVisibilityModeProperty);
			if(EditorGUI.EndChangeCheck())
			{
				ScrollArea scrollArea = inputField.TextAreaTransform.GetComponent<ScrollArea>();
				Undo.RecordObject(scrollArea, "Undo " + scrollArea.GetInstanceID());
				inputField.ScrollBarsVisibilityMode = (ScrollbarVisibilityMode)scrollBarsVisibilityModeProperty.enumValueIndex;
			}

			EditorGUILayout.PropertyField(scrollSpeedProperty);
			EditorGUILayout.PropertyField(fastScrollSensitivityProperty);
		}

		private void DrawResizeHorizontalProperties(AdvancedInputField inputField)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Resize Horizontal:", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(resizeMinWidthProperty);
		}

		private void DrawResizeVerticalProperties(AdvancedInputField inputField)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Resize Vertical:", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(resizeMinHeightProperty);
		}

		private void DrawEventProperties()
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Events:", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(onSelectionChangedProperty);
			EditorGUILayout.PropertyField(onBeginEditProperty);
			EditorGUILayout.PropertyField(onEndEditProperty);
			EditorGUILayout.PropertyField(onValueChangedProperty);
			EditorGUILayout.PropertyField(onCaretPositionChangedProperty);
			EditorGUILayout.PropertyField(onTextSelectionChangedProperty);
			EditorGUILayout.PropertyField(onSizeChangedProperty);
		}

		private void DrawMobileOnlyProperties()
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Mobile only:", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(actionBarProperty);
			if(actionBarProperty.boolValue)
			{
				EditorGUILayout.PropertyField(actionBarCutProperty);
				EditorGUILayout.PropertyField(actionBarCopyProperty);
				EditorGUILayout.PropertyField(actionBarPasteProperty);
				EditorGUILayout.PropertyField(actionBarSelectAllProperty);
			}

			EditorGUILayout.PropertyField(selectionCursorsProperty);
			EditorGUILayout.PropertyField(autocapitalizationTypeProperty);
		}
	}
}