//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin
{
	/// <summary>The mode of the InputField. Determines how changes in the text bounds should be handled.</summary>
	public enum InputFieldMode { SCROLL_TEXT, HORIZONTAL_RESIZE_FIT_TEXT, VERTICAL_RESIZE_FIT_TEXT }

	/// <summary>Configuration preset for the content of this InputField</summary>
	public enum ContentType { Standard, Autocorrected, IntegerNumber, DecimalNumber, Alphanumeric, Name, EmailAddress, Password, Pin, Custom, IPAddress, Sentence }

	/// <summary>The type of input</summary>
	public enum InputType { Standard, AutoCorrect, Password }

	/// <summary>The keyboard on mobile to use</summary>
	public enum KeyboardType { Default, ASCIICapable, NumbersAndPunctuation, URL, NumberPad, PhonePad, EmailAddress }

	/// <summary>The validation to use for the text</summary>
	public enum CharacterValidation { None, Integer, Decimal, Alphanumeric, Name, EmailAddress, IPAddress, Sentence, Custom }

	/// <summary>The type of line</summary>
	public enum LineType { SingleLine, MultiLineSubmit, MultiLineNewline }

	/// <summary>The type of autocapitalization</summary>
	public enum AutocapitalizationType { NONE, CHARACTERS, WORDS, SENTENCES }

	/// <summary>Determines which input event to use to select the inputfield</summary>
	public enum SelectionMode { SELECT_ON_RELEASE, SELECT_ON_PRESS }

	/// <summary>Determines how to use drag events</summary>
	public enum DragMode { UPDATE_TEXT_SELECTION, MOVE_TEXT }

	/// <summary>Determines what to use as start of the drag</summary>
	public enum StartDragMode { FROM_CURRENT_POSITION, FROM_SELECTION_START, FROM_SELECTION_END }

	/// <summary>The reason for beginning edit mode</summary>
	public enum BeginEditReason { USER_SELECT, KEYBOARD_NEXT, PROGRAMMATIC_SELECT }

	/// <summary>The reason for ending edit mode</summary>
	public enum EndEditReason { USER_DESELECT, KEYBOARD_CANCEL, KEYBOARD_DONE, KEYBOARD_NEXT, PROGRAMMATIC_DESELECT }

	/// <summary>The caret position in the text when beginning edit mode</summary>
	public enum CaretOnBeginEdit { LOCATION_OF_CLICK, START_OF_TEXT, END_OF_TEXT }

	/// <summary>The scroll behaviour when ending edit mode</summary>
	public enum ScrollBehaviourOnEndEdit { START_OF_TEXT, NO_SCROLL }

	/// <summary>The visibility mode for the scrollbars</summary>
	public enum ScrollbarVisibilityMode { ALWAYS_HIDDEN, ALWAYS_VISIBLE, IN_EDIT_MODE_WHEN_NEEDED, ALWAYS_WHEN_NEEDED }

	/// <summary>The main class of Advanced Input Field</summary>
	[RequireComponent(typeof(RectTransform))]
	public class AdvancedInputField: Selectable, IPointerClickHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IUpdateSelectedHandler
	{
		#region EVENT_CLASSES
		/// <summary>Event used for selection change</summary>
		[Serializable]
		public class SelectionChangedEvent: UnityEvent<bool> { }

		/// <summary>Event used for edit begin</summary>
		[Serializable]
		public class BeginEditEvent: UnityEvent<BeginEditReason> { }

		/// <summary>Event used for edit end</summary>
		[Serializable]
		public class EndEditEvent: UnityEvent<string, EndEditReason> { }

		/// <summary>Event used for text value change</summary>
		[Serializable]
		public class ValueChangedEvent: UnityEvent<string> { }

		/// <summary>Event used for caret position change</summary>
		[Serializable]
		public class CaretPositionChangedEvent: UnityEvent<int> { }

		/// <summary>Event used for selection change</summary>
		[Serializable]
		public class TextSelectionChangedEvent: UnityEvent<int, int> { }

		/// <summary>Event used for size change</summary>
		[Serializable]
		public class SizeChangedEvent: UnityEvent<Vector2> { }
		#endregion

		#region SERIALIZED_FIELDS
		[Tooltip("The mode of the InputField. Determines how changes in the text bounds should be handled.")]
		[SerializeField]
		private InputFieldMode mode;

		[Tooltip("The main text renderer")]
		[SerializeField]
		private TextRenderer textRenderer;

		[Tooltip("The text renderer used for text that has been processed")]
		[SerializeField]
		private TextRenderer processedTextRenderer;

		[Tooltip("The text renderer used as placeholder")]
		[SerializeField]
		private TextRenderer placeholderTextRenderer;

		[Tooltip("The caret renderer")]
		[SerializeField]
		private Image caretRenderer;

		[Tooltip("The renderer for text selection")]
		[SerializeField]
		private TextSelectionRenderer selectionRenderer;

		[Tooltip("The main text string")]
		[SerializeField]
		private string text;

		[Tooltip("The placeholder text string")]
		[SerializeField]
		private string placeholderText;

		[Tooltip("The maximum amount of characters allowed, zero means infinite")]
		[SerializeField]
		private int characterLimit;

		//[Tooltip("The maximum amount of lines allowed, zero means infinite")]
		//[SerializeField]
		//private int lineLimit; //TODO: Reenable when line limit works on mobile too (requires partial rewrite native bindings)

		[Tooltip("Configuration preset for the content of this InputField")]
		[SerializeField]
		[EnumOrder(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 9 })] //Custom should be displayed last
		private ContentType contentType;

		[Tooltip("The type of line")]
		[SerializeField]
		private LineType lineType;

		[Tooltip("The type of input")]
		[SerializeField]
		private InputType inputType;

		[Tooltip("The keyboard on mobile to use")]
		[SerializeField]
		private KeyboardType keyboardType;

		[Tooltip("The validation to use for the text")]
		[SerializeField]
		private CharacterValidation characterValidation;

		[Tooltip("The custom character validator to use for the text")]
		[SerializeField]
		private CharacterValidator characterValidator;

		[Tooltip("(Experimental) Indicates whether emojis should be allowed, requires TextMeshPro Text Renderers")]
		[SerializeField]
		private bool emojisAllowed;

		[Tooltip("The filter (if any) to use whenever text or caret position changes")]
		[SerializeField]
		private LiveProcessingFilter liveProcessingFilter;

		[Tooltip("The filter (if any) to use when input field has been deselected")]
		[SerializeField]
		private PostProcessingFilter postProcessingFilter;

		[Tooltip("Determines which input event to use to select the inputfield")]
		[SerializeField]
		private SelectionMode selectionMode;

		[Tooltip("Determines how to use drag events")]
		[SerializeField]
		private DragMode dragMode;

		[Tooltip("Indicates where to position the caret when beginning edit mode")]
		[SerializeField]
		private CaretOnBeginEdit caretOnBeginEdit;

		[Tooltip("The blink rate of the caret")]
		[SerializeField]
		[Range(0.1f, 4f)]
		private float caretBlinkRate = 0.85f;

		[Tooltip("The width of the caret")]
		[SerializeField]
		[Range(0.01f, 10)]
		private float caretWidth = 2;

		[Tooltip("The color of the caret")]
		[SerializeField]
		private Color caretColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

		[Tooltip("The color of the text selection")]
		[SerializeField]
		private Color selectionColor = new Color(168f / 255f, 206f / 255f, 255f / 255f, 192f / 255f);

		[Tooltip("Indicates if this input field is read only")]
		[SerializeField]
		private bool readOnly = false;

		[Tooltip("The scroll behaviour when ending edit mode")]
		[SerializeField]
		public ScrollBehaviourOnEndEdit scrollBehaviourOnEndEdit;

		[Tooltip("The visibility mode for the scrollbars")]
		[SerializeField]
		public ScrollbarVisibilityMode scrollBarsVisibilityMode;

		[Tooltip("The speed to scroll text (Canvas pixels per second)")]
		[SerializeField]
		private float scrollSpeed = 250;

		[Tooltip("The additional scroll sensitivity when dragging out of bounds")]
		[SerializeField]
		private float fastScrollSensitivity = 5;

		[Tooltip("The minimal width of the InputField")]
		[SerializeField]
		private float resizeMinWidth = 0;

		[Tooltip("The minimal height of the InputField")]
		[SerializeField]
		private float resizeMinHeight = 0;

		[Tooltip("Event used for selection change")]
		[SerializeField]
		private SelectionChangedEvent onSelectionChanged = new SelectionChangedEvent();

		[Tooltip("Event used for edit begin")]
		[SerializeField]
		private BeginEditEvent onBeginEdit = new BeginEditEvent();

		[Tooltip("Event used for edit end")]
		[SerializeField]
		private EndEditEvent onEndEdit = new EndEditEvent();

		[Tooltip("Event used for text value change")]
		[SerializeField]
		private ValueChangedEvent onValueChanged = new ValueChangedEvent();

		[Tooltip("Event used for caret position change")]
		[SerializeField]
		private CaretPositionChangedEvent onCaretPositionChanged = new CaretPositionChangedEvent();

		[Tooltip("Event used for text selection change")]
		[SerializeField]
		private TextSelectionChangedEvent onTextSelectionChanged = new TextSelectionChangedEvent();

		[Tooltip("Event used for size change")]
		[SerializeField]
		private SizeChangedEvent onSizeChanged = new SizeChangedEvent();

		[Tooltip("Indicates if the ActionBar should be used on mobile")]
		[SerializeField]
		private bool actionBar = false;

		[Tooltip("Indicates if the cut option should be enabled in the ActionBar")]
		[SerializeField]
		private bool actionBarCut = true;

		[Tooltip("Indicates if the copy option should be enabled in the ActionBar")]
		[SerializeField]
		private bool actionBarCopy = true;

		[Tooltip("Indicates if the paste option should be enabled in the ActionBar")]
		[SerializeField]
		private bool actionBarPaste = true;

		[Tooltip("Indicates if the select all option should be enabled in the ActionBar")]
		[SerializeField]
		private bool actionBarSelectAll = true;

		[Tooltip("Indicates if the Selection Cursors (handles for selection start and end) should be used on mobile")]
		[SerializeField]
		private bool selectionCursors = false;

		[Tooltip("The type of autocapitalization")]
		[SerializeField]
		private AutocapitalizationType autocapitalizationType;

		[Tooltip("The next input field (if any) to switch to when pressing the done button on the TouchScreenKeyboard")]
		[SerializeField]
		private AdvancedInputField nextInputField;
		#endregion

		#region FIELDS
		private RectTransform rectTransform;

		/// <summary>The text value for processed text (or the placeholder when text is empty)</summary>
		private string processedText;

		/// <summary>The transform containing the TextArea</summary>
		private RectTransform textAreaTransform;

		/// <summary>The scrollable ContentTransform</summary>
		private RectTransform textContentTransform;

		/// <summary>The TextInputHandler for current platform</summary>
		private TextInputHandler textInputHandler;

		/// <summary>The TextNavigator for current platform</summary>
		private TextNavigator textNavigator;

		/// <summary>The TextManipulator for current platform</summary>
		private TextManipulator textManipulator;

		/// <summary>The press position</summary>
		private Vector2 pressPosition;

		/// <summary>The text content position when press started</summary>
		private Vector2 pressTextContentPosition;

		/// <summary>The start position on the drag (as character index)</summary>
		private int dragStartPosition;

		/// <summary>Indicates if drag position is out of bounds</summary>
		private bool dragOutOfBounds;

		/// <summary>Indicates if the drag state should keep updating (for drag out of bounds)</summary>
		private bool updateDrag;

		/// <summary>Indicates if input field is currently in edit mode</summary>
		private bool editMode;

		/// <summary>The reason for beginning edit mode</summary>
		private BeginEditReason beginEditReason;

		/// <summary>The reason for ending edit mode</summary>
		private EndEditReason endEditReason;

		/// <summary>The Canvas</summary>
		private Canvas canvas;

		/// <summary>The last known canvas scale factor</summary>
		private float lastCanvasScaleFactor;

		/// <summary>Indicates if input field is selected</summary>
		private bool selected;

		/// <summary>The current drag mode</summary>
		private StartDragMode currentStartDragMode;

		/// <summary>The offset to use when determining drag position</summary>
		private Vector2 dragOffset;

		/// <summary>Indicates if this input field is initialized</summary>
		private bool initialized;
		#endregion

		#region PROPERTIES
		public RectTransform RectTransform
		{
			get
			{
				if(rectTransform == null)
				{
					rectTransform = GetComponent<RectTransform>();
				}

				return rectTransform;
			}
		}

		/// <summary>The size of the InputField RectTransform</summary>
		public Vector2 Size
		{
			get { return RectTransform.rect.size; }
			set
			{
				Vector2 size = RectTransform.rect.size;
				Vector2 sizeDifference = value - size;
				if(sizeDifference != Vector2.zero)
				{
					Vector2 sizeDelta = RectTransform.sizeDelta;
					sizeDelta += sizeDifference;
					RectTransform.sizeDelta = sizeDelta;

					if(onSizeChanged != null)
					{
						onSizeChanged.Invoke(value);
					}
				}
			}
		}

		/// <summary>The mode of the InputField. Determines how changes in the text bounds should be handled.</summary>
		public InputFieldMode Mode
		{
			get { return mode; }
			set
			{
				mode = value;

				ScrollArea scrollArea = TextAreaTransform.GetComponent<ScrollArea>();
				switch(mode)
				{
					case InputFieldMode.SCROLL_TEXT: scrollArea.enabled = true; break;
					case InputFieldMode.HORIZONTAL_RESIZE_FIT_TEXT: scrollArea.enabled = false; break;
					case InputFieldMode.VERTICAL_RESIZE_FIT_TEXT: scrollArea.enabled = false; break;
				}
			}
		}

		/// <summary>The transform containing the TextArea</summary>
		public RectTransform TextAreaTransform
		{
			get
			{
				if(textAreaTransform == null)
				{
					textAreaTransform = transform.Find("TextArea").GetComponent<RectTransform>();
				}
				return textAreaTransform;
			}
		}

		/// <summary>The scrollable ContentTransform</summary>
		public RectTransform TextContentTransform
		{
			get
			{
				if(textContentTransform == null)
				{
					textContentTransform = transform.Find("TextArea/Content").GetComponent<RectTransform>();
				}
				return textContentTransform;
			}
		}

		/// <summary>The main text renderer</summary>
		public TextRenderer TextRenderer
		{
			get
			{
				if(textRenderer == null)
				{
					textRenderer = transform.Find("TextArea/Content/Text").GetComponent<TextRenderer>();
				}
				return textRenderer;
			}
		}

		/// <summary>The text renderer for processed text</summary>
		public TextRenderer ProcessedTextRenderer
		{
			get
			{
				if(processedTextRenderer == null)
				{
					processedTextRenderer = transform.Find("TextArea/Content/Processed").GetComponent<TextRenderer>();
				}
				return processedTextRenderer;
			}
		}

		/// <summary>The text renderer used as placeholder</summary>
		public TextRenderer PlaceholderTextRenderer
		{
			get
			{
				if(placeholderTextRenderer == null)
				{
					placeholderTextRenderer = transform.Find("TextArea/Content/Placeholder").GetComponent<TextRenderer>();
				}
				return placeholderTextRenderer;
			}
		}

		/// <summary>The caret renderer</summary>
		public Image CaretRenderer
		{
			get
			{
				if(caretRenderer == null)
				{
					caretRenderer = transform.Find("TextArea/Content/Caret").GetComponent<Image>();
				}
				return caretRenderer;
			}
		}

		/// <summary>The renderer for text selection</summary>
		public TextSelectionRenderer SelectionRenderer
		{
			get
			{
				if(selectionRenderer == null)
				{
					selectionRenderer = transform.Find("TextArea/Content/Selection").GetComponent<TextSelectionRenderer>();
				}
				return selectionRenderer;
			}
		}

		/// <summary>The maximum amount of characters allowed, zero means infinite</summary>
		public int CharacterLimit
		{
			get { return characterLimit; }
			set { characterLimit = value; }
		}

		/// <summary>The maximum amount of lines allowed, zero means infinite</summary>
		//public int LineLimit //TODO: Reenable when line limit works on mobile too (requires partial rewrite native bindings)
		//{
		//	get { return lineLimit; }
		//	set { lineLimit = value; }
		//}

		/// <summary>Configuration preset for the content of this InputField</summary>
		public ContentType ContentType
		{
			get { return contentType; }
			set { contentType = value; }
		}

		/// <summary>The type of line</summary>
		public LineType LineType
		{
			get { return lineType; }
			set { lineType = value; }
		}

		/// <summary>The type of autocapitalization</summary>
		public AutocapitalizationType AutocapitalizationType
		{
			get { return autocapitalizationType; }
			set { autocapitalizationType = value; }
		}

		/// <summary>The type of input</summary>
		public InputType InputType
		{
			get { return inputType; }
			set { inputType = value; }
		}

		/// <summary>The keyboard on mobile to use</summary>
		public KeyboardType KeyboardType
		{
			get { return keyboardType; }
			set { keyboardType = value; }
		}

		/// <summary>The validation to use for the text</summary>
		public CharacterValidation CharacterValidation
		{
			get { return characterValidation; }
			set { characterValidation = value; }
		}

		/// <summary>The custom character validator to use for the text</summary>
		public CharacterValidator CharacterValidator
		{
			get { return characterValidator; }
			set { characterValidator = value; }
		}

		/// <summary>Indicates whether emojis should be allowed</summary>
		public bool EmojisAllowed
		{
			get { return emojisAllowed; }
			set { emojisAllowed = value; }
		}

		/// <summary>The filter (if any) to use whenever text or caret position changes</summary>
		public LiveProcessingFilter LiveProcessingFilter
		{
			get { return liveProcessingFilter; }
			set { liveProcessingFilter = value; }
		}

		/// <summary>The filter (if any) to use when input field has been deselected</summary>
		public PostProcessingFilter PostProcessingFilter
		{
			get { return postProcessingFilter; }
			set { postProcessingFilter = value; }
		}

		/// <summary>Determines which input event to use to select the inputfield</summary>
		public SelectionMode SelectionMode
		{
			get { return selectionMode; }
			set { selectionMode = value; }
		}

		/// <summary>Determines how to use drag events</summary>
		public DragMode DragMode
		{
			get { return dragMode; }
			set { dragMode = value; }
		}

		/// <summary>Indicates where to position the caret when beginning edit mode</summary>
		public CaretOnBeginEdit CaretOnBeginEdit
		{
			get { return caretOnBeginEdit; }
			set { caretOnBeginEdit = value; }
		}

		/// <summary>The blink rate of the caret</summary>
		public float CaretBlinkRate
		{
			get { return caretBlinkRate; }
			set { caretBlinkRate = value; }
		}

		/// <summary>The width of the caret</summary>
		public float CaretWidth
		{
			get { return caretWidth; }
			set { caretWidth = value; }
		}

		/// <summary>The color of the caret</summary>
		public Color CaretColor
		{
			get { return caretColor; }
			set { caretColor = value; }
		}

		/// <summary>The color of the text selection</summary>
		public Color SelectionColor
		{
			get { return selectionColor; }
			set { selectionColor = value; }
		}

		/// <summary>The scroll behaviour when ending edit mode</summary>
		public ScrollBehaviourOnEndEdit ScrollBehaviourOnEndEdit
		{
			get { return scrollBehaviourOnEndEdit; }
			set { scrollBehaviourOnEndEdit = value; }
		}

		/// <summary>The speed to scroll text (Canvas pixels per second)</summary>
		public float ScrollSpeed
		{
			get { return scrollSpeed; }
			set { scrollSpeed = value; }
		}

		/// <summary>The additional scroll sensitivity when dragging out of bounds</summary>
		public float FastScrollSensitivity
		{
			get { return fastScrollSensitivity; }
			set { fastScrollSensitivity = value; }
		}

		/// <summary>The minimal width of the InputField</summary>
		public float ResizeMinWidth
		{
			get { return resizeMinWidth; }
			set { resizeMinWidth = value; }
		}

		/// <summary>The minimal height of the InputField</summary>
		public float ResizeMinHeight
		{
			get { return resizeMinHeight; }
			set { resizeMinHeight = value; }
		}

		/// <summary>Event used for selection change</summary>
		public SelectionChangedEvent OnSelectionChanged
		{
			get { return onSelectionChanged; }
			set { onSelectionChanged = value; }
		}

		/// <summary>Event used for begin edit</summary>
		public BeginEditEvent OnBeginEdit
		{
			get { return onBeginEdit; }
			set { onBeginEdit = value; }
		}

		/// <summary>Event used for end edit</summary>
		public EndEditEvent OnEndEdit
		{
			get { return onEndEdit; }
			set { onEndEdit = value; }
		}

		/// <summary>Event used for value change</summary>
		public ValueChangedEvent OnValueChanged
		{
			get { return onValueChanged; }
			set { onValueChanged = value; }
		}

		/// <summary>Event used for value change</summary>
		public CaretPositionChangedEvent OnCaretPositionChanged
		{
			get { return onCaretPositionChanged; }
			set { onCaretPositionChanged = value; }
		}

		/// <summary>Event used for text selection change</summary>
		public TextSelectionChangedEvent OnTextSelectionChanged
		{
			get { return onTextSelectionChanged; }
			set { onTextSelectionChanged = value; }
		}

		/// <summary>Indicates if the ActionBar should be used on mobile</summary>
		public bool ActionBarEnabled
		{
			get { return actionBar; }
			set { actionBar = value; }
		}

		/// <summary>Indicates if the cut option should be enabled in the ActionBar</summary>
		public bool ActionBarCut
		{
			get { return actionBarCut; }
			set { actionBarCut = value; }
		}

		/// <summary>Indicates if the copy option should be enabled in the ActionBar</summary>
		public bool ActionBarCopy
		{
			get { return actionBarCopy; }
			set { actionBarCopy = value; }
		}

		/// <summary>Indicates if the paste option should be enabled in the ActionBar</summary>
		public bool ActionBarPaste
		{
			get { return actionBarPaste; }
			set { actionBarPaste = value; }
		}

		/// <summary>Indicates if the select all option should be enabled in the ActionBar</summary>
		public bool ActionBarSelectAll
		{
			get { return actionBarSelectAll; }
			set { actionBarSelectAll = value; }
		}

		/// <summary>Indicates if the Selection Cursors (handles for selection start and end) should be used on mobile</summary>
		public bool SelectionCursorsEnabled
		{
			get { return selectionCursors; }
			set { selectionCursors = value; }
		}

		/// <summary>The next input field (if any) to switch to when pressing the done button on the TouchScreenKeyboard</summary>
		public AdvancedInputField NextInputField
		{
			get { return nextInputField; }
			set { nextInputField = value; }
		}

		/// <summary>Indicates if autocorrection should be used</summary>
		public bool AutoCorrection { get { return inputType == InputType.AutoCorrect; } }

		/// <summary>Indicates is input should be secure</summary>
		public bool Secure { get { return inputType == InputType.Password; } }

		/// <summary>Indicates if line type is multiline</summary>
		public bool Multiline { get { return lineType != LineType.SingleLine; } }

		/// <summary>Indicates if this input field is initialized</summary>
		public bool Initialized { get { return initialized; } }

		/// <summary>Indicates whether next deselect event should be blocked</summary>
		public bool ShouldBlockDeselect { get; set; }

        /// <summary>Indicates whether all select and deselect events should be blocked</summary>
        public bool ShouldBlockAllSelections { get; set; }
        
        /// <summary>The visibility mode for the scrollbars</summary>
        public ScrollbarVisibilityMode ScrollBarsVisibilityMode
		{
			get { return scrollBarsVisibilityMode; }
			set
			{
				scrollBarsVisibilityMode = value;

				ScrollArea scrollArea = TextAreaTransform.GetComponent<ScrollArea>();
				scrollArea.HorizontalScrollbarVisibility = value;
				scrollArea.VerticalScrollbarVisibility = value;
			}
		}

		/// <summary>The Canvas</summary>
		internal Canvas Canvas
		{
			get
			{
				if(canvas == null)
				{
					canvas = GetComponentInParent<Canvas>();
					if(canvas != null && textInputHandler != null && textNavigator != null)
					{
						textInputHandler.OnCanvasScaleChanged(canvas.scaleFactor);
						textNavigator.OnCanvasScaleChanged(canvas.scaleFactor);

						lastCanvasScaleFactor = canvas.scaleFactor;
					}
				}

				return canvas;
			}
		}

		/// <summary>The current start drag mode</summary>
		internal StartDragMode CurrentStartDragMode
		{
			get { return currentStartDragMode; }
			set { currentStartDragMode = value; }
		}

		/// <summary>The offset to use when determining drag position</summary>
		internal Vector2 DragOffset
		{
			get { return dragOffset; }
			set { dragOffset = value; }
		}

		/// <summary>Indicates if the user is currently pressing this input field</summary>
		internal bool UserPressing { get; private set; }

		/// <summary>Indicates if this input field is read only</summary>
		public bool ReadOnly
		{
			get { return readOnly; }
			set
			{
				readOnly = value;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
				if(readOnly && Selected && NativeKeyboardManager.Keyboard != null)
				{
					NativeKeyboardManager.HideKeyboard();
				}
#endif
			}
		}

		/// <summary>The main text string</summary>
		public string Text
		{
			get { return text; }
			set { SetText(value); }
		}

		/// <summary>The caret position</summary>
		public int CaretPosition
		{
			get
			{
				return GetCaretPosition();
			}
			set
			{
				SetCaretPosition(value);
			}
		}

		/// <summary>The text selection start position</summary>
		public int TextSelectionStartPosition
		{
			get
			{
				return GetTextSelectionStartPosition();
			}
			set
			{
				SetTextSelectionStartPosition(value);
			}
		}

		/// <summary>The text selection end position</summary>
		public int TextSelectionEndPosition
		{
			get
			{
				return GetTextSelectionEndPosition();
			}
			set
			{
				SetTextSelectionEndPosition(value);
			}
		}

		/// <summary>The text value for processed text</summary>
		internal string ProcessedText
		{
			get { return processedText; }
			set
			{
				processedText = value;
				processedTextRenderer.Text = processedText;
				textNavigator.RefreshRenderedText();
				UpdateActiveTextRenderer();
			}
		}

		/// <summary>The placeholder text string</summary>
		public string PlaceHolderText
		{
			get { return placeholderText; }
			set
			{
				placeholderText = value;
				PlaceholderTextRenderer.Text = placeholderText;
				UpdateActiveTextRenderer();
			}
		}

		/// <summary>The selected text string</summary>
		public string SelectedText
		{
			get
			{
				if(initialized)
				{
					return textNavigator.SelectedText;
				}

				return string.Empty;
			}
		}

		/// <summary>The text that is actually rendered</summary>
		public string RenderedText
		{
			get
			{
				if(LiveProcessing)
				{
					return ProcessedTextRenderer.Text;
				}
				else
				{
					return TextRenderer.Text;
				}
			}
			set
			{
				if(LiveProcessing)
				{
					if(ProcessedTextRenderer.Text != value)
					{
						ProcessedTextRenderer.Text = value;
						ProcessedTextRenderer.UpdateImmediately();
						textNavigator.UpdateSelection();
					}
				}
				else
				{
					if(TextRenderer.Text != value)
					{
						TextRenderer.Text = value;
						TextRenderer.UpdateImmediately();
						textNavigator.UpdateSelection();
					}
				}
			}
		}

		/// <summary>Indicates if LiveProcessing is active</summary>
		public bool LiveProcessing
		{
			get { return liveProcessingFilter != null; }
		}

		/// <summary>Indicates if PostProcessing is active</summary>
		public bool PostProcessing
		{
			get { return postProcessingFilter != null; }
		}

		/// <summary>Indicates if Enter key should submit</summary>
		public bool ShouldSubmit
		{
			get { return (lineType != LineType.MultiLineNewline); }
		}

		/// <summary>Indicates if input field is a password field</summary>
		public bool IsPasswordField
		{
			get { return inputType == InputType.Password; }
		}

		/// <summary>Indicates if input field is selected</summary>
		public bool Selected
		{
			get
			{
				return selected;
			}
			private set
			{
				if(selected != value)
				{
					selected = value;
					if(onSelectionChanged != null)
					{
						onSelectionChanged.Invoke(selected);
					}

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
					if(selected && ActionBarEnabled && textInputHandler is MobileTextInputHandler)
					{
						((MobileTextInputHandler)textInputHandler).InitActionBar(this, textRenderer);
					}

					NativeKeyboardManager.UpdateKeyboardActiveState();
#endif
				}
			}
		}
		#endregion

		#region UNITY_METHODS
		protected override void Awake()
		{
			base.Awake();

			if(UnityEngine.Application.isPlaying)
			{
#if !UNITY_EDITOR && UNITY_WSA
				if(UWPThreadHelper.Instance == null)
				{
					UWPThreadHelper.CreateInstance(); //Has to be created on Unity thread
				}
#endif
				InitializeInputField();
			}

			currentStartDragMode = StartDragMode.FROM_CURRENT_POSITION;
		}

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
		protected override void Start()
		{
			base.Start();
			NativeKeyboardManager.AddHardwareKeyboardChangedListener(OnHardwareKeyboardChanged);
		}
#endif

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if(textInputHandler != null)
			{
				textInputHandler.OnDestroy();
			}
			if(textNavigator != null)
			{
				textNavigator.OnDestroy();
			}
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();

			if(Selected && Canvas != null && lastCanvasScaleFactor != Canvas.scaleFactor)
			{
				textInputHandler.OnCanvasScaleChanged(Canvas.scaleFactor);
				textNavigator.OnCanvasScaleChanged(Canvas.scaleFactor);

				lastCanvasScaleFactor = Canvas.scaleFactor;
			}

			if(Initialized)
			{
				UpdateActiveTextRenderer();
			}
		}
		#endregion

		#region PUBLIC_METHODS
		/// <summary>Gets the text of this input field</summary>
		/// <returns>The current text value of this input field</returns>
		public string GetText()
		{
			return text;
		}

		/// <summary>Sets the text of this input field</summary>
		/// <param name="text">The text value</param>
		/// <param name="invokeTextChangeEvent">Indicates whether this method should invoke the Text Change event</param>
		public void SetText(string text, bool invokeTextChangeEvent = false)
		{
			if(this.text != text)
			{
				this.text = text;
				TextRenderer.Text = text;
				UpdateActiveTextRenderer();

				if(!initialized)
				{
					return;
				}
				textNavigator.RefreshRenderedText();
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID || UNITY_WSA)
				if(Selected && !ShouldUseHardwareKeyboard())
				{
					((MobileTextManipulator)textManipulator).UpdateNativeText(text);
				}
#endif

				if(invokeTextChangeEvent && onValueChanged != null)
				{
					StartCoroutine(DelayedValueChanged());
				}

				if(LiveProcessing)
				{
					string processedText = liveProcessingFilter.ProcessText(text, textNavigator.CaretPosition);
					if(processedText != null)
					{
						ProcessedText = processedText;

						if(Selected)
						{
							int caretPosition = textNavigator.CaretPosition;
							int processedCaretPosition = liveProcessingFilter.DetermineProcessedCaret(text, caretPosition, processedText);
							textNavigator.ProcessedCaretPosition = processedCaretPosition;
						}
					}
				}
				else
				{
					if(Selected)
					{
						if(textNavigator.CaretPosition > text.Length)
						{
							textNavigator.SetCaretPosition(text.Length, true);
						}
					}
				}
			}
		}

		/// <summary>Gets current caret position</summary>
		/// <returns>The caret position</returns>
		public int GetCaretPosition()
		{
			if(initialized)
			{
				return textNavigator.CaretPosition;
			}
			else
			{
				Debug.LogWarning("Couldn't get caret position, the input field is not initialized yet");
			}

			return -1;
		}

		/// <summary>Sets the caret position</summary>
		/// <param name="caretPosition">The caret position value</param>
		/// <param name="invokeTextSelectionChangeEvent">Indicates whether this method should invoke the Caret Position Change event</param>
		public void SetCaretPosition(int caretPosition, bool invokeCaretPositonChangeEvent = false)
		{
			if(initialized && text != null)
			{
				caretPosition = Mathf.Clamp(caretPosition, 0, text.Length);
				textNavigator.SetCaretPosition(caretPosition, invokeCaretPositonChangeEvent);
			}
			else
			{
				Debug.LogWarning("Couldn't set caret position, the input field is not initialized yet");
			}
		}

		/// <summary>Gets current text selection start position</summary>
		/// <returns>The selection start position</returns>
		public int GetTextSelectionStartPosition()
		{
			if(initialized)
			{
				return textNavigator.SelectionStartPosition;
			}
			else
			{
				Debug.LogWarning("Couldn't get text selection start position, the input field is not initialized yet");
			}

			return -1;
		}

		/// <summary>Sets the text selection start position</summary>
		/// <param name="selectionEndPosition">The selection start position value</param>
		/// <param name="invokeSelectionChangeEvent">Indicates whether this method should invoke the Text Selection Change event</param>
		public void SetTextSelectionStartPosition(int selectionStartPosition, bool invokeTextSelectionChangeEvent = false)
		{
			if(initialized && text != null)
			{
				selectionStartPosition = Mathf.Clamp(selectionStartPosition, 0, text.Length);
				textNavigator.SetSelectionStartPosition(selectionStartPosition, invokeTextSelectionChangeEvent);
			}
			else
			{
				Debug.LogWarning("Couldn't set text selection start position, the input field is not initialized yet");
			}
		}

		/// <summary>Gets current text selection end position</summary>
		/// <returns>The selection end position</returns>
		public int GetTextSelectionEndPosition()
		{
			if(initialized)
			{
				return textNavigator.SelectionEndPosition;
			}
			else
			{
				Debug.LogWarning("Couldn't get text selection end position, the input field is not initialized yet");
			}

			return -1;
		}

		/// <summary>Sets the text selection end position</summary>
		/// <param name="selectionEndPosition">The selection end position value</param>
		/// <param name="invokeSelectionChangeEvent">Indicates whether this method should invoke the Text Selection Change event</param>
		public void SetTextSelectionEndPosition(int selectionEndPosition, bool invokeTextSelectionChangeEvent = false)
		{
			if(initialized && text != null)
			{
				selectionEndPosition = Mathf.Clamp(selectionEndPosition, 0, text.Length);
				textNavigator.SetSelectionEndPosition(selectionEndPosition, invokeTextSelectionChangeEvent);
			}
			else
			{
				Debug.LogWarning("Couldn't set text selection end position, the input field is not initialized yet");
			}
		}

		/// <summary>Sets the text selection</summary>
		/// <param name="selectionStartPosition">The selection start position value</param>
		/// <param name="selectionEndPosition">The selection end position value</param>
		/// <param name="invokeSelectionChangeEvent">Indicates whether this method should invoke the Text Selection Change event</param>
		public void SetTextSelection(int selectionStartPosition, int selectionEndPosition, bool invokeTextSelectionChangeEvent = false)
		{
			if(initialized && text != null)
			{
				selectionStartPosition = Mathf.Clamp(selectionStartPosition, 0, text.Length);
				selectionEndPosition = Mathf.Clamp(selectionEndPosition, 0, text.Length);
				textNavigator.SetSelectionStartPosition(selectionStartPosition, false);
				textNavigator.SetSelectionEndPosition(selectionEndPosition, invokeTextSelectionChangeEvent);
			}
			else
			{
				Debug.LogWarning("Couldn't set text selection, the input field is not initialized yet");
			}
		}

		/// <summary>Sets the caret position to the start of text</summary>
		/// <param name="invokeCaretPositonChangeEvent">Indicates whether this method should invoke the Caret Position Change event</param>
		public void SetCaretToTextStart(bool invokeCaretPositonChangeEvent = false)
		{
			if(text != null)
			{
				SetCaretPosition(0, invokeCaretPositonChangeEvent);
			}
			else
			{
				Debug.LogWarning("Couldn't set caret to text start, text is null");
			}
		}

		/// <summary>Sets the caret position to the end of text</summary>
		/// <param name="invokeCaretPositonChangeEvent">Indicates whether this method should invoke the Caret Position Change event</param>
		public void SetCaretToTextEnd(bool invokeCaretPositonChangeEvent = false)
		{
			if(text != null)
			{
				SetCaretPosition(text.Length, invokeCaretPositonChangeEvent);
			}
			else
			{
				Debug.LogWarning("Couldn't set caret to text end, text is null");
			}
		}

		/// <summary>Applies the character limit change</summary>
		/// <param name="characterLimit">The new character limit value</param>
		public void ApplyCharacterLimit(int characterLimit)
		{
			if(characterLimit > 0 && text.Length > characterLimit)
			{
				text = text.Substring(0, characterLimit);
				TextRenderer.Text = text;
			}
		}

		/// <summary>Applies the line limit change</summary>
		/// <param name="lineLimit">The new line limit value</param>
		//public void ApplyLineLimit(int lineLimit) //TODO: Reenable when line limit works on mobile too (requires partial rewrite native bindings)
		//{
		//	if(lineLimit > 0 && text.Length > 0)
		//	{
		//		TextRenderer.Text = text;
		//		while(TextRenderer.LineCount > lineLimit && text.Length > 0)
		//		{
		//			text = text.Substring(0, text.Length - 1);
		//			TextRenderer.Text = text;
		//		}
		//	}
		//}

		/// <summary>Applies the content type change</summary>
		/// <param name="contentType">The new content type value</param>
		public void UpdateContentType(ContentType contentType)
		{
			switch(contentType)
			{
				case ContentType.Standard:
					{
						// Don't enforce line type for this content type.
						inputType = InputType.Standard;
						keyboardType = KeyboardType.Default;
						characterValidation = CharacterValidation.None;
						emojisAllowed = false;
						break;
					}
				case ContentType.Autocorrected:
					{
						// Don't enforce line type for this content type.
						inputType = InputType.AutoCorrect;
						keyboardType = KeyboardType.Default;
						characterValidation = CharacterValidation.None;
						emojisAllowed = false;
						break;
					}
				case ContentType.IntegerNumber:
					{
						lineType = LineType.SingleLine;
						inputType = InputType.Standard;
						keyboardType = KeyboardType.NumberPad;
						characterValidation = CharacterValidation.Integer;
						emojisAllowed = false;
						break;
					}
				case ContentType.DecimalNumber:
					{
						lineType = LineType.SingleLine;
						inputType = InputType.Standard;
						keyboardType = KeyboardType.NumbersAndPunctuation;
						characterValidation = CharacterValidation.Decimal;
						emojisAllowed = false;
						break;
					}
				case ContentType.Alphanumeric:
					{
						lineType = LineType.SingleLine;
						inputType = InputType.Standard;
						keyboardType = KeyboardType.ASCIICapable;
						characterValidation = CharacterValidation.Alphanumeric;
						emojisAllowed = false;
						break;
					}
				case ContentType.Name:
					{
						lineType = LineType.SingleLine;
						inputType = InputType.Standard;
						keyboardType = KeyboardType.Default;
						characterValidation = CharacterValidation.Name;
						emojisAllowed = false;
						break;
					}
				case ContentType.EmailAddress:
					{
						lineType = LineType.SingleLine;
						inputType = InputType.Standard;
						keyboardType = KeyboardType.EmailAddress;
						characterValidation = CharacterValidation.EmailAddress;
						emojisAllowed = false;
						break;
					}
				case ContentType.Password:
					{
						lineType = LineType.SingleLine;
						inputType = InputType.Password;
						keyboardType = KeyboardType.Default;
						characterValidation = CharacterValidation.None;
						emojisAllowed = false;
						break;
					}
				case ContentType.Pin:
					{
						lineType = LineType.SingleLine;
						inputType = InputType.Password;
						keyboardType = KeyboardType.NumberPad;
						characterValidation = CharacterValidation.Integer;
						emojisAllowed = false;
						break;
					}
				case ContentType.IPAddress:
					{
						lineType = LineType.SingleLine;
						inputType = InputType.Standard;
						keyboardType = KeyboardType.PhonePad;
						characterValidation = CharacterValidation.IPAddress;
						emojisAllowed = false;
						break;
					}
				case ContentType.Sentence:
					{
						lineType = LineType.MultiLineNewline;
						inputType = InputType.Standard;
						keyboardType = KeyboardType.Default;
						characterValidation = CharacterValidation.Sentence;
						emojisAllowed = false;
						break;
					}
				default:
					{
						// Includes Custom type. Nothing should be enforced.
						break;
					}
			}
		}

		/// <summary>Clears the InputField</summary>
		public void Clear()
		{
			Text = string.Empty;
		}

		/// <summary>Manually selects this inputfield (call this to select this inputfield programmatically)</summary>
		public void ManualSelect(BeginEditReason beginEditReason = BeginEditReason.PROGRAMMATIC_SELECT)
		{
			if(!interactable)
			{
				Debug.LogWarningFormat("InputField is not interactable, it won't be selected");
			}
			else if(initialized)
			{
				this.beginEditReason = beginEditReason;
				Reselect();
				EnableSelection();
				BeginEditMode();
				switch(caretOnBeginEdit)
				{
					case CaretOnBeginEdit.START_OF_TEXT: textNavigator.MoveToStart(); break;
					case CaretOnBeginEdit.END_OF_TEXT: textNavigator.MoveToEnd(); break;
					default: textNavigator.MoveToEnd(); break;
				}
				textInputHandler.Process();
			}
			else
			{
				Debug.LogWarningFormat("Couldn't select input field, the input field is not initialized yet");
			}
		}

		/// <summary>Manually deselects this inputfield (call this to deselect this inputfield programmatically)</summary>
		public void ManualDeselect(EndEditReason endEditReason = EndEditReason.PROGRAMMATIC_DESELECT)
		{
			if(initialized)
			{
				Deselect(endEditReason);
			}
			else
			{
				Debug.LogWarningFormat("Couldn't deselect input field, the input field is not initialized yet");
			}
		}
		#endregion

		/// <summary>Initializes the InputField</summary>
		internal void InitializeInputField()
		{
			if(GetComponentInChildren<ScrollArea>() == null) //Deprecation check
			{
				Debug.LogWarning("This AdvancedInputField instance is deprecated, please use the ConversionTool (from the \'Advanced Input Field\' dropdown) to convert it to the new format");
			}

			Canvas.ForceUpdateCanvases(); //Make sure the layout information is correct
			TextRenderer.CaretWidth = CaretWidth;
			TextRenderer.Multiline = Multiline;
			ProcessedTextRenderer.CaretWidth = CaretWidth;
			ProcessedTextRenderer.Multiline = Multiline;
			PlaceholderTextRenderer.Multiline = Multiline;

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID || UNITY_WSA)
#if !UNITY_IOS //For some reason Unity doesn't register events from hardware keyboards on iOS
			if(ShouldUseHardwareKeyboard())
			{
				textInputHandler = new StandaloneTextInputHandler();
				textNavigator = new StandaloneTextNavigator();
				textManipulator = new TextManipulator();
			}
			else
