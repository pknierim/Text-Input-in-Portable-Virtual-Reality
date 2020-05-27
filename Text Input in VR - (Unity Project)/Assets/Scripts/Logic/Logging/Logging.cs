using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Mode = MainController.Mode;

public class Logging : MonoBehaviour, ILogging
{
    // Path and file info
    protected static string BasePath;
    protected static string LoggingFolder = "Logging";
    protected static string WritingFolder = "Writing";
    protected static string EditingFolder = "Editing";
    protected static string _ = "_";
    protected static string C = ",";
    protected static string CurrentLoggingFolderPath;
    protected string KeystrokesLogPath;
    protected string KeystrokesWritingLogFileName;
    protected string TextsLogPath;
    protected string TextsLogFileName;
    protected string TextsPerformanceLogPath;
    protected string TextsPerformanceLogFileName;
    protected string SelectionsLogFileName;
    protected string SelectionsLogPath;
    protected string PitchYawRollLogFileName;
    protected string PitchYawRollLogPath;
    protected string UserID;
    protected string Today;
    protected string CurrentTime;
    protected Mode Mode;
    protected bool IsWriting;

    // Logging contents
    protected string InputText = "";
    protected int SentenceCounter;
    protected int InputCounterPerText;
    protected int InputCountPerSet;
    protected int ArrowKeysCountPerSet;     
    protected int DeleteKeyCountPerSet;     
    protected int CursorPlacingCountPerSet; 
    protected int TextSelectionCountPerSet;
    protected int ScrollActionCountPerSet;
    protected int TotalArrowKeysPerTask;
    protected int TotalDeleteKeysPerTask;
    protected int TotalCursorPlacingsPerTask;
    protected int TotalTextSelectionsPerTask;
    protected int TotalScrollActionCountPerTask;
    protected float TimeSinceTaskStart; // Time for whole Task (e.g. VR Crop Video Writing - 3x10)
    protected float TimeSinceSetStart; // Time for 1 set (e.g. 10 sentences), will be set after finishing a set
    protected float TimeSinceTextStart; // Time for a sentence/text, will be set after each sentence/text
    protected float TimeToFirstKeyPress;
    protected float TimeSinceUserActionStart;
    protected bool HardkeyboardMode;
    protected bool LoggingAllowed;
    protected bool IsFirstKeyPressOfRound;
    protected bool UserActionTimerOn;
    protected char[] delimiters;
    protected char nextCorrectChar;
    protected bool correctInput;
    protected bool keyUp;
    // Performance logging
    protected int TotalWords;
    protected int TotalCorrectWords;
    protected int TotalInputLength;
    protected float WPMGross;
    protected float WPMGross1;
    protected float WPMGross2;
    protected float WPMCorrect;
    protected bool SoftSubmitted = false;
    protected int RoundCounter;
    protected int FrameCounter;
    public bool CurrentRunTimeOver;

