using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalSettings
{

    public static int SceneType { get; set; }
    public static Condition Condition { get; set; }
    public static string UserID { get; set; }
    public static int TextPackageNumber { get; set; }
    public static int TaskNumber { get; set; }
    public static Condition[] UserRow { get; set; }
    public static bool InGame { get; set; }
    public static bool CurrentWriting { get; set; }
    public static int CurrentConditionNumber { get; set; }
    public static int NumberOfFinishedConditions { get; set; }
    public static bool Finished { get; set; }
    public static string CurrentScene { get; set; }
    public static int CurrentRound { get; set; }
    public static bool CurrentTracked { get; set; }
    public static bool InTask { get; set; }
    public static bool GlobalBlock { get; set; }
    public static Task CurrentTask { get; set; }
    public static bool ImageTargetOnceTracked { get; set; }
    public static Task FirstTask = Task.Writing;
    public static List<int> UsedPhrases { get; set; }
    public static bool KeepTargetAlive = true;
    public static bool CheckAndHandleOutOfBounds = true;
    public static bool VideoOutOfBounds { get; set; }
    public static bool UseTextMeshInVR = true;
    public static bool IsCurrentSceneVR { get; set; }

    public static Condition[] conditions = {
        Condition.VR_VideoCrop,
        Condition.Softkeyboard_Phone, 
        //Condition.VR_No_Hands_No_Keyboard, 
        Condition.Hardkeyboard_Phone
    };
    public static int NumberOfConditions = conditions.Length;
    public static int NumberOfPhrasesForWriting = 10;        // 10 for every run
    public static int NumberOfTextsForEditing = 1;          // 3 for every run
    public static int NumberOfRunsForWriting = 3;           // 3 main rounds, 1 training
    public static int NumberOfRunsForEditing = 3;           // 3 main rounds, 1 training
    public static int NumberOfTrainingRuns = 1;

    public static float GameTimePerTask = 720;               // 600 -720 per task
    public static float GameTimePerRun = 300;                // 300 sec

    public static int TextLoad = 2;                         // 0 for normal text length (6 sentences), 2 for double (12)

    public static int FontSizeInfoVR = 8;
    public static int FontSizeInfoNonVR = 80;
}
