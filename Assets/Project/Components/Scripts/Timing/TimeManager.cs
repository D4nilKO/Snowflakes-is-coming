using System;
using System.Collections;
using Project.Components.Scripts;
using Project.Components.Scripts.Utility;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float startTimeScale = 1;

    [SerializeField] private float startTimePauseBeforeContinueTime = 0.5f;
    private float timePauseBeforeContinueTime;

    [HideInInspector] public int secondCounter;
    [HideInInspector] public int minuteCounter;

    [HideInInspector] [Range(1, 59)] public int secondsToWin;
    [HideInInspector] [Min(0)] public int minutesToWin;

    private SurviveTimer gameTimer = new();
    private TimerViewer timerViewer;
    private GameStateMachine gameStateMachine;

    private void Awake()
    {
        gameStateMachine = FindObjectOfType<GameStateMachine>();
        timerViewer = FindObjectOfType<TimerViewer>();
        Time.timeScale = startTimeScale;

        minuteCounter = gameTimer.Minute;
        secondCounter = gameTimer.Second;
    }

    private void Start()
    {
        gameStateMachine.PauseGame();
        gameStateMachine.ResumeGame();
    }

    private void FixedUpdate()
    {
        MainTimer();
    }

    private void MainTimer()
    {
        gameTimer.AddTime(Time.fixedDeltaTime);

        if (secondCounter == gameTimer.Second) return;
        secondCounter = gameTimer.Second;

        CheckLevelWon();

        timerViewer.UpdateMainTimerText(gameTimer.FormattedTime());
    }

    private void CheckLevelWon()
    {
        if ((secondsToWin != secondCounter) || (minutesToWin != minuteCounter)) return;
        gameStateMachine.WonLevel();
    }

    public void ApplyWaitBeforeContinueTime()
    {
        StopCoroutine(WaitBeforeContinueTime());
        StartCoroutine(WaitBeforeContinueTime());
    }

    private IEnumerator WaitBeforeContinueTime()
    {
        timePauseBeforeContinueTime = startTimePauseBeforeContinueTime;
        while (timePauseBeforeContinueTime > 0)
        {
            timePauseBeforeContinueTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = startTimeScale;
    }
}