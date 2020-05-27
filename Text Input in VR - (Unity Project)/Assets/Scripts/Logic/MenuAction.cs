using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuAction : MonoBehaviour {

    public InputField InputUserID;
    public Text UserIDConfirm;
    public Text ConditionText;
    public Text Notes;
    public Button VRVideoCropButton;
    public Button VR3DButton;
    public Button SoftkeyboardButton;
    public Button HardkeyboardButton;
    public Button KeyboardConfirmButton;
    public Button UserIDSubmit;
    protected bool UserIDSet;
    protected IAppDelegator appDelegator;
    public InputField BackdoorField;
    public Toggle BackdoorToggle;
    protected bool IsBackdoor;

    #region PUBLIC_METHODS
    public void OnStartSceneVRVideoCrop()
    {
        if (IsBackdoor && BackdoorField.text.Length == 0) return;
        if (IsBackdoor) 
        {
            GlobalSettings.TextPackageNumber = int.Parse(BackdoorField.text);
            GlobalSettings.CurrentTask = BackdoorToggle.isOn ? Task.Writing : Task.Editing;
            GlobalSettings.CurrentWriting = BackdoorToggle.isOn;
        }
        if (!UserIDSet) { StartCoroutine(NoUserIDSet()); return; }
        GlobalSettings.Condition = Condition.VR_VideoCrop;
        GlobalSettings.CurrentScene = "VRKeyboardScene";
        KeyboardConfirmButton.gameObject.SetActive(true);
        SceneManager.LoadScene(GlobalSettings.CurrentScene);
    }
    public void OnStartSceneVR3DKeyboard()
    {
        if (IsBackdoor && BackdoorField.text.Length == 0) return;
        if (IsBackdoor)
        {
            GlobalSettings.TextPackageNumber = int.Parse(BackdoorField.text);
            GlobalSettings.CurrentTask = BackdoorToggle.isOn ? Task.Writing : Task.Editing;
            GlobalSettings.CurrentWriting = BackdoorToggle.isOn;
        }
        if (!UserIDSet) { StartCoroutine(NoUserIDSet()); return; }
        GlobalSettings.Condition = Condition.VR_No_Hands_No_Keyboard;
        GlobalSettings.CurrentScene = "VRKeyboardScene";
        KeyboardConfirmButton.gameObject.SetActive(true);
        SceneManager.LoadScene(GlobalSettings.CurrentScene);
    }
    public void OnStartSceneSoftkeyboard()
    {
        if (IsBackdoor && BackdoorField.text.Length == 0) return;
        if (IsBackdoor)
        {
            GlobalSettings.TextPackageNumber = int.Parse(BackdoorField.text);
            GlobalSettings.CurrentTask = BackdoorToggle.isOn ? Task.Writing : Task.Editing;
            GlobalSettings.CurrentWriting = BackdoorToggle.isOn;
        }
        if (!UserIDSet) { StartCoroutine(NoUserIDSet()); return; }
        GlobalSettings.Condition = Condition.Softkeyboard_Phone;
        GlobalSettings.CurrentScene = "SoftKeyboardScene";
        KeyboardConfirmButton.gameObject.SetActive(true);
        SceneManager.LoadScene(GlobalSettings.CurrentScene);
    }
    public void OnStartSceneHardKeyboard()
    {
        if (IsBackdoor && BackdoorField.text.Length == 0) return;
        if (IsBackdoor)
        {
            GlobalSettings.TextPackageNumber = int.Parse(BackdoorField.text);
            GlobalSettings.CurrentTask = BackdoorToggle.isOn ? Task.Writing : Task.Editing;
            GlobalSettings.CurrentWriting = BackdoorToggle.isOn;
        }
        if (!UserIDSet) { StartCoroutine(NoUserIDSet()); return; }
        GlobalSettings.Condition = Condition.Hardkeyboard_Phone;
        GlobalSettings.CurrentScene = "HardKeyboardScene";
        KeyboardConfirmButton.gameObject.SetActive(true);
        SceneManager.LoadScene(GlobalSettings.CurrentScene);
    }
    #endregion // PUBLIC_METHODS

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        if (GlobalSettings.InGame)
        {
            UserIDConfirm.text = string.Concat("User ID set to: ", GlobalSettings.UserID);
            UserIDSet = true;
            PrepareTextsForCondition();
        }
        //KeyboardConfirmButton.gameObject.SetActive(false);
        InputUserID.onValueChanged.AddListener(OnParticipantIDReset);
        VRVideoCropButton.gameObject.SetActive(false);
        VR3DButton.gameObject.SetActive(false);
        SoftkeyboardButton.gameObject.SetActive(false);
        HardkeyboardButton.gameObject.SetActive(false);
        KeyboardConfirmButton.gameObject.SetActive(false);
        BackdoorField.gameObject.SetActive(false);
        BackdoorToggle.gameObject.SetActive(false);
        SetupConditionButtons();
    }

    private void Update()
    {
        if (GlobalSettings.Condition == Condition.Hardkeyboard_Phone
            || GlobalSettings.Condition == Condition.VR_No_Hands_No_Keyboard
            || GlobalSettings.Condition == Condition.VR_VideoCrop)
        {
            KeyboardConfirmButton.gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ConfirmKeyboardOnOff();
            }
        }
        else
        {
            KeyboardConfirmButton.gameObject.SetActive(true);
        }
    }

    public void SetUserID()
    {
        GlobalSettings.UserID = InputUserID.text;
        UserIDConfirm.text = string.Concat("User ID set to: ", GlobalSettings.UserID);
        UserIDSet = true;
        appDelegator.OnUserIDSet();
        UserIDSubmit.enabled = false;
        UserIDSubmit.interactable = false;
    }

    public void ConfirmKeyboardOnOff()
    {
        EnableConditionButton();
    }

    public void PrepareTextsForCondition()
    {
        Notes.color = Color.red;
        KeyboardConfirmButton.gameObject.SetActive(true);
        KeyboardConfirmButton.enabled = true;
        KeyboardConfirmButton.interactable = true;

        switch (GlobalSettings.Condition)
        {
            case Condition.VR_VideoCrop:
                VRVideoCropButton.interactable = false;
                VR3DButton.interactable = false;
                SoftkeyboardButton.interactable = false;
                HardkeyboardButton.interactable = false;
                Notes.text = "Confirm turned ON Bluetooth Keyboard \nby pressing <b>ENTER</b>";
                break;
            case Condition.VR_No_Hands_No_Keyboard:
                VRVideoCropButton.interactable = false;
                VR3DButton.interactable = false;
                SoftkeyboardButton.interactable = false;
                HardkeyboardButton.interactable = false;
                Notes.text = "Confirm turned ON Bluetooth Keyboard \nby pressing <b>ENTER</b>";
                break;
            case Condition.Softkeyboard_Phone:
                VRVideoCropButton.interactable = false;
                VR3DButton.interactable = false;
                SoftkeyboardButton.interactable = false;
                HardkeyboardButton.interactable = false;
                Notes.text = "Confirm turned OFF Bluetooth Keyboard \nby pressing <b>Confirm</b>";
                break;
            case Condition.Hardkeyboard_Phone:
                VRVideoCropButton.interactable = false;
                VR3DButton.interactable = false;
                SoftkeyboardButton.interactable = false;
                HardkeyboardButton.interactable = false;
                Notes.text = "Confirm turned ON Bluetooth Keyboard \nby pressing <b>ENTER</b>";
                break;
        }
        ConditionText.text = string.Concat("Next Condition: ", GlobalSettings.Condition);
    }

    public void SetListener(IAppDelegator l)
    {
        this.appDelegator = l;
    }

    public void EnableConditionButton()
    {
        Notes.color = Color.green;
        switch (GlobalSettings.Condition)
        {
            case Condition.VR_VideoCrop:
                VRVideoCropButton.interactable = true;
                Notes.text = "Turned ON Bluetooth Keyboard";
                break;
            case Condition.VR_No_Hands_No_Keyboard:
                VR3DButton.interactable = true;
                Notes.text = "Turned ON Bluetooth Keyboard";
                break;
            case Condition.Softkeyboard_Phone:
                SoftkeyboardButton.interactable = true;
                Notes.text = "Turned OFF Bluetooth Keyboard";
                break;
            case Condition.Hardkeyboard_Phone:
                HardkeyboardButton.interactable = true;
                Notes.text = "Turned ON Bluetooth Keyboard";
                break;
        }
    }

    public void OnFinishedTasks()
    {
        ConditionText.text = string.Concat("SUPER! You finished all tasks!");
    }

    public void OnUserIdError(string id)
    {
        UserIDConfirm.text = string.Concat(id, " is an invalid User ID.");
        UserIDSubmit.enabled = true;
        UserIDSubmit.interactable = true;
    }

    private void SetupConditionButtons()
    {
        foreach (Condition condition in GlobalSettings.conditions) 
        {
            ShowConditionButton(condition);
        }
    }

    private void ShowConditionButton(Condition condition)
    {
        switch (condition)
        {
            case Condition.VR_VideoCrop:
                VRVideoCropButton.gameObject.SetActive(true);
                break;
            case Condition.Hardkeyboard_Phone:
                HardkeyboardButton.gameObject.SetActive(true);
                break;
            case Condition.Softkeyboard_Phone:
                SoftkeyboardButton.gameObject.SetActive(true);
                break;
            case Condition.VR_No_Hands_No_Keyboard:
                VR3DButton.gameObject.SetActive(true);
                break;
        }
    }

    private void OnParticipantIDReset(string val)
    {
        UserIDSubmit.enabled = true;
        UserIDSubmit.interactable = true;
    }

    private IEnumerator NoUserIDSet()
    {
        UserIDConfirm.text = "No User ID set.";
        yield return new WaitForSeconds(3);
        UserIDConfirm.text = "";
    }

    public void BackdoorFunction()
    {
        IsBackdoor = true;
        VRVideoCropButton.gameObject.SetActive(true);
        SoftkeyboardButton.gameObject.SetActive(true);
        HardkeyboardButton.gameObject.SetActive(true);
        KeyboardConfirmButton.gameObject.SetActive(true);
        VRVideoCropButton.interactable = true;
        SoftkeyboardButton.interactable = true;
        HardkeyboardButton.interactable = true;
        KeyboardConfirmButton.interactable = true;
        BackdoorField.gameObject.SetActive(true);
        BackdoorToggle.gameObject.SetActive(true);
    }

    public void TestVR()
    {
        SceneManager.LoadScene("VRKeyboardScene");
    }
}
