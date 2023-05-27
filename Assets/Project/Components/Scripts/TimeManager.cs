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
    
    [HideInInspector][Range(1, 59)] public int secondsToWin;
    [HideInInspector][Min(0)] public int minutesToWin;

    private SurviveTimer gameTime = new();
    private TimerViewer timerViewer;
    private GameStateMachine gameStateMachine;

    private void Awake()
    {
        gameStateMachine = FindObjectOfType<GameStateMachine>();
    }

    private void Start()
    {
        timerViewer = FindObjectOfType<TimerViewer>();
        Time.timeScale = startTimeScale;

        minuteCounter = gameTime.Minute;
        secondCounter = gameTime.Second;
    }

    private void FixedUpdate()
    {
        MainTimer();
    }

    private void MainTimer()
    {
        gameTime.AddTime(Time.fixedDeltaTime);

        if (secondCounter == gameTime.Second) return;
        secondCounter = gameTime.Second;

        CheckLevelWon();
        
        timerViewer.UpdateMainTimerText(gameTime.FormattedTime());
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