    // Called everytime a new task withing a condition is started
    public void Setup(string id, Mode mode)
    {
        Today = DateTime.Today.ToString("dd-MM-yy");
        CurrentTime = DateTime.Now.ToString("hh-mm-ss");
        BasePath = Path.Combine(Application.persistentDataPath, LoggingFolder);
        if (!Directory.Exists(BasePath))
        {
            Directory.CreateDirectory(BasePath);
        }
        this.UserID = id;
        this.Mode = mode;
        CreateDir();
        SentenceCounter = 0;
        TimeSinceTaskStart = 0;
        TimeSinceSetStart = 0;
        TimeSinceTextStart = 0;
        TimeSinceUserActionStart = 0;
        TimeToFirstKeyPress = 0;
        InputCounterPerText = 0;
        InputCountPerSet = 0;
        ArrowKeysCountPerSet = 0;
        DeleteKeyCountPerSet = 0;
        CursorPlacingCountPerSet = 0;
        TextSelectionCountPerSet = 0;
        ScrollActionCountPerSet = 0;
        TotalWords = 0;
        TotalCorrectWords = 0;
        TotalInputLength = 0;
        WPMGross = 0;
        WPMGross1 = 0;
        WPMGross2 = 0;
        WPMCorrect = 0;
        RoundCounter = 0;
        LoggingAllowed = false;
        UserActionTimerOn = false;
        IsFirstKeyPressOfRound = true;
        HardkeyboardMode = Mode != Mode.Writing_SoftKeyboard && Mode != Mode.Editing_SoftKeyboard;
        IsWriting = Mode == Mode.Writing_VR_3D || Mode == Mode.Writing_HardKeyboard || Mode == Mode.Writing_SoftKeyboard || Mode == Mode.Writing_VR_VideoCrop;
        delimiters = new char[] { ' ', '\r', '\n' };
        // Log files and paths
        TextsPerformanceLogFileName = String.Concat(UserID, _, mode.ToString(), _, Today, _, CurrentTime, _, "texts_performance_data.txt");
        TextsPerformanceLogPath = Path.Combine(CurrentLoggingFolderPath, TextsPerformanceLogFileName);
        TextsLogFileName = String.Concat(UserID, _, mode.ToString(), _, Today, _, CurrentTime, _, "texts.txt");
        TextsLogPath = Path.Combine(CurrentLoggingFolderPath, TextsLogFileName);
        SelectionsLogFileName = String.Concat(UserID, _, mode.ToString(), _, Today, _, CurrentTime, _, "keystrokes_selections.txt");
        SelectionsLogPath = Path.Combine(CurrentLoggingFolderPath, SelectionsLogFileName);
        KeystrokesWritingLogFileName = String.Concat(UserID, _, mode.ToString(), _, Today, _, CurrentTime, _, "keystrokes.txt");
        KeystrokesLogPath = IsWriting ? Path.Combine(CurrentLoggingFolderPath, KeystrokesWritingLogFileName) :
                                            Path.Combine(CurrentLoggingFolderPath, SelectionsLogFileName);
        PitchYawRollLogFileName = String.Concat(UserID, _, mode.ToString(), _, Today, _, CurrentTime, _, "pitch_yaw_roll.txt");
        PitchYawRollLogPath = Path.Combine(CurrentLoggingFolderPath, PitchYawRollLogFileName);
    }

    public void SetLoggingAllowed(bool b)
    {
        this.LoggingAllowed = b;
        if (!b) ResetSetMeasures();
        else TimeSinceTextStart = 0;
    }

    private void ResetSetMeasures()
    {
        Debug.Log("LOGGING - RESET:" + Environment.NewLine +
                  "ArrowKeysCountPerSet: " + ArrowKeysCountPerSet + ", " + Environment.NewLine +
                  "DeleteKeyCountPerSet: " + DeleteKeyCountPerSet + ", " + Environment.NewLine +
                  "CursorPlacingCountPerSet: " + CursorPlacingCountPerSet + ", " + Environment.NewLine +
                  "TextSelectionCountPerSet: " + TextSelectionCountPerSet + ", " + Environment.NewLine +
                  "ScrollActionCountPerSet: " + ScrollActionCountPerSet + Environment.NewLine +
                  "TotalArrowKeysPerTask: " + TotalArrowKeysPerTask + ", " + Environment.NewLine +
                  "TotalDeleteKeysPerTask: " + TotalDeleteKeysPerTask + ", " + Environment.NewLine +
                  "TotalCursorPlacingsPerTask: " + TotalCursorPlacingsPerTask + ", " + Environment.NewLine +
                  "TotalTextSelectionsPerTask: " + TotalTextSelectionsPerTask + ", " + Environment.NewLine +
                  "TotalScrollActionCountPerTask: " + TotalScrollActionCountPerTask);
        TimeSinceSetStart = 0;
        InputCountPerSet = 0;
        ArrowKeysCountPerSet = 0;
        DeleteKeyCountPerSet = 0;
        CursorPlacingCountPerSet = 0;
        TextSelectionCountPerSet = 0;
        ScrollActionCountPerSet = 0;
    }

    private void ResetTextTimer()
    {
        TimeSinceTextStart = 0;
        TimeSinceUserActionStart = 0;
        InputCounterPerText = 0;
        if (!HardkeyboardMode && SoftSubmitted)
        {
            SoftSubmitted = !SoftSubmitted;
        }
        UserActionTimerOn = false;
    }

