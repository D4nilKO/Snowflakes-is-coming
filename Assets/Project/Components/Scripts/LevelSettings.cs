using System.Collections.Generic;
using System.Linq;
using Project.Components.Scripts.Enemies;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class LevelSettings : MonoBehaviour
    {
        [Header("Номер уровня")] [SerializeField]
        private int numberOfLevel;

        [Header("Период спавна врагов")] [SerializeField]
        private int timeToSpawn;

        [Header("Время, которое нужно ДОПОЛНИТЕЛЬНО продержаться")] [Range(0, 59)] [SerializeField] [Space(5)]
        private int secondsToWin = 1;

        [Range(0, 2)] [SerializeField] private int minutesToWin;

        [Space(10)] public List<EnemyTypeInfo> enemyTypesInfo;

        private TimeManager timeManager;
        private EnemySpawner enemySpawner;
        
        private int mainTimeToSurvive;
        private const int SecondsInMinute = 60;

        private void Awake()
        {
            timeManager = FindObjectOfType<TimeManager>();
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }

        private void Start()
        {
            LoadSceneSettings();
        }

        private void CalculateTimeToSurvive()
        {
            mainTimeToSurvive = enemyTypesInfo.Sum(t => t.maxSpawnCount * timeToSpawn);

            mainTimeToSurvive += secondsToWin + (minutesToWin * SecondsInMinute);
        }

        private void LoadSceneSettings()
        {
            CalculateTimeToSurvive();

            timeManager.secondsToWin = (mainTimeToSurvive % SecondsInMinute);
            timeManager.minutesToWin = (mainTimeToSurvive / SecondsInMinute);
            
            enemySpawner.enemyTypes = enemyTypesInfo;
            enemySpawner.timerSeconds = timeToSpawn;
        }
    }
}