#endif
			{
				textInputHandler = new MobileTextInputHandler();
				if(ActionBarEnabled)
				{
					((MobileTextInputHandler)textInputHandler).InitActionBar(this, TextRenderer);
				}
				textNavigator = new MobileTextNavigator();
				textManipulator = new MobileTextManipulator();
			}
#else
			textInputHandler = new StandaloneTextInputHandler();
			textNavigator = new StandaloneTextNavigator();
			textManipulator = new TextManipulator();
#endif

			textInputHandler.Initialize(this, textNavigator, textManipulator);
			textNavigator.Initialize(this, CaretRenderer, SelectionRenderer);
			textManipulator.Initialize(this, textNavigator, TextRenderer, ProcessedTextRenderer);

			initialized = true;
			if(LiveProcessing)
			{
				string processedText = liveProcessingFilter.ProcessText(text, textNavigator.CaretPosition);
				if(processedText != null)
				{
					ProcessedText = processedText;
				}
			}
			textNavigator.EndEditMode();
			textManipulator.EndEditMode();
			Selected = false;
			textNavigator.RefreshRenderedText();
			textNavigator.UpdateCaret();

			ScrollArea scrollArea = GetComponentInChildren<ScrollArea>();
			if(Multiline)
			{
				scrollArea.Horizontal = false;
				scrollArea.Vertical = true;
			}
			else
			{
				scrollArea.Horizontal = true;
				scrollArea.Vertical = false;
			}
			scrollArea.DragMode = dragMode;

			StartCoroutine(InitialTextRendererFix());
		}

		/// <summary>Workaround for incorrect text renderer size on initialize</summary>
		internal IEnumerator InitialTextRendererFix()
		{
			for(int i = 0; i < 3; i++)
			{
				UpdateActiveTextRenderer();
				yield return null;
			}
		}

		internal IEnumerator DelayedValueChanged()
		{
			yield return null;
			if(onValueChanged != null)
			{
				onValueChanged.Invoke(text);
			}
		}

		internal IEnumerator DelayedCaretPositionChanged()
		{
			yield return null;
			if(onCaretPositionChanged != null)
			{
				onCaretPositionChanged.Invoke(textNavigator.CaretPosition);
			}
		}

		internal IEnumerator DelayedTextSelectionChanged()
		{
			yield return null;
			if(onTextSelectionChanged != null)
			{
				onTextSelectionChanged.Invoke(textNavigator.SelectionStartPosition, textNavigator.SelectionEndPosition);
			}
		}

		/// <summary>Updates the Text Alignment based on given Text Renderer</summary>
		internal void UpdateTextAlignment(TextRenderer textRenderer)
		{
			Vector2 anchor = new Vector2();
			switch(textRenderer.TextAlignment)
			{
				case TextAlignment.TOP_LEFT: anchor = new Vector2(0, 1); break;
				case TextAlignment.TOP: anchor = new Vector2(0.5f, 1); break;
				case TextAlignment.TOP_RIGHT: anchor = new Vector2(1, 1); break;
				case TextAlignment.LEFT: anchor = new Vector2(0, 0.5f); break;
				case TextAlignment.CENTER: anchor = new Vector2(0.5f, 0.5f); break;
				case TextAlignment.RIGHT: anchor = new Vector2(1, 0.5f); break;
				case TextAlignment.BOTTOM_LEFT: anchor = new Vector2(0, 0); break;
				case TextAlignment.BOTTOM: anchor = new Vector2(0.5f, 0); break;
				case TextAlignment.BOTTOM_RIGHT: anchor = new Vector2(1, 0); break;
			}

			TextContentTransform.anchorMin = anchor;
			TextContentTransform.anchorMax = anchor;
			TextContentTransform.pivot = anchor;
		}

		/// <summary>Updates the Size based on given Text Renderer</summary>
		internal void UpdateSize(TextRenderer textRenderer)
		{
			Vector2 size = Size;
			Vector2 preferredSize = textRenderer.PreferredSize;
			TextContentTransform.sizeDelta = preferredSize;

			if(mode != InputFieldMode.SCROLL_TEXT)
			{
				switch(mode)
				{
					case InputFieldMode.HORIZONTAL_RESIZE_FIT_TEXT:
						float marginX = (RectTransform.rect.width - TextAreaTransform.rect.width);
						size.x = Mathf.Max(preferredSize.x, ResizeMinWidth - marginX);
						size.x += marginX;
						Size = size;
						break;
					case InputFieldMode.VERTICAL_RESIZE_FIT_TEXT:
						float marginY = (RectTransform.rect.height - TextAreaTransform.rect.height);
						size.y = Mathf.Max(preferredSize.y, ResizeMinHeight - marginY);
						size.y += marginY;
						Size = size;
						break;
				}
			}
		}

		/// <summary>Updates which Text Renderer that should be visible and which should be hidden</summary>
		internal void UpdateVisibleTextRenderers()
		{
			if(Selected)
			{
				if(LiveProcessing)
				{
					if(string.IsNullOrEmpty(processedText))
					{
						PlaceholderTextRenderer.Show();
						TextRenderer.Hide();
						ProcessedTextRenderer.Hide();
					}
					else
					{
						PlaceholderTextRenderer.Hide();
						TextRenderer.Hide();
						ProcessedTextRenderer.Show();
					}
				}
				else
				{
					if(string.IsNullOrEmpty(text))
					{
						PlaceholderTextRenderer.Show();
						TextRenderer.Hide();
						ProcessedTextRenderer.Hide();
					}
					else
					{
						PlaceholderTextRenderer.Hide();
						TextRenderer.Show();
						ProcessedTextRenderer.Hide();
					}
				}
			}
			else
			{
				if(LiveProcessing || PostProcessing)
				{
					if(string.IsNullOrEmpty(processedText))
					{
						PlaceholderTextRenderer.Show();
						TextRenderer.Hide();
						ProcessedTextRenderer.Hide();
					}
					else
					{
						PlaceholderTextRenderer.Hide();
						TextRenderer.Hide();
						ProcessedTextRenderer.Show();
					}
				}
				else
				{
					if(string.IsNullOrEmpty(text))
					{
						PlaceholderTextRenderer.Show();
						TextRenderer.Hide();
						ProcessedTextRenderer.Hide();
					}
					else
					{
						PlaceholderTextRenderer.Hide();
						TextRenderer.Show();
						ProcessedTextRenderer.Hide();
					}
				}
			}
		}

		/// <summary>Gets the currently active/visible Text Renderer</summary>
		internal TextRenderer GetActiveTextRenderer()
		{
			UpdateVisibleTextRenderers();

			if(PlaceholderTextRenderer.Visible)
			{
				return PlaceholderTextRenderer;
			}
			else if(ProcessedTextRenderer.Visible)
			{
				return ProcessedTextRenderer;
			}
			else
			{
				return TextRenderer;
			}
		}

		/// <summary>Updates text and caret of current active Text Renderer</summary>
		public void UpdateActiveTextRenderer()
		{
			TextRenderer activeTextRenderer = GetActiveTextRenderer();
			activeTextRenderer.UpdateImmediately();
			UpdateTextAlignment(activeTextRenderer);
			UpdateSize(activeTextRenderer);
			activeTextRenderer.UpdateImmediately();

			UpdateCaretPosition();
		}

		/// <summary>Updates the visual position of the caret</summary>
		public void UpdateCaretPosition()
		{
			if(Initialized)
			{
				textNavigator.UpdateCaret();
			}
		}

		/// <summary>Resets the drag start position to given position (after text delete)</summary>
		internal void ResetDragStartPosition(int position)
		{
			dragStartPosition = position;
		}

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
		/// <summary>Event callback when a hardware keyboard has been connected/disconnected</summary>
		/// <param name="connected">Indicates whether a hardware keyboard has been connected</param>
		internal void OnHardwareKeyboardChanged(bool connected)
		{
			InitializeInputField();
		}

		internal bool ShouldUseHardwareKeyboard()
		{
			bool shouldUseHardwareKeyboard = false;

			switch(Settings.MobileKeyboardBehaviour)
			{
				case MobileKeyboardBehaviour.USE_HARDWARE_KEYBOARD_WHEN_AVAILABLE:
					shouldUseHardwareKeyboard = NativeKeyboardManager.HardwareKeyboardConnected;
					break;
				case MobileKeyboardBehaviour.ALWAYS_USE_TOUCHSCREENKEYBOARD:
					shouldUseHardwareKeyboard = false;
					break;
				case MobileKeyboardBehaviour.ALWAYS_USE_HARDWAREKEYBOARD:
					shouldUseHardwareKeyboard = true;
					break;
			}

			return shouldUseHardwareKeyboard;
		}
