using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public TextMeshProUGUI countDown;
    protected int InputMode;
    protected IControllerLogic listener;
    public static bool IsWriting;

    public void Reset(int type)
    {
        //if (InputMode == 1 || InputMode == 5) return;
        Debug.Log("CountDown - Reset");
        string startType = InputMode == 1 || InputMode == 5 ? "START" : "<b>ENTER</b>";
        string taskType = IsWriting ? "writing" : "editing";
        countDown.color = Color.black;
        countDown.fontSize = InputMode == 1 || InputMode == 5 || InputMode == 6 || InputMode == 2 ? 100 : GlobalSettings.FontSizeInfoVR;
        switch (type)
        {
            case 0:
                countDown.text = "Press " + startType + " to start " + taskType + " task <b>Round " + GlobalSettings.CurrentRound + "</b>";
                break;
            case 1:
                countDown.text = "Press " + startType + " to start <b>Round " + GlobalSettings.CurrentRound + "</b>";
                break;
            case 2:
                countDown.text = GlobalSettings.CurrentRound != 0 ? "<b>Time over</b> for <b>Round " + (GlobalSettings.CurrentRound) + "</b>"
                    : "<b>Time over</b> for <b> training round</b>";
                break;
            case 3:
                countDown.text = "Press " + startType + " to start " + taskType + " task \n<b>Training Round</b>";
                break;
        }
    }

    public void Clear()
    {
        countDown.text = "";
    }

    public void TimeOverText()
    {
        countDown.color = Color.black;
        countDown.fontSize = InputMode == 1 || InputMode == 5 || InputMode == 6 || InputMode == 2 ? 100 : GlobalSettings.FontSizeInfoVR;
        countDown.text = "Time is over.";
    }

    public void SetListener(IControllerLogic l)
    {
        this.listener = l;
    }

    public void SetInputMode(int i)
    {
        this.InputMode = i;
    }

    public IEnumerator CountdownFunction()
    {
        countDown.fontSize = InputMode == 1 || InputMode == 5 || InputMode == 6 || InputMode == 2 ? 100 : 13;
        countDown.text = "3";
        countDown.color = Color.red;
        yield return new WaitForSeconds(1);

        countDown.text = "2";
        yield return new WaitForSeconds(1);

        countDown.text = "1";
        yield return new WaitForSeconds(1);

        countDown.color = Color.green;
        countDown.text = "GO!";

        listener.CountDownEnd();
        yield return new WaitForSeconds(1);
        countDown.fontSize = InputMode == 1 || InputMode == 5 || InputMode == 6 || InputMode == 2 ? 100 : 13;
        countDown.text = "";
    }
}