    private void ResetSetTimer()
    {
        TotalInputLength = 0;
        TotalWords = 0;
        TotalCorrectWords = 0;
        IsFirstKeyPressOfRound = true;
        CurrentRunTimeOver = false;
        InputText = "";
    }

    void Update()
    {
        FrameCounter++;
        if (GlobalSettings.IsCurrentSceneVR && (FrameCounter % 10 == 0))
        {

        }
        if (LoggingAllowed)
        {
            //TimeSinceTaskStart += Time.deltaTime;
            TimeSinceSetStart += Time.deltaTime;
            TimeSinceTextStart += Time.deltaTime;
            if (HardkeyboardMode) LogHardKeyStroke();
            if (UserActionTimerOn) TimeSinceUserActionStart += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Return) && UserActionTimerOn)
            {
                UserActionTimerOn = false;
                //ResetTextTimer();
            }
            keyUp &= !Input.anyKey;
            if (!HardkeyboardMode && SoftSubmitted)
            {
                ResetTextTimer();
            }
        }
    }

    // Called after each finished sentence / text
    public void LogTextInfo(string OriginalString, string InputString)
    {
        //ResetTextTimer();
        RoundCounter++;
        if (IsWriting)
        {
            InputText = HardkeyboardMode ? InputText : InputString;
            string textLogEntry = String.Concat(InputText, C, OriginalString, C, TimeSinceUserActionStart.ToString("F2"), C,
                                                TimeSinceTextStart.ToString("F2"), C, TimeSinceSetStart.ToString("F2"), C, 
                                                TimeSinceTaskStart.ToString("F2"), C, InputCounterPerText, C, DateTime.Now.ToString("hh:mm:ss"), Environment.NewLine);
            File.AppendAllText(TextsLogPath, textLogEntry);
            CalcPerformanceData(OriginalString);
            InputText = "";
        }
        else 
        {
            string textLogEntry = String.Concat(InputString, C, OriginalString, C, TimeSinceUserActionStart.ToString("F2"), C,
                                                TimeSinceTextStart.ToString("F2"), C, TimeSinceSetStart.ToString("F2"), C, 
                                                TimeSinceTaskStart.ToString("F2"), C, InputCounterPerText, C, DateTime.Now.ToString("hh:mm:ss"), Environment.NewLine);
            File.AppendAllText(TextsLogPath, textLogEntry);
        }
        File.AppendAllText(KeystrokesLogPath, "END_" + RoundCounter.ToString() + Environment.NewLine);
        ResetTextTimer();
    }

    // Optional: Calculate Performance measures like WPM at runtime and log them
    private void CalcPerformanceData(string Original)
    {
        var wordsInputString = InputText.Split(delimiters).ToList();
        var wordsOriginalString = Original.Split(delimiters).ToList();
        var currInputTextCount = wordsInputString.Count;
        var currOriginalTextCount = wordsOriginalString.Count;
        int correctWords = currOriginalTextCount;
        if (currInputTextCount > currOriginalTextCount)
        {
            for (int i = 0; i < (currInputTextCount - currOriginalTextCount); i++)
            {
                wordsOriginalString.Add("****");
            }
        }
        if (currInputTextCount < currOriginalTextCount)
        {
            for (int i = 0; i < (currOriginalTextCount - currInputTextCount); i++)
            {
                wordsInputString.Add("****");
            }
        }

        for (int i = 0; i < currOriginalTextCount; i++)
        {
            if (wordsOriginalString[i] != wordsInputString[i])
            {
                //this is needed so that extra words at the end of the input string do not count as wrong words(correctWordsPerPhrase--)
                //correctWords = wordsOriginalString[i] != "****" ? correctWords-- : correctWords;
                if (wordsOriginalString[i] != "****")
                {
                    correctWords--;
                }
            }
        }

        TotalWords += InputText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        TotalCorrectWords += correctWords;
        TotalInputLength += InputText.Length;
        WPMGross = (TotalWords / (TimeSinceSetStart - TimeToFirstKeyPress)) * 60;                   // Words are seperated by space
        WPMGross1 = ((InputCountPerSet / 5) / (TimeSinceSetStart - TimeToFirstKeyPress)) * 60;      // Words are calculated by the counter
        WPMCorrect = (TotalCorrectWords / (TimeSinceSetStart - TimeToFirstKeyPress)) * 60;          // Words are only correct words
        WPMGross2 = (TotalInputLength / 5) / (TimeSinceSetStart - TimeToFirstKeyPress) * 60;        // Word is 5 chars long WITH Spaces
    }

    // Logging of performance data like WPM, input count etc.
    public void LogPerformanceData(bool EndOfRun)
    {
        TotalArrowKeysPerTask += ArrowKeysCountPerSet;
        TotalDeleteKeysPerTask += DeleteKeyCountPerSet;
        TotalCursorPlacingsPerTask += CursorPlacingCountPerSet;
        TotalTextSelectionsPerTask += TextSelectionCountPerSet;
        TotalScrollActionCountPerTask += ScrollActionCountPerSet;
        TimeSinceTaskStart += TimeSinceSetStart;

        if (CurrentRunTimeOver) File.AppendAllText(KeystrokesLogPath, "END_TIME_OVER" + Environment.NewLine);

        int RoundCounterEnd = 0;

        if (IsWriting)
        {
            if (RoundCounter % 10 == 0)
            {
                RoundCounterEnd = RoundCounter / GlobalSettings.NumberOfPhrasesForWriting;
            }
            else if (RoundCounter < 10)
            {
                RoundCounterEnd = 1;
            }
            else if (RoundCounter < 20)
            {
                RoundCounterEnd = 2;
            }
            else if (RoundCounter < 30)
            {
                RoundCounterEnd = 3;
            }
        }
        else 
        {
            RoundCounterEnd = RoundCounter / GlobalSettings.NumberOfTextsForEditing;
        }

        string textLogEntryFinal = IsWriting
            ? EndOfRun ? String.Concat("wpm_gross: ", WPMGross, Environment.NewLine,
                                                         "wpm_gross1: ", WPMGross1, Environment.NewLine,
                                                         "wpm_gross2: ", WPMGross2, Environment.NewLine,
                                                         "wpm_correct: " + WPMCorrect, Environment.NewLine,
                                                         "total_inputs: " + InputCountPerSet, Environment.NewLine,
                                                         "total_words: " + TotalWords, Environment.NewLine,
                                                         "set_completion_time: " + TimeSinceSetStart.ToString("F2"), Environment.NewLine,
                                                         "END_" + RoundCounterEnd.ToString(), Environment.NewLine,
                                                         "task_completion_time: " + TimeSinceTaskStart.ToString("F2"), Environment.NewLine) :
                                           String.Concat("wpm_gross: ", WPMGross, Environment.NewLine,
                                                         "wpm_gross1: ", WPMGross1, Environment.NewLine,
                                                         "wpm_gross2: ", WPMGross2, Environment.NewLine,
                                                         "wpm_correct: " + WPMCorrect, Environment.NewLine,
                                                         "total_inputs: " + InputCountPerSet, Environment.NewLine,
                                                         "total_words: " + TotalWords, Environment.NewLine,
                                                         "set_completion_time: " + TimeSinceSetStart.ToString("F2"), Environment.NewLine,
                                                         "END_" + RoundCounterEnd.ToString(), Environment.NewLine)
            : HardkeyboardMode
                ? EndOfRun ? String.Concat("total_inputs: " + InputCountPerSet, Environment.NewLine,
                                           "arrowkey_inputs: " + ArrowKeysCountPerSet, Environment.NewLine,
                                           "delete_inputs: " + DeleteKeyCountPerSet, Environment.NewLine,
                                           "scroll_actions: " + ScrollActionCountPerSet, Environment.NewLine,
                                           "text_selections: " + TextSelectionCountPerSet, Environment.NewLine,
                                           "set_completion_time: " + TimeSinceSetStart.ToString("F2"), Environment.NewLine,
                                           "END_" + RoundCounterEnd.ToString(), Environment.NewLine,
                                           "arrowkey_inputs_total: " + TotalArrowKeysPerTask, Environment.NewLine,
                                           "delete_inputs_total: " + TotalDeleteKeysPerTask, Environment.NewLine,
                                           "scroll_actions_total: " + TotalScrollActionCountPerTask, Environment.NewLine,
                                           "text_selections_total: " + TotalTextSelectionsPerTask, Environment.NewLine,
                                           "task_completion_time: " + TimeSinceTaskStart.ToString("F2"), Environment.NewLine) :
                             String.Concat("total_inputs: " + InputCountPerSet, Environment.NewLine,
                                           "arrowkey_inputs: " + ArrowKeysCountPerSet, Environment.NewLine,
                                           "delete_inputs: " + DeleteKeyCountPerSet, Environment.NewLine,
                                           "scroll_actions: " + ScrollActionCountPerSet, Environment.NewLine,
                                           "text_selections: " + TextSelectionCountPerSet, Environment.NewLine,
                                           "set_completion_time: " + TimeSinceSetStart.ToString("F2"), Environment.NewLine,
                                            "END_" + RoundCounterEnd.ToString(), Environment.NewLine)
                : EndOfRun ? String.Concat("total_inputs: " + InputCountPerSet, Environment.NewLine,
                                           "cursor_placings: " + CursorPlacingCountPerSet, Environment.NewLine,
                                           "text_selections: " + TextSelectionCountPerSet, Environment.NewLine,
                                           "delete_inputs: " + DeleteKeyCountPerSet, Environment.NewLine,
                                           "scroll_actions: " + ScrollActionCountPerSet, Environment.NewLine,
                                           "set_completion_time: " + TimeSinceSetStart.ToString("F2"), Environment.NewLine,
                                           "END_" + RoundCounterEnd.ToString(), Environment.NewLine,
                                           "delete_inputs_total: " + TotalDeleteKeysPerTask, Environment.NewLine,
                                           "scroll_actions_total: " + TotalScrollActionCountPerTask, Environment.NewLine,
                                           "text_selections_total: " + TotalTextSelectionsPerTask, Environment.NewLine,
                                           "cursor_placings_total: " + TotalCursorPlacingsPerTask, Environment.NewLine,
                                           "task_completion_time: " + TimeSinceTaskStart.ToString("F2"), Environment.NewLine) :
                             String.Concat("total_inputs: " + InputCountPerSet, Environment.NewLine,
                                           "cursor_placings: " + CursorPlacingCountPerSet, Environment.NewLine,
                                           "delete_inputs: " + DeleteKeyCountPerSet, Environment.NewLine,
                                           "scroll_actions: " + ScrollActionCountPerSet, Environment.NewLine,
                                           "text_selections: " + TextSelectionCountPerSet, Environment.NewLine,
                                           "set_completion_time: " + TimeSinceSetStart.ToString("F2"), Environment.NewLine,
                                            "END_" + RoundCounterEnd.ToString(), Environment.NewLine);

        File.AppendAllText(TextsPerformanceLogPath, textLogEntryFinal);
        if (IsWriting) 
        {
            string end = "END_" + RoundCounterEnd.ToString() + Environment.NewLine;
            File.AppendAllText(TextsLogPath, end);
        }
        else
        {
            string end = "END_" + RoundCounterEnd.ToString() + Environment.NewLine;
            File.AppendAllText(TextsLogPath, end);
        }
        //if (!EndOfRun) File.AppendAllText(KeystrokesLogPath, "END_" + RoundCounter.ToString() + Environment.NewLine);
        ResetSetTimer();
    }

    // Log keystrokes done with the hard keyboard
    private void LogHardKeyStroke()     
    {
        if (!LoggingAllowed) return;
        string keystrokeLogEntry;
        if (HardkeyboardMode)
        {
            foreach (char key in Input.inputString)
            {
                // Ignore number keys
                if (!Input.GetKeyDown(KeyCode.KeypadEnter) && !Input.GetKeyDown(KeyCode.Keypad1) && !Input.GetKeyDown(KeyCode.Keypad2) &&
                    !Input.GetKeyDown(KeyCode.Keypad3) && !Input.GetKeyDown(KeyCode.Keypad6) && !Input.GetKeyDown(KeyCode.Keypad4) &&
                    !Input.GetKeyDown(KeyCode.Keypad5) && !Input.GetKeyDown(KeyCode.Keypad8) && !Input.GetKeyDown(KeyCode.Keypad9))
                {

                    UserActionTimerOn = true;

                    // Happens on beginning of every round
                    if (IsFirstKeyPressOfRound)
                    {
                        TimeToFirstKeyPress = TimeSinceTextStart;
                        IsFirstKeyPressOfRound = false;
                        string firstKeyStrokeLogEntry = String.Concat("first_action_time: ", TimeToFirstKeyPress, Environment.NewLine);
                        File.AppendAllText(TextsPerformanceLogPath, firstKeyStrokeLogEntry);
                    }


                    if (key == "\b"[0])
                    {
                        keystrokeLogEntry = String.Concat("\\backspace", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
                        File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
                        InputCounterPerText = IsWriting ? InputCounterPerText > 0 ? InputCounterPerText-- : 0 : InputCounterPerText++;
                        InputCountPerSet = IsWriting ? InputCountPerSet > 0 ? InputCountPerSet-- : 0 : InputCountPerSet++;
                        DeleteKeyCountPerSet++;
                        if (InputText.Length != 0) InputText = InputText.Substring(0, InputText.Length - 1);
                    }
                    else if (key == "\n"[0] || key == "\r"[0])
                    {
                        keystrokeLogEntry = String.Concat("\\return", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
                        File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        keystrokeLogEntry = String.Concat("\\escape", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
                        File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
                    }
                    //else if (Input.GetKeyDown(KeyCode.LeftShift))
                    //{
                    //    keystrokeLogEntry = String.Concat("\\shift", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
                    //    File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
                    //}
                    else
                    {
                        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) ||
                            Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow)) return;
                        keystrokeLogEntry = key == ' '
                            ? String.Concat("\\space", C, DateTime.Now.ToString("hh:mm:ss"),
                                                          C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine)
                            : String.Concat(key, C, DateTime.Now.ToString("hh:mm:ss"),
                                                          C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
                        File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
                        InputCounterPerText++;
                        InputCountPerSet++;
                        InputText += key;
                    }
                }
            }
            if (Input.anyKeyDown)
            {

                if (!IsWriting && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow)
                                      || Input.GetKeyDown(KeyCode.DownArrow)))
                {
                    UserActionTimerOn = true;
                    if (IsFirstKeyPressOfRound)
                    {
                        TimeToFirstKeyPress = TimeSinceTextStart;
                        IsFirstKeyPressOfRound = false;
                        string firstKeyStrokeLogEntry = String.Concat("first_action_time: ", TimeToFirstKeyPress, Environment.NewLine);
                        File.AppendAllText(TextsPerformanceLogPath, firstKeyStrokeLogEntry);
                    }
                    ArrowKeysCountPerSet++;
                    keystrokeLogEntry = Input.GetKeyDown(KeyCode.LeftArrow)
                        ? String.Concat("\\left", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine)
                        : Input.GetKeyDown(KeyCode.UpArrow)
                        ? String.Concat("\\up", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine)
                        : Input.GetKeyDown(KeyCode.RightArrow)
                        ? String.Concat("\\right", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine)
                        : String.Concat("\\down", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
                    File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
                    InputCounterPerText++;
                    InputCountPerSet++;
                }
            }
        }
    }

    // Log Keystrokes done on the touch screen keyboard
    private void LogSoftKeystroke(char key, bool IsSubmit)
    {
        if (!LoggingAllowed || HardkeyboardMode) return;

        UserActionTimerOn = true;

        if (IsFirstKeyPressOfRound)
        {
            TimeToFirstKeyPress = TimeSinceTextStart;
            IsFirstKeyPressOfRound = false;
            string firstKeyStrokeLogEntry = String.Concat("first_action_time: ", TimeToFirstKeyPress, Environment.NewLine);
            File.AppendAllText(TextsPerformanceLogPath, firstKeyStrokeLogEntry);
        }

        string keystrokeLogEntry;

        if (key == "\b"[0])
        {
            keystrokeLogEntry = String.Concat("\\backspace", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
            File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
            InputCounterPerText = IsWriting ? InputCounterPerText > 0 ? InputCounterPerText-- : 0 : InputCounterPerText++;
            InputCountPerSet = IsWriting ? InputCountPerSet > 0 ? InputCountPerSet-- : 0 : InputCountPerSet++;
            DeleteKeyCountPerSet++;
            if (InputText.Length != 0) InputText = InputText.Substring(0, InputText.Length - 1);
        }
        else if (key == "\n"[0] || key == "\r"[0])
        {
            if (!IsSubmit)
            {
                keystrokeLogEntry = String.Concat("\\return", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
                File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
            }
            else 
            {
                keystrokeLogEntry = String.Concat("\\submit", C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
                File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
            }
        }
        else if (key == ' ')
        {
            keystrokeLogEntry = String.Concat("\\space", C,
                                              DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);   //  C, nextCorrectChar, C, nextCorrectChar == ' '
            File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
            InputCounterPerText++;
            InputCountPerSet++;
            InputText += " ";
        }
        else
        {
            keystrokeLogEntry = IsWriting ? String.Concat(key, C, DateTime.Now.ToString("hh:mm:ss"),
                                                          C, TimeSinceTaskStart.ToString("F2"), C, Environment.NewLine) :
                                                  String.Concat(key, C, DateTime.Now.ToString("hh:mm:ss"),
                                                                C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);    // nextCorrectChar, C, key == nextCorrectChar, C,
            File.AppendAllText(KeystrokesLogPath, keystrokeLogEntry);
            InputCounterPerText++;
            InputCountPerSet++;
            InputText += key;
        }
    }

    // Log cursor selections on soft keyboard for text editing
    private void LogCursorSelectionSoft(int position)
    {
        if (!LoggingAllowed) return;
        UserActionTimerOn = true;

        if (IsFirstKeyPressOfRound)
        {
            TimeToFirstKeyPress = TimeSinceTextStart;
            IsFirstKeyPressOfRound = false;
            string firstActioneLogEntry = String.Concat("first_action_time: ", TimeToFirstKeyPress, Environment.NewLine);
            File.AppendAllText(TextsPerformanceLogPath, firstActioneLogEntry);
        }

        string selectionLogEntry = String.Concat("\\cursor", C, position, C, DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
        File.AppendAllText(SelectionsLogPath, selectionLogEntry);

        CursorPlacingCountPerSet++;
        // TODO: Is cursor change an input?
        InputCounterPerText++;
        InputCountPerSet++;
    }

    // Log text selection when a user selects a word in the text
    private void LogTextSelection(int start, int end, string word)
    {
        if (!LoggingAllowed) return;
        UserActionTimerOn = true;

        if (IsFirstKeyPressOfRound)
        {
            TimeToFirstKeyPress = TimeSinceTextStart;
            IsFirstKeyPressOfRound = false;
            string firstActioneLogEntry = String.Concat("first_action_time: ", TimeToFirstKeyPress, Environment.NewLine);
            File.AppendAllText(TextsPerformanceLogPath, firstActioneLogEntry);
        }

        string selectionLogEntry = String.Concat("\\text_selection", C,
                                                 DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"),
                                                 C, start, C, end, C, word, Environment.NewLine);
        if (!HardkeyboardMode) File.AppendAllText(SelectionsLogPath, selectionLogEntry);
        else File.AppendAllText(KeystrokesLogPath, selectionLogEntry);

        TextSelectionCountPerSet++;
        // TODO: Is text selection an input?
        InputCounterPerText++;
        InputCountPerSet++;
    }

    private void LogScroll()
    {
        if (!LoggingAllowed) return;
        UserActionTimerOn = true;
        if (IsFirstKeyPressOfRound && !HardkeyboardMode)
        {
            TimeToFirstKeyPress = TimeSinceTextStart;
            IsFirstKeyPressOfRound = false;
            string firstActioneLogEntry = String.Concat("first_action_time: ", TimeToFirstKeyPress, Environment.NewLine);
            File.AppendAllText(TextsPerformanceLogPath, firstActioneLogEntry);
        }
        string selectionLogEntry = String.Concat("\\scroll", C,
                                                 DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"), Environment.NewLine);
        File.AppendAllText(SelectionsLogPath, selectionLogEntry);
        ScrollActionCountPerSet++;
        // TODO: Is scroll an input?
        InputCounterPerText++;
        InputCountPerSet++;
    }

    public void OnTouchKeystrokeDone(string inputChar)
    {
        LogSoftKeystroke(inputChar[0], false);
    }

    public void OnSoftKeyboardSubmitted()
    {
        if (IsWriting) LogSoftKeystroke("\r"[0], false);
        else LogSoftKeystroke("\r"[0], true);
        SoftSubmitted = true;
    }

    public void OnCursorPositionChanged(int pos)
    {
        LogCursorSelectionSoft(pos);
    }

    public void OnTextSelectionDone(int start, int end, string word)
    {
        LogTextSelection(start, end, word);
    }

    public void OnScrollAction()
    {
        LogScroll();
    }

    public void OnLogPitchYawRoll(float pitch, float yaw, float roll)
    {
        string pyrText = string.Concat(pitch, C, yaw, C, roll, C,
                                      DateTime.Now.ToString("hh:mm:ss"), C, TimeSinceTaskStart.ToString("F2"));
        File.AppendAllText(PitchYawRollLogPath,  pyrText + Environment.NewLine);
    }

    private void CreateDir()
    {
        switch (Mode)
        {
            case Mode.Writing_VR_VideoCrop:
                if (!Directory.Exists(Path.Combine(BasePath, "VR_VideoCrop/Writing"))) Directory.CreateDirectory(Path.Combine(BasePath, "VR_VideoCrop/Writing"));
                CurrentLoggingFolderPath = Path.Combine(BasePath, "VR_VideoCrop/Writing");
                break;
            case Mode.Writing_SoftKeyboard:
                if (!Directory.Exists(Path.Combine(BasePath, "SoftKeyboard/Writing"))) Directory.CreateDirectory(Path.Combine(BasePath, "SoftKeyboard/Writing"));
                CurrentLoggingFolderPath = Path.Combine(BasePath, "SoftKeyboard/Writing");
                break;
            case Mode.Writing_HardKeyboard:
                if (!Directory.Exists(Path.Combine(BasePath, "HardKeyboard/Writing"))) Directory.CreateDirectory(Path.Combine(BasePath, "HardKeyboard/Writing"));
                CurrentLoggingFolderPath = Path.Combine(BasePath, "HardKeyboard/Writing");
                break;
            case Mode.Writing_VR_3D:
                if (!Directory.Exists(Path.Combine(BasePath, "VR_3D/Writing"))) Directory.CreateDirectory(Path.Combine(BasePath, "VR_3D/Writing"));
                CurrentLoggingFolderPath = Path.Combine(BasePath, "VR_3D/Writing");
                break;
            case Mode.Editing_VR_VideoCrop:
                if (!Directory.Exists(Path.Combine(BasePath, "VR_VideoCrop/Editing"))) Directory.CreateDirectory(Path.Combine(BasePath, "VR_VideoCrop/Editing"));
                CurrentLoggingFolderPath = Path.Combine(BasePath, "VR_VideoCrop/Editing");
                break;
            case Mode.Editing_SoftKeyboard:
                if (!Directory.Exists(Path.Combine(BasePath, "SoftKeyboard/Editing"))) Directory.CreateDirectory(Path.Combine(BasePath, "SoftKeyboard/Editing"));
                CurrentLoggingFolderPath = Path.Combine(BasePath, "SoftKeyboard/Editing");
                break;
            case Mode.Editing_HardKeyboard:
                if (!Directory.Exists(Path.Combine(BasePath, "HardKeyboard/Editing"))) Directory.CreateDirectory(Path.Combine(BasePath, "HardKeyboard/Editing"));
                CurrentLoggingFolderPath = Path.Combine(BasePath, "HardKeyboard/Editing");
                break;
            case Mode.Editing_VR_3D:
                if (!Directory.Exists(Path.Combine(BasePath, "VR_3D/Editing"))) Directory.CreateDirectory(Path.Combine(BasePath, "VR_3D/Editing"));
                CurrentLoggingFolderPath = Path.Combine(BasePath, "VR_3D/Editing");
                break;
        }
    }
}

public interface ILogging 
{
    void OnTouchKeystrokeDone(string inputChar);
    void OnSoftKeyboardSubmitted();
    void OnCursorPositionChanged(int pos);
    void OnTextSelectionDone(int start, int end, string word);
    void OnScrollAction();
    void OnLogPitchYawRoll(float pitch, float yaw, float roll);
}