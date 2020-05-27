using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour, IControllerLogic
{
    public enum Mode
    {
        Writing_VR_VideoCrop,
        Writing_SoftKeyboard,
        Writing_HardKeyboard,
        Writing_VR_3D,
        Editing_VR_VideoCrop,
        Editing_SoftKeyboard,
        Editing_HardKeyboard,
        Editing_VR_3D
    }
    public Mode mode = new Mode();

    protected IDelegator delegator;
    private bool inputActionAllowed;
    private bool gameRunning = false;
    private bool setPaused = false;
    private bool initHold = false;
    private int InputMode;
    protected bool IsWriting;
    protected bool IsHardKeyboard;
    protected GameObject WritingField;
    protected GameObject EditingField;
    protected TextEditController TextEditController;
    protected TextInputController TextInputController;
    protected TextInputControllerSoft TextInputControllerSoft;
    protected GameObject PhrasesComponent;
    protected GameObject StartSoftButton;
    protected GameObject StartButton;
    protected CountDown CountDown;
    protected Phrases Phrases;
    protected EditSentences EditSentences;
    protected GameTimer GameTimer;
    protected Logging Logging;
    protected int PlayedRuns = 0;
    protected int PlayedSets = 0;

    protected static int NUM_RUNS_WRITING = GlobalSettings.NumberOfRunsForWriting;    
    protected static int NUM_RUNS_EDITING = GlobalSettings.NumberOfRunsForEditing;      
    protected static int NUMBER_OF_RUNS = 0;    

    public void ResetVariables()
    {
        PlayedRuns = 0;
        PlayedSets = 0;
    }

    public void Setup(IDelegator d)
    {
        this.delegator = d;
        GlobalSettings.CurrentRound = 0;
        SetupComponents();
        switch (mode)
        {
            case Mode.Writing_VR_VideoCrop:
                InputMode = 0;
                WritingField = GameObject.FindWithTag("WritingField");
                TextInputController = WritingField.GetComponent<TextInputController>();
                Phrases = GetComponent<Phrases>();
                TextInputController.SetListener(this, Phrases);
                GlobalSettings.IsCurrentSceneVR = true;
                GameObject.Find("ARCamera").GetComponent<PitchYawRoll>().SetListener(Logging);
                break;
            case Mode.Writing_SoftKeyboard:
                InputMode = 1;
                WritingField = GameObject.FindWithTag("WritingField");
                TextInputControllerSoft = WritingField.GetComponent<TextInputControllerSoft>();
                StartSoftButton = GameObject.Find("StartSoftButton");
                Phrases = GetComponent<Phrases>();
                GlobalSettings.IsCurrentSceneVR = false;
                break;
            case Mode.Writing_HardKeyboard:
                InputMode = 2;
                WritingField = GameObject.FindWithTag("WritingField");
                TextInputController = WritingField.GetComponent<TextInputController>();
                Phrases = GetComponent<Phrases>();
                //StartButton = GameObject.Find("StartButton");
                initHold = true;
                TextInputController.SetListener(this, Phrases);
                GlobalSettings.IsCurrentSceneVR = false;
                break;
            case Mode.Writing_VR_3D:
                InputMode = 3;
                WritingField = GameObject.FindWithTag("WritingField");
                TextInputController = WritingField.GetComponent<TextInputController>();
                Phrases = GetComponent<Phrases>();
                TextInputController.SetListener(this, Phrases);
                break;
            case Mode.Editing_VR_VideoCrop:
                InputMode = 4;
                EditingField = GameObject.FindWithTag("EditingField");
                TextEditController = EditingField.GetComponent<TextEditController>();
                TextEditController.SetupListeners(false);
                EditSentences = GetComponent<EditSentences>();
                TextEditController.SetListeners(this, EditSentences, Logging);
                GlobalSettings.IsCurrentSceneVR = true;
                GameObject.Find("ARCamera").GetComponent<PitchYawRoll>().SetListener(Logging);
                break;
            case Mode.Editing_SoftKeyboard:
                InputMode = 5;
                EditingField = GameObject.FindWithTag("EditingField");
                TextEditController = EditingField.GetComponent<TextEditController>();
                StartSoftButton = GameObject.Find("StartSoftButton");
                TextEditController.SetupListeners(true);
                EditSentences = GetComponent<EditSentences>();
                TextEditController.SetListeners(this, EditSentences, Logging);
                GlobalSettings.IsCurrentSceneVR = false;
                break;
            case Mode.Editing_HardKeyboard:
                InputMode = 6;
                EditingField = GameObject.FindWithTag("EditingField");
                TextEditController = EditingField.GetComponent<TextEditController>();
                TextEditController.SetupListeners(false);
                EditSentences = GetComponent<EditSentences>();
                TextEditController.SetListeners(this, EditSentences, Logging);
                GlobalSettings.IsCurrentSceneVR = false;
                break;
            case Mode.Editing_VR_3D:
                InputMode = 7;
                EditingField = GameObject.FindWithTag("EditingField");
                TextEditController = EditingField.GetComponent<TextEditController>();
                EditSentences = GetComponent<EditSentences>();
                TextEditController.SetListeners(this, EditSentences, Logging);
                break;
        }
        setPaused = false;
        SetGlobalSettings();
        InitListeners();
        CountDown.SetInputMode(InputMode);
        IsWriting = InputMode < 4;
        IsHardKeyboard = InputMode != 1 && InputMode != 5;
        CountDown.IsWriting = IsWriting;
        CountDown.Reset(3);
        NUMBER_OF_RUNS = IsWriting ? NUM_RUNS_WRITING : NUM_RUNS_EDITING;
        delegator.OnComponentsInitialized();
    }

    void SetupComponents()
    {
        CountDown = GetComponent<CountDown>();
        GameTimer = GetComponent<GameTimer>();
        Logging = GetComponent<Logging>();
        SetupLogging();
    }

    void SetupLogging()
    {
        Logging.Setup(GlobalSettings.UserID, mode); 
    }

    void InitListeners()
    {
        if (mode == Mode.Writing_SoftKeyboard)
        {
            WritingField.GetComponent<TextInputControllerSoft>().SetListener(this, Phrases, Logging);
        }
        if (InputMode > 4) EditSentences.SetListenerControllerLogic(this);
        CountDown.SetListener(this);
        GameTimer.SetListener(this);
    }

    void Update()
    {

        GlobalSettings.InTask = gameRunning;
        if (Input.GetKeyDown(KeyCode.Return) && GlobalSettings.GlobalBlock) return;
        if (Input.GetKeyDown(KeyCode.Return) && !gameRunning && !setPaused)
        {
            StartCoroutine(CountDown.CountdownFunction());
            GlobalSettings.GlobalBlock = true;
            inputActionAllowed = false;
            gameRunning = false;
        }
        else if (Input.GetKeyDown(KeyCode.Return) && setPaused)
        {
            // Set was paused, start next run
            GlobalSettings.GlobalBlock = true;
            setPaused = false;
            StartCoroutine(CountDown.CountdownFunction());
        }
    }

    public void StartButtonFunction()
    {
        StartCoroutine(CountDown.CountdownFunction());
        inputActionAllowed = false;
        gameRunning = false;
        StartButton.SetActive(false);
    }

    public void StartSoftMode()
    {
        StartCoroutine(CountDown.CountdownFunction());
        inputActionAllowed = false;
        gameRunning = false;
        StartSoftButton.SetActive(false);
    }

    public void CountDownEnd()
    {
        GlobalSettings.GlobalBlock = false;
        inputActionAllowed = true;
        gameRunning = !gameRunning;
        if (GlobalSettings.CurrentRound == 1) GameTimer.SetGameRunning(gameRunning);
        GameTimer.SetRunRunning(true);
        Logging.SetLoggingAllowed(true);
        switch (mode)
        {
            case Mode.Writing_VR_VideoCrop:
                WritingField.GetComponent<TextInputController>().SetInputAllowed(inputActionAllowed); 
                break;
            case Mode.Writing_SoftKeyboard:
                WritingField.GetComponent<TextInputControllerSoft>().SetInputAllowed(inputActionAllowed); 
                break;
            case Mode.Writing_HardKeyboard:
                WritingField.GetComponent<TextInputController>().SetInputAllowed(inputActionAllowed);
                break;
            case Mode.Writing_VR_3D:
                WritingField.GetComponent<TextInputController>().SetInputAllowed(inputActionAllowed);
                break;
            case Mode.Editing_VR_VideoCrop:
                EditingField.GetComponent<TextEditController>().SetInputAllowed(inputActionAllowed);
                break;
            case Mode.Editing_SoftKeyboard:
                EditingField.GetComponent<TextEditController>().SetInputAllowed(inputActionAllowed);
                break;
            case Mode.Editing_HardKeyboard:
                EditingField.GetComponent<TextEditController>().SetInputAllowed(inputActionAllowed);
                break;
            case Mode.Editing_VR_3D:
                EditingField.GetComponent<TextEditController>().SetInputAllowed(inputActionAllowed);
                break;
        }
        if (InputMode < 4)  // Means Writing
        {
            // Writing Taks
            Phrases.gameStarted = gameRunning;
            if (InputMode != 1) { TextInputController.startOfRun = true; TextInputController.SetMode(InputMode); }
            else TextInputControllerSoft.startOfRun = true;
        }
        else 
        {
            // Editing Tasks
            TextEditController.gameStarted = gameRunning;
            TextEditController.SetMode(InputMode);
            TextEditController.startOfRun = true;
        }
    }

    private void SetGlobalSettings()
    {
        UnityEngine.XR.XRSettings.enabled = mode == Mode.Writing_VR_VideoCrop || mode == Mode.Writing_VR_3D || 
            mode == Mode.Editing_VR_VideoCrop || mode == Mode.Editing_VR_3D;
        if (mode == Mode.Writing_SoftKeyboard || mode == Mode.Editing_SoftKeyboard) Screen.orientation = ScreenOrientation.Portrait;
        else if (mode == Mode.Editing_HardKeyboard || mode == Mode.Writing_HardKeyboard) Screen.orientation = ScreenOrientation.Landscape;
    }

    public void OnTextFinished(string original, string transcribed)
    {

        if (IsWriting)
        {
            if (mode != Mode.Writing_SoftKeyboard)
                Logging.LogTextInfo(original, TextInputController.GetInputString());
            else
                Logging.LogTextInfo(original, transcribed);
        }
        else 
        {
            Logging.LogTextInfo(original, TextEditController.GetInputFieldText());
        }
    }

    public void OnTextFinished()
    {
        //WritingField.GetComponent<TextInputControllerSoft>().SetInputAllowed(inputActionAllowed);
    }

    // A Run of n phrases/texts finished
    public void RunFinished()
    {
        PlayedRuns++;
        Debug.Log("RunFinished - " + PlayedRuns + " RUNS of " + NUMBER_OF_RUNS + " finished ");
        GameTimer.ResetRunTimer();
        if (PlayedRuns == NUMBER_OF_RUNS)        // Set n of NUMBER_OF_SETS is finished --> probably 1
        {
            Logging.LogPerformanceData(true);
            Logging.SetLoggingAllowed(false);
            gameRunning = false;
            setPaused = true;
            SetFinished();
        }
        else                                    // Run n of NUMBER_OF_RUNS is finished, start new Run
        {
            if (IsWriting) 
            {
                Logging.LogPerformanceData(false);
                StartCoroutine(PauseSet());
                if (InputMode != 1) TextInputController.SetInputAllowed(false);
                else TextInputControllerSoft.SetInputAllowed(false);
                Phrases.Init();
            }
            else 
            {
                Logging.LogPerformanceData(false);
                StartCoroutine(PauseSet());
                TextEditController.gameStarted = false;
                TextEditController.SetInputAllowed(false);
                TextEditController.ResetTextCount();
            }
            GlobalSettings.CurrentRound += 1;
            StartCoroutine(ResetFunction());
        }
    }

    private IEnumerator PauseSet()
    {
        yield return new WaitForSeconds(2);
        setPaused = true;
        Logging.SetLoggingAllowed(false);
        inputActionAllowed = !inputActionAllowed;
        gameRunning = false;
    }

    // A Set of n Runs finished
    private void SetFinished()
    {
        if (!IsWriting)
        {
            TextEditController.gameStarted = false;
            TextEditController.SetInputAllowed(false);
            TextEditController.InputField.ManualDeselect();
        }
        delegator.OnTaskFinished(false);
    }


    // Time for writing or editing run over
    public void OnRunTimeOver()
    {
        Logging.CurrentRunTimeOver = true;
        if (IsWriting)
        {
            if (IsHardKeyboard)
            {
                TextInputController.OnFinished(true);
            }
            else
            {
                TextInputControllerSoft.OnFinished(true);
            }
        }
        else
        {
            CountDown.Reset(2);
            TextEditController.OnTimeOverForRun();
        }
        GameTimer.SetRunRunning(false);
        GameTimer.ResetRunTimer();
    }

    public IEnumerator ResetFunction()
    {
        CountDown.Reset(1);
        if (IsWriting && InputMode == 1 || InputMode == 5)
        {
            StartSoftButton.SetActive(true); TextInputControllerSoft.originalText.text = "";
        }
        else if (IsWriting && InputMode == 0 || InputMode == 3)
        {
            TextInputController.originalText.text = "";
        }
        yield return new WaitForSeconds(3);
    }
}

public interface IControllerLogic 
{
    void CountDownEnd();
    void OnTextFinished();
    void OnTextFinished(string original, string transcribed);
    void RunFinished();
    //void OnTimeOver();
    void OnRunTimeOver();
}
