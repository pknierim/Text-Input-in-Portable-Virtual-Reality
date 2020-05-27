using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedInputFieldPlugin;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class TextInputController : MonoBehaviour, ITextInputControllerListener
{
    public AdvancedInputField inputField;
    public TextMeshProUGUI originalText;
    public Text original;
    public int mode;
    private bool inputAllowed;
    public static bool startOfRun;
    private string inputString;
    protected string CurrentPhrase;
    protected IPhrases PhrasesListener;
    protected IControllerLogic controller;
    protected Phrases Phrases;
    protected float initX;
    protected bool KeyboardCheck;

    // Start is called before the first frame update
    void Start()
    {
        inputAllowed = false;
        inputField.AutocapitalizationType = AutocapitalizationType.NONE;
        inputField.OnEndEdit.AddListener(OnInputEnd);
        inputField.OnBeginEdit.AddListener(OnInputBegin);
        inputField.OnValueChanged.AddListener(OnValueChanged);
        Phrases = GameObject.Find("MainController").GetComponent<Phrases>();
        Phrases.SetListenerTextInput(this);
        inputField.ShouldBlockDeselect = true;
        KeyboardCheck = false;
        //initX = transform.position.x;
        //transform.position = new Vector2(Screen.width, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8) && !inputField.Selected)
        {
            inputField.Select();
        }
        if (Input.GetKeyDown(KeyCode.F8) && inputField.Selected)
        {
            inputField.ManualDeselect();
        }

        if (inputAllowed && startOfRun) //!inputField.Selected &&
        {
            Debug.Log("TextInputController - 1");
            startOfRun = false;
            PhrasesListener.OnStartTask();
            //inputField.ShouldBlockDeselect = true;
        }
        if (Input.GetKeyDown(KeyCode.Return) && inputAllowed)
        {
            Debug.Log("TextInputController - 2");
            ResetInputFieldText();
            controller.OnTextFinished(CurrentPhrase, GetInputString());
            PhrasesListener.TextFinished();
        }
        if (inputAllowed && !inputField.Selected) SelectInputField();

        if (!inputAllowed && inputField.Selected) 
        {
            inputField.Clear();
            UnSelectInputField();
        }
    }

    public void SetMode(int m)
    {
        this.mode = m;
        inputField.ShouldBlockAllSelections = mode == 0;
    }

    public void SetListener(IControllerLogic c, IPhrases p)
    {
        this.controller = c;
        this.PhrasesListener = p;
    }

    public void SetInputAllowed(bool b)
    {
        Debug.Log("TextInputController - SetInputAllowed - " + b);
        inputAllowed = b;
        if (b) 
        {
            SelectInputField();
        }
        if (!b) UnSelectInputField();
    }

    public string GetInputString()
    {
        inputString = inputField.Text;
        return this.inputString;
    }

    public void EndInput()
    {
        inputField.interactable &= (mode != 1 && mode != 2);
        ClearAllFields();
    }

    public void ClearAllFields()
    {
        inputField.Clear();
        if (mode == 0 || mode == 3) originalText.text = "";
        else original.text = "";
        inputField.ShouldBlockDeselect = false;
        UnSelectInputField();
    }

    public void SelectInputField()
    {
        inputField.interactable = true;
        inputField.ShouldBlockDeselect = true;
        //inputField.Select();
        inputField.ManualSelect();
    }

    public void UnSelectInputField()
    {
        inputField.ManualDeselect();
    }

    private void ResetInputFieldText()
    {
        inputField.Clear();
        inputString = "";
        inputField.SetText(inputString);
        SetInputAllowed(false);
        //transform.position = new Vector2(Screen.width, transform.position.y);
        //inputField.CaretColor = Color.clear;
    }

    public void NextSentence(string Sentence)
    {
        CurrentPhrase = Sentence;
        if (mode == 0 || mode == 3) originalText.text = Sentence;
        else original.text = Sentence;
        SetInputAllowed(true);
        SelectInputField();
    }

    public void OnFinished(bool timeOver)
    {
        if (!timeOver)
        {
            if (mode == 0 || mode == 3) originalText.text = "All phrases <b>completed</b>!";
            else original.text = "All phrases <b>completed</b>!";
            inputAllowed = false;
            inputField.ShouldBlockDeselect = false;
            UnSelectInputField();
            StartCoroutine(Completed());
        }
        else 
        {
            if (mode == 0 || mode == 3) originalText.text = GlobalSettings.CurrentRound != 0 ? "<b>Time over</b> for <b>Round " + (GlobalSettings.CurrentRound) + "</b>"
                    : "<b>Time over</b> for <b> Training Round</b>";
            else original.text = GlobalSettings.CurrentRound != 0 ? "<b>Time over</b> for <b>Round " + (GlobalSettings.CurrentRound) + "</b>"
                    : "<b>Time over</b> for <b> Training Round</b>";
            inputAllowed = false;
            inputField.ShouldBlockDeselect = false;
            UnSelectInputField();
            //controller.OnTextFinished(CurrentPhrase, GetInputString());
            StartCoroutine(Completed());
        }
    }

    public void OnInputBegin(BeginEditReason arg0)
    {
        Debug.Log("TextInputController - OnInputBegin - " + arg0);
    }

    public void OnInputEnd(string result, EndEditReason reason)
    {
        Debug.Log("TextInputController - OnInputEnd - " + reason + "\n" + result);
        //inputField.ManualSelect();
    }

    public void OnValueChanged(string val)
    {
        if (val.Length > 0)
        {

        }
    }

    private IEnumerator Completed()
    {
        yield return new WaitForSeconds(2);
        if (mode == 0 || mode == 3) originalText.text = "";
        else original.text = "";
        controller.RunFinished();
    }
}

public interface ITextInputControllerListener
{
    void NextSentence(string Sentence);
    void OnFinished(bool timeOver);
}
