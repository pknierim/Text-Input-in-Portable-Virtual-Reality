using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedInputFieldPlugin;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.EventSystems;

public class TextEditController : MonoBehaviour, ITextEditControllerListener, IDragHandler, IEndDragHandler
{
    public AdvancedInputField InputField;
    public AdvancedInputField CorrectionsField;
    public TextMeshProUGUI CorrectionsTM;
    public Text CorrectionsText;
    public Button SubmitButton;
    protected EditSentences EditSentences;
    protected ILogging logging;
    protected IEditSentences editSentences;
    protected IControllerLogic controller;
    public static bool gameStarted = false;
    public static bool startOfRun;
    protected int mode;
    protected string CurrentValue = "";
    protected string CompareValue = "";
    protected bool initialized;
    protected string InputStringOld = "";
    protected string InputStringCurrent = "";
    protected string CurrentInputChar;
    protected int textsCount = 0;
    protected bool InputAllowed;
    protected bool IsTraining;
    protected int diff = 0;
    protected string lastSelectedWord;
    protected bool loggingAllowed;
    protected GameObject Hints;
    protected float lastYPosInputField;
    protected bool firstYPosInputFieldUpdated;

    protected static int NumberOfTexts = GlobalSettings.NumberOfTextsForEditing;

    void Start()
    {
        EditSentences = GameObject.Find("MainController").GetComponent<EditSentences>();
        Hints = GameObject.Find("Hints");
        Hints.gameObject.SetActive(true);
        EditSentences.SetEditControllerListener(this);
        InputField.interactable = true;
        IsTraining = true;
        InputAllowed = false;
        firstYPosInputFieldUpdated = false;
        if (SubmitButton != null) SubmitButton.gameObject.SetActive(false);
        if (mode == 4 || mode == 7) SelectInputField();
    }

    public void SetMode(int m)
    {
        this.mode = m;
        InputField.ShouldBlockDeselect = mode != 5;
        InputField.ShouldBlockAllSelections = mode == 6 || mode == 4;
        diff = mode == 5 ? 100 : 90;
    }

    public void SetListeners(IControllerLogic c, IEditSentences e, ILogging l)
    {
        this.controller = c;
        this.logging = l;
        this.editSentences = e;
    }

    public void SetupListeners(bool forSoft)
    {
        if (forSoft)
        {
            InputField.OnEndEdit.AddListener(OnInputEnded);
            InputField.OnCaretPositionChanged.AddListener(OnCursorPositionChanged);
            InputField.OnValueChanged.AddListener(OnValueChange);
        }
        InputField.OnTextSelectionChanged.AddListener(OnTextSelectionChange);
    }

    // Update is called once per frame
    void Update()
    {
        // For hard keyboard scenario
        if (Input.GetKeyDown(KeyCode.Escape) && gameStarted && InputAllowed)
        {
            //NumberOfTexts = EditSentences.CurrentPackage.TextObjects.Length;
            if (textsCount != 0 && textsCount < NumberOfTexts && !IsTraining)
            {
                controller.OnTextFinished(EditSentences.CurrentText, GetInputFieldText());
                editSentences.OnTextFinished();
            }
            else
            {
                IsTraining = false;
                InputAllowed = false;
                controller.OnTextFinished(EditSentences.CurrentText, GetInputFieldText());
                if (mode == 4 || mode == 7) if (GlobalSettings.UseTextMeshInVR) CorrectionsTM.text = ""; else CorrectionsText.text = "";
                else CorrectionsField.Clear();
                InputField.Clear();
                EndInput();
                controller.RunFinished();
            }                                  // A text has been finished
        }
        if (InputAllowed && startOfRun)
        {
            editSentences.OnEditStarted(IsTraining);
            Hints.gameObject.SetActive(false);
            startOfRun = false;
        }
        if (!InputField.Selected && InputAllowed)
        {
            if (mode == 6 || mode == 5)
            {
                SelectInputField();
            }
            CompareValue = InputField.Text;
            InputStringOld = InputField.Text;
        }
        if (!InputAllowed && InputField.Selected)
        {
            InputField.Clear();
            UnSelectInputField();
        }
        if (InputField.Text.Length > 0 && Input.anyKeyDown) FormatSentences();
        if (mode == 5 || mode == 6 && (CorrectionsField.TextContentTransform.position.y - InputField.TextContentTransform.position.y) < diff) AdjustTextMargin();
        if (mode == 5) SubmitButton.gameObject.SetActive(InputAllowed);
        if (mode == 6 && !firstYPosInputFieldUpdated && Input.anyKeyDown) lastYPosInputField = InputField.TextContentTransform.position.y; firstYPosInputFieldUpdated = true;
    }

