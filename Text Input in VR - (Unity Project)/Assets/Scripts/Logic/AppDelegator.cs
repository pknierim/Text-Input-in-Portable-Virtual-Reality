using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppDelegator : MonoBehaviour, IAppDelegator
{
    protected MenuAction MenuAction;
    protected int CurrentConditionNumber;
    protected Condition[] UserRow;

    private void Start()
    {
        MenuAction = GetComponent<MenuAction>();
        MenuAction.SetListener(this);
        if (!GlobalSettings.InGame) 
        {
            GlobalSettings.TextPackageNumber = 0;
            GlobalSettings.NumberOfFinishedConditions = 0;
            GlobalSettings.CurrentTask = GlobalSettings.FirstTask;
            GlobalSettings.UsedPhrases = new List<int>();
            CurrentConditionNumber = 0;
        }
    }

    public void OnUserIDSet()
    {
        if(Conditions.UserIDConditions.ContainsKey(GlobalSettings.UserID))
        {
            UserRow = Conditions.UserIDConditions[GlobalSettings.UserID];
            GlobalSettings.UserRow = UserRow;
            GlobalSettings.Condition = UserRow[CurrentConditionNumber];
            GlobalSettings.CurrentConditionNumber = 0;
            StartCoroutine(EnableMenuCondition());
        }
        else 
        {
            MenuAction.OnUserIdError(GlobalSettings.UserID);
        }
    }

    public void OnConditionFinished()
    {
        GlobalSettings.InGame = true;
        GlobalSettings.CurrentConditionNumber++;
        GlobalSettings.Condition = GlobalSettings.UserRow[GlobalSettings.CurrentConditionNumber];
        SceneManager.LoadSceneAsync("Menu");
        SceneManager.UnloadSceneAsync(GlobalSettings.CurrentScene);
        MenuAction = GetComponent<MenuAction>();
        StartCoroutine(EnableMenuCondition());
    }

    private IEnumerator EnableMenuCondition()
    {
        yield return new WaitForSeconds(3);
        MenuAction.PrepareTextsForCondition();
    }

    public void OnAllTasksFinished()
    {
        GlobalSettings.Finished = true;
        SceneManager.LoadSceneAsync("Menu");
    }
}

public interface IAppDelegator
{
    void OnUserIDSet();
    void OnConditionFinished();
    void OnAllTasksFinished();
}

public static class Conditions
{
    //private static readonly Condition[] row1 = { Condition.VR_VideoCrop, Condition.VR_No_Hands_No_Keyboard, Condition.Softkeyboard_Phone, Condition.Hardkeyboard_Phone };
    //private static readonly Condition[] row2 = { Condition.VR_No_Hands_No_Keyboard, Condition.Softkeyboard_Phone, Condition.VR_VideoCrop, Condition.Hardkeyboard_Phone };
    //private static readonly Condition[] row3 = { Condition.Softkeyboard_Phone, Condition.Hardkeyboard_Phone, Condition.VR_No_Hands_No_Keyboard, Condition.VR_VideoCrop };
    //private static readonly Condition[] row4 = { Condition.Hardkeyboard_Phone, Condition.VR_VideoCrop, Condition.Softkeyboard_Phone, Condition.VR_No_Hands_No_Keyboard };

    private static readonly Condition[] row1 = { Condition.VR_VideoCrop, Condition.Softkeyboard_Phone, Condition.Hardkeyboard_Phone };
    private static readonly Condition[] row2 = { Condition.Softkeyboard_Phone,Condition.Hardkeyboard_Phone, Condition.VR_VideoCrop };
    private static readonly Condition[] row3 = { Condition.Hardkeyboard_Phone, Condition.VR_VideoCrop, Condition.Softkeyboard_Phone };
    private static readonly Condition[] row4 = { Condition.Hardkeyboard_Phone, Condition.Softkeyboard_Phone, Condition.VR_VideoCrop };
    private static readonly Condition[] row5 = { Condition.VR_VideoCrop, Condition.Hardkeyboard_Phone, Condition.Softkeyboard_Phone };
    private static readonly Condition[] row6 = { Condition.Softkeyboard_Phone, Condition.VR_VideoCrop, Condition.Hardkeyboard_Phone };

    public static readonly Dictionary<string, Condition[]> UserIDConditions = new Dictionary<string, Condition[]>()
        {
            { "1",  row1 },
            { "2",  row2 },
            { "3",  row3 },
            { "4",  row4 },
            { "5",  row5 },
            { "6",  row6 },
            { "7",  row1 },
            { "8",  row2 },
            { "9",  row3 },
            { "10", row4 },
            { "11", row5 },
            { "12", row6 },
            { "13", row1 },
            { "14", row2 },
            { "15", row3 },
            { "16", row4 },
            { "17", row5 },
            { "18", row6 },
            { "19", row1 },
            { "20", row2 },
            { "21", row3 },
            { "22", row4 },
            { "23", row5 },
            { "24", row6 },
            { "25", row1 },
            { "26", row2 },
            { "27", row3 },
            { "28", row4 },
            { "29", row5 },
            { "30", row6 },
            { "31", row1 },
            { "32", row2},
            { "33", row3},
            { "34", row4},
            { "35", row5},
            { "36", row6}
        };

    //public static readonly Dictionary<string, Condition[]> UserIDConditions = new Dictionary<string, Condition[]>()
    //{
    //    { "1",  row1 },
    //    { "2",  row2 },
    //    { "3",  row3 },
    //    { "4",  row4 },
    //    { "5",  row1 },
    //    { "6",  row2 },
    //    { "7",  row3 },
    //    { "8",  row4 },
    //    { "9",  row1 },
    //    { "10", row2 },
    //    { "11", row3 },
    //    { "12", row4 },
    //    { "13", row1 },
    //    { "14", row2 },
    //    { "15", row3 },
    //    { "16", row4 },
    //    { "17", row1 },
    //    { "18", row2 },
    //    { "19", row3 },
    //    { "20", row4 },
    //    { "21", row1 },
    //    { "22", row2 },
    //    { "23", row3 },
    //    { "24", row4 },
    //    { "25", row1 },
    //    { "26", row2 },
    //    { "27", row3 },
    //    { "28", row4 },
    //    { "29", row1 },
    //    { "30", row2 },
    //    { "31", row3 },
    //    { "32", row4 }
    //};
}
