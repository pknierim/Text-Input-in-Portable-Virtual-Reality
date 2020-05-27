using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static float MAX_TIME_GAME = GlobalSettings.GameTimePerTask;
    public static float MAX_TIME_RUN = GlobalSettings.GameTimePerRun;
    private float gameTimer = 0;
    private float runTimer = 0;
    private bool gameRunning = false;
    private bool runRunning = false;
    private IControllerLogic listener;

    public void SetListener(IControllerLogic l)
    {
        this.listener = l;
    }

    public void SetGameRunning(bool b)
    {
        this.gameRunning = b;
        gameTimer = 0;
    }

    public void SetRunRunning(bool b)
    {
        this.runRunning = b;
    }

    public void ResetRunTimer()
    {
        runTimer = 0;
        runRunning = false;
    }

    void Update()
    {
        //Debug.Log("GameTimer: " + gameTimer + " s");
        //Debug.Log("RunTimer: " + runTimer + " s");
        gameTimer = gameRunning ? gameTimer + Time.deltaTime : gameTimer;
        runTimer = runRunning ? runTimer + Time.deltaTime : runTimer;
        if (gameRunning && gameTimer > MAX_TIME_GAME)
        {
            gameRunning = false;
            //listener.OnTimeOver();
        }
        if (runRunning && runTimer > MAX_TIME_RUN)
        {
            runRunning = false;
            runTimer = 0;
            listener.OnRunTimeOver();
        }
    }
}
