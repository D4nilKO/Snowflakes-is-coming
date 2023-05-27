using System.Collections;
using Project.Components.Scripts;
using Project.Components.Scripts.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float startTimeScale = 1;

    [SerializeField] private float startTimePauseBeforeContinueTime = 0.5f;

    private float timePauseBeforeContinueTime;

    [HideInInspector] public int secondCounter;
    [HideInInspector] public int minuteCounter;

    private SurviveTimer gameTime = new();
    private TimerViewer timerViewer;


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

        
        timerViewer.UpdateMainTimerText(gameTime.FormattedTime());
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