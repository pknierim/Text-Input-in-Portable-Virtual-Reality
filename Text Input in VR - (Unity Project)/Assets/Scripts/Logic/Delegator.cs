using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mode = MainController.Mode;

public class Delegator : AppDelegator, IDelegator
{
    public GameObject WritingScreen;
    public GameObject EditingScreen;
    public TextMeshProUGUI InfoText;

    protected Condition CurrentCondition;
    protected Task CurrentTask;
    protected GameObject MainController;
    protected MainController MainControllerScript;
    protected Phrases Phrases;
    protected CountDown CountDown;
    protected EditSentences EditSentences;
    protected GameTimer GameTimer;
    protected Logging Logging;
    protected GameObject KeyboardTarget;
    protected bool IsVR;

    void Start()
    {
        CurrentCondition = GlobalSettings.Condition;
        CurrentTask = GlobalSettings.CurrentTask;
        GlobalSettings.CurrentWriting = CurrentTask == Task.Writing;
        GlobalSettings.GlobalBlock = true;
        GlobalSettings.TaskNumber = 1;
        UpdateIsVR();
        MainController = GameObject.Find("MainController");
        MainControllerScript = MainController.GetComponent<MainController>();
        SetupComponentsForTask();
    }

    private void UpdateIsVR()
    {
        IsVR = (CurrentCondition == Condition.VR_VideoCrop || CurrentCondition == Condition.VR_No_Hands_No_Keyboard);
    }

    private void SetupComponentsForTask()
    {
        switch (CurrentCondition)
        {
            case Condition.VR_VideoCrop:
                SetupVRVideoCropComponents();
                break;
            case Condition.VR_No_Hands_No_Keyboard:
                SetupVR3DComponents();
                break;
            case Condition.Softkeyboard_Phone:
                SetupSoftKeyboardComponents();
                break;
            case Condition.Hardkeyboard_Phone:
                SetupHardKeyboardComponents();
                break;
        }
    }

    private void SetupVRVideoCropComponents()
    {
        Debug.Log("SetupVRVideoCropComponents 1");
#if !UNITY_EDITOR
        KeyboardTarget = GameObject.Find("KeyboardTarget");
        GameObject.Find("UI_Render_Controller Variant").GetComponent<RC_UI_Texture>().is3DModus = false;
       //GameObject.Find("VideoCrop").SetActive(true);
#endif
        if (CurrentTask == Task.Writing)
        {
            //GameObject.Find("3D").SetActive(false);
            EditingScreen.SetActive(false);
            MainControllerScript.GetComponent<CountDown>().countDown = GameObject.Find("InfoCountDown").GetComponent<TextMeshProUGUI>();
            MainControllerScript.mode = Mode.Writing_VR_VideoCrop;
        }
        else 
        {
            EditingScreen.SetActive(true);
            WritingScreen.SetActive(false);
            //Phrases.phrasesText = GameObject.Find("PhrasesText").GetComponent<Text>();
            MainControllerScript.mode = Mode.Editing_VR_VideoCrop;
            MainController.GetComponent<Phrases>().enabled = false;
        }
        MainControllerScript.ResetVariables();
        MainControllerScript.Setup(this);
    }

    private void SetupVR3DComponents()
    {
        KeyboardTarget = GameObject.Find("KeyboardTarget");
        //GameObject.Find("UI_Render_Controller Variant").GetComponent<RC_UI_Texture>().is3DModus = true;
        if (CurrentTask == Task.Writing)
        {
            GameObject.Find("VideoCrop").SetActive(false);
            GameObject.Find("Region_Capture Variant").SetActive(false);
            GameObject.Find("Magic_Keyboard_2").SetActive(false);
            //GameObject.Find("KeyboardImg").SetActive(false);
            GameObject.Find("UI_Render_Controller Variant").SetActive(false);
            EditingScreen.SetActive(false);
            MainControllerScript.GetComponent<CountDown>().countDown = GameObject.Find("InfoCountDown").GetComponent<TextMeshProUGUI>();
            MainControllerScript.mode = Mode.Writing_VR_3D;
        }
        else
        {
            EditingScreen.SetActive(true);
            WritingScreen.SetActive(false);
            //GameObject.Find("3D").SetActive(true);
            //Phrases.phrasesText = GameObject.Find("PhrasesText").GetComponent<Text>();
            MainControllerScript.mode = Mode.Editing_VR_3D;
            MainController.GetComponent<Phrases>().enabled = false;
        }
        MainControllerScript.ResetVariables();
        MainControllerScript.Setup(this);
    }

    private void SetupSoftKeyboardComponents()
    {
        if (CurrentTask == Task.Writing)
        {
            EditingScreen.SetActive(false);
            MainControllerScript.GetComponent<CountDown>().countDown = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
            InfoText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
            MainControllerScript.mode = Mode.Writing_SoftKeyboard;
        }
        else
        {
            EditingScreen.SetActive(true);
            WritingScreen.SetActive(false);
            MainControllerScript.GetComponent<CountDown>().countDown = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
            InfoText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
            MainControllerScript.mode = Mode.Editing_SoftKeyboard;
            MainController.GetComponent<Phrases>().enabled = false;
        }
        MainControllerScript.ResetVariables();
        MainControllerScript.Setup(this);
    }