    public void OnScroll()
    {
        AdjustTextMargin();
        if (loggingAllowed) 
        {
            if (mode == 5 && Input.touchCount > 0) logging.OnScrollAction();
            else if (mode == 6 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) && Math.Abs(lastYPosInputField - InputField.TextContentTransform.position.y) > 10) 
            {
                logging.OnScrollAction();
                lastYPosInputField = InputField.TextContentTransform.position.y;
            }
        }
    }

    private void AdjustTextMargin()
    {
        CorrectionsField.TextContentTransform.position = new Vector2(CorrectionsField.TextContentTransform.position.x,
                                                                     InputField.TextContentTransform.position.y + diff);
    }

    private void FormatSentences()
    {
        InputField.SetText(Regex.Replace(InputField.Text, @"(\. )([A-Za-z])", "$1" + Environment.NewLine + "$2"));
        InputField.SetText(Regex.Replace(InputField.Text, @"(\.)([A-Za-z])", "$1" + Environment.NewLine + "$2"));
    }

    public void EndInput()
    {
        InputField.interactable &= (mode != 5 && mode != 6);
        ClearAllFields();
    }

    public void ClearAllFields()
    {
        InputField.Clear();
        if (mode == 4 || mode == 7) if (GlobalSettings.UseTextMeshInVR) CorrectionsTM.text = ""; else CorrectionsText.text = "";
        else CorrectionsField.Text = "";
        InputField.ShouldBlockDeselect = false;
        UnSelectInputField();
    }

    private void SelectInputField()
    {
        InputField.interactable = true;
        InputField.Select();
        InputField.ManualSelect();
    }

    private void UnSelectInputField()
    {
        InputField.ManualDeselect();
    }

    public void SetInputAllowed(bool b)
    {
        this.InputAllowed = b;
    }

    public void ResetTextCount()
    {
        textsCount = 0;
        InputField.Clear();
        if (mode == 5 || mode == 6) CorrectionsField.Clear();
        else if (GlobalSettings.UseTextMeshInVR) CorrectionsTM.text = ""; else CorrectionsText.text = "";
        if (mode == 6 || mode == 5) UnSelectInputField();
    }

    public void OnTimeOverForRun()
    {
        InputAllowed = false;
        UnSelectInputField();
        if (mode == 4 || mode == 7) if (GlobalSettings.UseTextMeshInVR) CorrectionsTM.text = ""; else CorrectionsText.text = "";
        else CorrectionsField.Text = "";
        StartCoroutine(TimeOverDelay());
    }

    private IEnumerator TimeOverDelay()
    {
        yield return new WaitForSeconds(3);
        controller.RunFinished();
    }

    public void NextText(string withErrors, string withCorrections)
    {
        Debug.Log("TextEditController - NextText - " + withErrors.Length);
        if (!IsTraining) textsCount++;
        InputField.SetText(withErrors);
        InputField.SetCaretPosition(withErrors.Length);
        CompareValue = InputField.Text;
        InputStringOld = InputField.Text;
        if (mode == 5 || mode == 6) CorrectionsField.SetText(withCorrections);
        else if (GlobalSettings.UseTextMeshInVR) CorrectionsTM.text = withCorrections; else CorrectionsText.text = withCorrections;
        InputAllowed = true;
        loggingAllowed = true;
        SelectInputField();
    }

    public void OnInputEnded(string result, EndEditReason reason)
    {
        // For soft keyboard scenario
        Debug.Log("TextEditController - OnEndEdit - " + reason);
        if (reason == EndEditReason.KEYBOARD_DONE)
        {
            OnSubmit();
        }
    }

    public void OnSubmit()
    {
        logging.OnSoftKeyboardSubmitted();
        if (textsCount < NumberOfTexts && !IsTraining)
        {
            if (mode == 6) UnSelectInputField();
            InputAllowed = false;
            loggingAllowed = false;
            controller.OnTextFinished(EditSentences.CurrentText, GetInputFieldText());
            editSentences.OnTextFinished();
            if (mode == 4 || mode == 7) SelectInputField();
        }
        else
        {
            IsTraining = false;
            InputAllowed = false;
            loggingAllowed = false;
            controller.OnTextFinished(EditSentences.CurrentText, GetInputFieldText());
            if (mode == 4 || mode == 7) if (GlobalSettings.UseTextMeshInVR) CorrectionsTM.text = ""; else CorrectionsText.text = "";
            else CorrectionsField.SetText("");
            InputField.Clear();
            EndInput();
            controller.RunFinished();
        }
    }

    public void OnCursorPositionChanged(int pos)
    {
        if (CompareValue.Length != 0)
        {
            if (CurrentValue.Length == CompareValue.Length)
            {
                if (pos < InputField.Text.Length)
                    logging.OnCursorPositionChanged(pos);
            }
            else
            {
                CurrentValue = CompareValue;
            }
        }
    }

    public void OnTextSelectionChange(int start, int end)
    {
        if (start != end && start < end)
        {
            if (CompareValue.Substring(start, end - start) != lastSelectedWord)
            {
                Debug.Log("Text selected by user " + start + ", " + end + " (" + CompareValue.Substring(start, end - start) + ")");
                logging.OnTextSelectionDone(start, end, CompareValue.Substring(start, end - start));
                lastSelectedWord = CompareValue.Substring(start, end - start);
            }
        }
    }

    public void OnValueChange(string value)
    {
        if (!InputAllowed) return;

        CompareValue = value;

        if (InputStringOld.Length > 0 && !value.Equals(InputStringOld))
        {
            if (value.Length < InputStringOld.Length)
            {
                if (value.Length == InputStringOld.Length - 1)
                {
                    CurrentInputChar = "\b";
                }
                else
                {
                    if (InputStringOld.Length - value.Length == lastSelectedWord.Length)
                    {
                        CurrentInputChar = "\b";
                    }
                    else
                    {
                        CurrentInputChar = value.Substring(InputField.CaretPosition - 1, 1);
                    }
                }
            }
            else
            {
                CurrentInputChar = value.Substring(InputField.CaretPosition - 1, 1);
            }
            InputStringOld = value;
            logging.OnTouchKeystrokeDone(CurrentInputChar);
        }
        else if (InputStringOld.Length == 0)
        {
            InputStringOld = value;
            CurrentInputChar = value;
            logging.OnTouchKeystrokeDone(CurrentInputChar);
        }
    }

    public string GetInputFieldText()
    {
        return InputField.Text.Replace(Environment.NewLine, "");
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
    }
}

public interface ITextEditControllerListener
{
    void NextText(string nextWithErrors, string nextWithCorrections);
}