#endif

		#region INTERFACE_METHODS
		public void OnPointerClick(PointerEventData eventData)
		{
            if(!interactable || ShouldBlockAllSelections) { return; }

			if(!Selected && selectionMode == SelectionMode.SELECT_ON_RELEASE)
			{
				Vector2 currentPosition = eventData.position;
				Vector2 move = currentPosition - pressPosition;
				float fontSize = GetActiveTextRenderer().FontSize;
				if(Mathf.Abs(move.x) > fontSize || Mathf.Abs(move.y) > fontSize) //Using font size as the tap threshold
				{
					EventSystem.current.SetSelectedGameObject(null);
					return;
				}

				beginEditReason = BeginEditReason.USER_SELECT;
				EnableSelection();
				BeginEditMode();

				Vector2 localMousePosition;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(TextAreaTransform, eventData.position, eventData.pressEventCamera, out localMousePosition);
				localMousePosition.x += (TextAreaTransform.rect.width * 0.5f);
				localMousePosition.y -= (TextAreaTransform.rect.height * 0.5f);

				if(caretOnBeginEdit == CaretOnBeginEdit.LOCATION_OF_CLICK)
				{
					textInputHandler.OnPress(localMousePosition);
					textInputHandler.OnRelease(localMousePosition);
				}
				else if(caretOnBeginEdit == CaretOnBeginEdit.START_OF_TEXT)
				{
					SetCaretToTextStart(true);
				}
				else if(caretOnBeginEdit == CaretOnBeginEdit.END_OF_TEXT)
				{
					SetCaretToTextEnd(true);
				}
			}
			else if(Selected && selectionMode == SelectionMode.SELECT_ON_PRESS && !editMode)
			{
				Vector2 currentPosition = eventData.position;
				Vector2 move = currentPosition - pressPosition;
				float fontSize = GetActiveTextRenderer().FontSize;
				if(Mathf.Abs(move.x) > fontSize || Mathf.Abs(move.y) > fontSize) //Using font size as the tap threshold
				{
					return;
				}

				beginEditReason = BeginEditReason.USER_SELECT;
				EnableSelection();
				BeginEditMode();
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
            if (ShouldBlockAllSelections) return;
			if(!Selected || dragMode == DragMode.MOVE_TEXT)
			{
				IBeginDragHandler beginDragHandler = transform.parent.GetComponentInParent<IBeginDragHandler>();
				if(beginDragHandler != null)
				{
					beginDragHandler.OnBeginDrag(eventData);
				}

				updateDrag = false;

				return;
			}

			updateDrag = true;

			if(currentStartDragMode == StartDragMode.FROM_CURRENT_POSITION)
			{
				dragStartPosition = textNavigator.VisibleCaretPosition;
			}
			else if(currentStartDragMode == StartDragMode.FROM_SELECTION_START)
			{
				dragStartPosition = textNavigator.VisibleSelectionStartPosition;
			}
			else if(currentStartDragMode == StartDragMode.FROM_SELECTION_END)
			{
				dragStartPosition = textNavigator.VisibleSelectionEndPosition;
			}

			eventData.Use();
		}

		public void OnDrag(PointerEventData eventData)
		{
            if (ShouldBlockAllSelections) return;
			if(!Selected || dragMode == DragMode.MOVE_TEXT)
			{
				IDragHandler dragHandler = transform.parent.GetComponentInParent<IDragHandler>();
				if(dragHandler != null)
				{
					dragHandler.OnDrag(eventData);
				}

				updateDrag = false;

				return;
			}

			if(PositionOutOfBounds(eventData))
			{
				if(!dragOutOfBounds)
				{
					dragOutOfBounds = true;
				}
			}
			else
			{
				dragOutOfBounds = false;
			}

			Vector2 localMousePosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(TextAreaTransform, eventData.position, eventData.pressEventCamera, out localMousePosition);
			localMousePosition.x += (TextAreaTransform.rect.width * 0.5f);
			localMousePosition.y -= (TextAreaTransform.rect.height * 0.5f);

			localMousePosition += dragOffset;

			if(dragMode == DragMode.UPDATE_TEXT_SELECTION)
			{
				int position = 0;
				if(LiveProcessing)
				{
					position = textNavigator.GetCharacterIndexFromPosition(ProcessedTextRenderer, localMousePosition);
				}
				else
				{
					position = textNavigator.GetCharacterIndexFromPosition(TextRenderer, localMousePosition);
				}

				textNavigator.UpdateSelectionArea(position, dragStartPosition, false);
			}

			textInputHandler.OnDrag(localMousePosition);

			eventData.Use();
		}

		public void OnEndDrag(PointerEventData eventData)
		{
            if (ShouldBlockAllSelections) return;
            if (!Selected || dragMode == DragMode.MOVE_TEXT)
			{
				IEndDragHandler endDragHandler = transform.parent.GetComponentInParent<IEndDragHandler>();
				if(endDragHandler != null)
				{
					endDragHandler.OnEndDrag(eventData);
				}

				updateDrag = false;
			}

			dragStartPosition = -1;
			currentStartDragMode = StartDragMode.FROM_CURRENT_POSITION;
			updateDrag = false;

			eventData.Use();
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
            if (ShouldBlockAllSelections) return;
            base.OnPointerDown(eventData);
			UserPressing = true;
			pressPosition = eventData.position;
			pressTextContentPosition = textContentTransform.anchoredPosition;

			if(!Selected)
			{
				if(selectionMode == SelectionMode.SELECT_ON_PRESS)
				{
					beginEditReason = BeginEditReason.USER_SELECT;
					EnableSelection();
				}
				else if(selectionMode == SelectionMode.SELECT_ON_RELEASE)
				{
					return;
				}
			}

			Vector2 localMousePosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(TextAreaTransform, eventData.position, eventData.pressEventCamera, out localMousePosition);
			localMousePosition.x += (TextAreaTransform.rect.width * 0.5f);
			localMousePosition.y -= (TextAreaTransform.rect.height * 0.5f);

			textInputHandler.OnPress(localMousePosition);
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
            if (ShouldBlockAllSelections) return;
            base.OnPointerUp(eventData);
			UserPressing = false;
			updateDrag = false;

			if(!Selected)
			{
				return;
			}

			Vector2 localMousePosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(TextAreaTransform, eventData.position, eventData.pressEventCamera, out localMousePosition);
			localMousePosition.x += (TextAreaTransform.rect.width * 0.5f);
			localMousePosition.y -= (TextAreaTransform.rect.height * 0.5f);

			textInputHandler.OnRelease(localMousePosition);
		}

		public override void OnDeselect(BaseEventData eventData)
		{
            base.OnDeselect(eventData);
            updateDrag = false;
            StartCoroutine(DelayedDeselect());
        }

		public void OnUpdateSelected(BaseEventData eventData)
		{
			if(textNavigator.EditMode)
			{
				textInputHandler.Process();
				textNavigator.UpdateSelected();

				if(updateDrag)
				{
					UpdateDrag();
				}
			}

			eventData.Use();
		}

		public override void Select()
		{
			ManualSelect();
		}

		internal void Deselect(EndEditReason reason)
		{
			endEditReason = reason;
			EventSystem.current.SetSelectedGameObject(null);
		}
		#endregion

		/// <summary>Updates the drag for out of bounds text scroll</summary>
		internal void UpdateDrag()
		{
			if(dragOutOfBounds)
			{
				Vector2 localMousePosition;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(TextAreaTransform, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out localMousePosition);
				localMousePosition.x += (TextAreaTransform.rect.width * 0.5f);
				localMousePosition.y -= (TextAreaTransform.rect.height * 0.5f);

				localMousePosition += dragOffset;

				if(dragMode == DragMode.UPDATE_TEXT_SELECTION)
				{
					int position = 0;
					if(LiveProcessing)
					{
						position = textNavigator.GetCharacterIndexFromPosition(ProcessedTextRenderer, localMousePosition);
					}
					else
					{
						position = textNavigator.GetCharacterIndexFromPosition(TextRenderer, localMousePosition);
					}
					textNavigator.UpdateSelectionArea(position, dragStartPosition, false);

					float fullSize = Mathf.Min(Canvas.pixelRect.width, Canvas.pixelRect.height); //Use same reference size for horizontal and vertical scroll sensitivity calculation
					if(Multiline)
					{
						float scrollSensitivity = 1;
						if(localMousePosition.y > 0)
						{
							scrollSensitivity += fastScrollSensitivity * Mathf.Abs(localMousePosition.y / fullSize);
						}
						else if(localMousePosition.y < -TextAreaTransform.rect.height)
						{
							scrollSensitivity += fastScrollSensitivity * Mathf.Abs((-TextAreaTransform.rect.height - localMousePosition.y) / fullSize);
						}

						textNavigator.UpdateVerticalScrollPosition(TextRenderer, scrollSensitivity);
					}
					else
					{
						float scrollSensitivity = 1;
						if(localMousePosition.x < 0)
						{
							scrollSensitivity += fastScrollSensitivity * Mathf.Abs(localMousePosition.x / fullSize);
						}
						else if(localMousePosition.x > TextAreaTransform.rect.width)
						{
							scrollSensitivity += fastScrollSensitivity * Mathf.Abs((localMousePosition.x - TextAreaTransform.rect.width) / fullSize);
						}

						textNavigator.UpdateHorizontalScrollPosition(TextRenderer, scrollSensitivity);
					}
				}
			}
		}

		/// <summary>Checks if position is out of bounds</summary>
		/// <param name="eventData">The event data to check</param>
		/// <returns>true is position is out of bounds</returns>
		internal bool PositionOutOfBounds(PointerEventData eventData)
		{
			return !RectTransformUtility.RectangleContainsScreenPoint(TextAreaTransform, eventData.position, eventData.pressEventCamera);
		}

		/// <summary>Marks as selected</summary>
		internal void EnableSelection()
		{
			Selected = true;
		}

		/// <summary>Marks as deselected</summary>
		internal void DisableSelection()
		{
			Selected = false;
		}

		/// <summary>Enables text editing</summary>
		internal void BeginEditMode()
		{
			editMode = true;
			textInputHandler.BeginEditMode();
			textNavigator.BeginEditMode();
			textManipulator.BeginEditMode();
			UpdateActiveTextRenderer();

			if(onBeginEdit != null)
			{
				onBeginEdit.Invoke(beginEditReason);
			}
		}

		/// <summary>Disables text editing</summary>
		internal void EndEditMode()
		{
			editMode = false;
			textNavigator.EndEditMode();
			textManipulator.EndEditMode();
			textInputHandler.CancelInput();
			UpdateActiveTextRenderer();

			if(onEndEdit != null)
			{
				onEndEdit.Invoke(Text, endEditReason);
			}
		}

		/// <summary>Checks if ActionBar is selected</summary>
		/// <returns>true if ActionBar is selected</returns>
		internal bool IsActionBarSelected()
		{
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_WSA)
			GameObject currentSelection = EventSystem.current.currentSelectedGameObject;
			if(currentSelection != null)
			{
				return currentSelection.GetComponentInParent<CanvasFrontRenderer>();
			}
#endif

			return false;
		}

		/// <summary>Checks if an InputFieldButton child is pressed or selected</summary>
		/// <returns>true if an InputFieldButton child is pressed or selected</returns>
		internal bool IsInputFieldButtonSelected()
		{
			InputFieldButton[] inputFieldButtons = GetComponentsInChildren<InputFieldButton>();
			int length = inputFieldButtons.Length;
			for(int i = 0; i < length; i++)
			{
				if(inputFieldButtons[i].IsButtonPressed() || inputFieldButtons[i].IsButtonSelected())
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>Delayed deselect method to check for valid deselects</summary>
		internal IEnumerator DelayedDeselect()
		{
			yield return null;

			if(!initialized)
			{
				yield break;
			}
            //Debug.Log("DELAYED DESELECT BLOCK " + ShouldBlockDeselect);
            if (IsActionBarSelected() || IsInputFieldButtonSelected() || ShouldBlockDeselect) //Invalid deselect
            {
                Reselect();
                //ShouldBlockDeselect = false;
            }
            else //Valid deselect
            {
                DisableSelection();
                EndEditMode();
            }

            endEditReason = EndEditReason.USER_DESELECT; //Reset to default reason (deselection)
        }

		/// <summary>(Re)selects the InputField</summary>
		internal void Reselect()
		{
			EventSystem.current.SetSelectedGameObject(gameObject);
			textInputHandler.OnSelect();
		}
	}
}