    private void SetupHardKeyboardComponents()
    {
        if (CurrentTask == Task.Writing)
        {
            EditingScreen.SetActive(false);
            MainControllerScript.GetComponent<CountDown>().countDown = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
            InfoText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
            MainControllerScript.mode = Mode.Writing_HardKeyboard;
        }
        else
        {
            EditingScreen.SetActive(true);
            WritingScreen.SetActive(false);
            //Phrases.phrasesText = GameObject.Find("PhrasesText").GetComponent<Text>();
            MainControllerScript.GetComponent<CountDown>().countDown = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
            InfoText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
            MainControllerScript.mode = Mode.Editing_HardKeyboard;
            MainController.GetComponent<Phrases>().enabled = false;
        }
        MainControllerScript.ResetVariables();
        MainControllerScript.Setup(this);
    }

    public void OnTaskFinished(bool timeOver)
    {
        bool conditionFinished = GlobalSettings.TaskNumber == 2;
        //CurrentTask = GlobalSettings.CurrentWriting ? Task.Writing : Task.Editing;
        if (!conditionFinished)
        {
            Debug.Log("Delegator - OnTaskFinished - Next Task");
            GlobalSettings.GlobalBlock = true;
            SetInfoText(true, timeOver);
            CurrentTask = GlobalSettings.CurrentTask == Task.Writing ? Task.Editing : Task.Writing;
            GlobalSettings.CurrentWriting = CurrentTask == Task.Writing;
            GlobalSettings.TaskNumber = conditionFinished ? 1 : 2;
            StartCoroutine(OnTaskFinishedAfter(2));
        }
        else
        {
            if (GlobalSettings.NumberOfFinishedConditions == GlobalSettings.NumberOfConditions - 1)
            {
                Debug.Log("Delegator - OnTaskFinished - Finished");
                SetInfoText(false, timeOver);
                StartCoroutine(OnTaskFinishedAfter(1));
            }
            else
            {
                Debug.Log("Delegator - OnTaskFinished - Next Condition");
                SetInfoText(true, timeOver);

                GlobalSettings.TaskNumber = conditionFinished ? 1 : 2;
                StartCoroutine(OnTaskFinishedAfter(0));
            }
        }

        //if (CurrentTask == Task.Writing)
        //{
        //    Debug.Log("Delegator - OnTaskFinished - Next Task");
        //    GlobalSettings.GlobalBlock = true;
        //    SetInfoText(true, timeOver);
        //    CurrentTask = GlobalSettings.CurrentTask == Task.Writing ? Task.Editing : Task.Writing;
        //    GlobalSettings.CurrentWriting = CurrentTask == Task.Writing;
        //    GlobalSettings.TaskNumber = conditionFinished ? 1 : 2;
        //    StartCoroutine(OnTaskFinishedAfter(conditionFinished ? 0 : 2));
        //}
        //else 
        //{
        //    if (!allTasksFinished)
        //    {
        //        // Condition finished, go back to menu and inform AppDelegator
        //        Debug.Log("Delegator - OnTaskFinished - Next Condition");
        //        SetInfoText(true, timeOver);

        //        GlobalSettings.TaskNumber = conditionFinished ? 1 : 2;
        //        StartCoroutine(OnTaskFinishedAfter(conditionFinished ? 0 : 2));
        //    }
        //    else 
        //    {
        //        Debug.Log("Delegator - OnTaskFinished - Finished");
        //        SetInfoText(false, timeOver);
        //        StartCoroutine(OnTaskFinishedAfter(CurrentTask == Task.Editing ? 1 : 0));
        //    }
        //}
    }

    private void SetInfoText(bool isForCondition, bool timeOver)
    {
        if(isForCondition)
        {
            UpdateIsVR();
            InfoText.color = Color.black;
            InfoText.fontSize = IsVR ? GlobalSettings.FontSizeInfoVR : GlobalSettings.FontSizeInfoNonVR;
            InfoText.text = CurrentTask == Task.Writing
                ? timeOver ? string.Concat("<b>Time over</b> for <b>writing task</b>.",
                              Environment.NewLine, "Next task will be <b>text editing</b>.") :
                string.Concat("Great! \nYou finished <b>writing task</b>" + ".",
                              Environment.NewLine, "Next task will be <b>text editing</b>.")
                : IsVR
                ? timeOver ? string.Concat("<b>Time over</b> for <b>editing task</b> ",
                              Environment.NewLine, "You can take off the Headset.") :
                string.Concat("Great! \nYou finished <b>editing task</b> and condition <b>", GlobalSettings.Condition, "</b>.",
                              Environment.NewLine, "You can take off the Headset.")
                : timeOver ? string.Concat("<b>Time over</b> for <b>editing task</b>.") :
                string.Concat("Great! \nYou finished <b>editing task</b> and condition <b>", GlobalSettings.Condition, "</b>.");
        }
        else
        {
            InfoText.color = Color.red;
            InfoText.fontSize = IsVR ? GlobalSettings.FontSizeInfoVR : GlobalSettings.FontSizeInfoNonVR;
            InfoText.text = timeOver ? string.Concat("<b>Time over</b> for condition <b>", GlobalSettings.Condition, "</b>\nYou are done with all tasks") :
                string.Concat("Great! \nYou finished all tasks and conditions!");
        }
    }

    private IEnumerator OnTaskFinishedAfter(int type)
    {
        yield return new WaitForSeconds(4);
        switch (type)
        {
            case 0:
                GlobalSettings.TextPackageNumber = GlobalSettings.TextPackageNumber + 1;
                GlobalSettings.NumberOfFinishedConditions++;
                OnConditionFinished();
                break;
            case 1:
                OnAllTasksFinished();
                break;
            case 2:
                SetupComponentsForTask();
                break;
        }
    }

    public void OnComponentsInitialized()
    {
        GlobalSettings.GlobalBlock = false;
    }
}

public interface IDelegator
{
    void OnTaskFinished(bool timeOver);
    void OnComponentsInitialized();
}
