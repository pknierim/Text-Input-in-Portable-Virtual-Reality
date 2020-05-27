using System.Collections;
using AdvancedInputFieldPlugin;
using UnityEngine;
using UnityEngine.UI;

public class TextInputControllerSoft : MonoBehaviour, ITextInputControllerListener
{
    public AdvancedInputField inputField;
    public Text originalText;
    private bool inputAllowed;
    public static bool startOfRun;
    private string inputString;
    protected IPhrases phrases;
    protected string InputStringOld = "";
    protected string InputStringCurrent = "";
    protected string CurrentInputChar;
    protected string CurrentPhrase;
    protected ILogging logging;
    protected IControllerLogic controller;
    protected Phrases Phrases;

    // Start is called before the first frame update
    void Start()
    {
        inputAllowed = false;
        inputField.OnEndEdit.AddListener(OnInputEnd);
        inputField.OnValueChanged.AddListener(OnValueChange);
        Phrases = GameObject.Find("MainController").GetComponent<Phrases>();
        Phrases.SetListenerTextInput(this);
        if (inputField.Selected) inputField.ManualDeselect();
        //inputField.ShouldBlockDeselect = true;
    }

    private void Update()
    {
        if (inputAllowed && !inputField.Selected)
        {
            SelectInputField();
        }
    }

    public void SetListener(IControllerLogic c, IPhrases p, ILogging l)
    {
        this.controller = c;
        this.phrases = p;
        this.logging = l;
    }

    private void ActivateInput()
    {
        SelectInputField();
        phrases.OnStartTask();
        startOfRun = false;
    }

    public void EndInput()
    {
        inputField.interactable = false;
        ClearAllFields();
    }

    private void ClearAllFields()
    {
        inputField.Clear();
        originalText.text = "";
        UnSelectInputField();
    }

    public void SelectInputField()
    {
        inputField.Select();
        inputField.ManualSelect();
    }

    public void UnSelectInputField()
    {
        inputField.ManualDeselect();
    }

    public void SetInputAllowed(bool b)
    {
        this.inputAllowed = b;
        if (inputAllowed) ActivateInput();
    }

    private void ResetInputFieldText()
    {
        inputString = "";
        inputField.SetText(inputString);
        InputStringOld = "";
    }

    public string GetInputString()
    {
        inputString = inputField.Text;
        return this.inputString;
    }

    public void NextSentence(string Sentence)
    {
        Debug.Log("TextInputControllerSoft - NextSentence " + Sentence);
        CurrentPhrase = Sentence;
        originalText.text = Sentence;
        inputAllowed = true;
        if (!inputField.Selected) SelectInputField();
    }

    public void OnFinished(bool timeOver)
    {
        if (!timeOver)
        {
            //controller.OnTextFinished(CurrentPhrase, GetInputString());
            originalText.text = "All phrases completed!";
            inputAllowed = false;
            UnSelectInputField();
            StartCoroutine(Completed());
        }
        else
        {
            originalText.text = GlobalSettings.CurrentRound != 0 ? "<b>Time over</b> for round " + "<b>" + (GlobalSettings.CurrentRound) + "</b>"
                    : "<b>Time over</b> for <b> training round</b>";
            inputAllowed = false;
            ClearAllFields();
            UnSelectInputField();
            StartCoroutine(Completed());
        }
    }

    private IEnumerator Completed()
    {
        yield return new WaitForSeconds(2);
        originalText.text = "";
        controller.RunFinished();
    }

    public void OnInputEnd(string result, EndEditReason reason)
    {
        if (reason == EndEditReason.KEYBOARD_DONE)
        {
            Debug.Log("TextInputControllerSoft - OnInputEnd");
            inputAllowed = false;
            logging.OnSoftKeyboardSubmitted();
            controller.OnTextFinished(CurrentPhrase, GetInputString());
            ResetInputFieldText();
            phrases.TextFinished();
        }
        else 
        {
            // Reselect input field since only DONE is allowed for deselecting
            SelectInputField();
        }
    }

    public void OnValueChange(string value)
    {
        //inputField.SetText(value);
        if (!inputAllowed) return;
        if (InputStringOld.Length > 0 && !value.Equals(InputStringOld))
        {
            CurrentInputChar = value.Length > InputStringOld.Length ? value.Substring(InputStringOld.Length, 1) : "\b";
            InputStringOld = value;
            logging.OnTouchKeystrokeDone(CurrentInputChar);
        }
        else if (InputStringOld.Length == 0)
        {
            InputStringOld = value;
            CurrentInputChar = value;
            logging.OnTouchKeystrokeDone(CurrentInputChar);
        }
        //Debug.Log("OnValueChange - InputStringCurrent: " + value);
        //Debug.Log("OnValueChange - CurrentInputChar: " + (CurrentInputChar == " "));
        //logging.OnTouchKeystrokeDone(CurrentInputChar);
    }
}
