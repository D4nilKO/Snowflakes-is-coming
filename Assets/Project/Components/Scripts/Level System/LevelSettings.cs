using System.Collections.Generic;
using System.Linq;
using Project.Components.Scripts.Data;
using Project.Components.Scripts.Entities.Enemies;
using Project.Components.Scripts.Level_System;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class LevelSettings : MonoBehaviour
    {
        public string jsonFileName;

        [Header("Период спавна врагов")] [SerializeField]
        private int timeToSpawn;

        [Header("Время, которое нужно ДОПОЛНИТЕЛЬНО продержаться")] [SerializeField]
        private int secondsToWin;

        [SerializeField] private int minutesToWin;

        [Space(10)] public List<EnemyTypeInfo> enemyTypesInfo;

        private TimeManager timeManager;
        private EnemySpawner enemySpawner;
        private int mainTimeToSurvive;
        private const int SecondsInMinute = 60;

        private void Awake()
        {
            timeManager = FindObjectOfType<TimeManager>();
            enemySpawner = FindObjectOfType<EnemySpawner>();
            LoadLevelSettings();
        }

        private void CalculateTimeToSurvive()
        {
            mainTimeToSurvive = enemyTypesInfo.Sum(t => t.MaxSpawnCount * timeToSpawn);
            mainTimeToSurvive += secondsToWin + (minutesToWin * SecondsInMinute);
        }

        private void LoadLevelSettings()
        {
            var json = Resources.Load<TextAsset>(jsonFileName);

            if (json != null)
            {
                var levelDataList = JsonUtility.FromJson<LevelDataList>(json.text);

                int maxLevelsCount = levelDataList.Levels.Count;
                GameData.maxLevelsCount = maxLevelsCount;
                int currentLevelNumber = GameData.currentLevelNumber;

                if (currentLevelNumber <= maxLevelsCount)
                {
                    var levelData = levelDataList.Levels[currentLevelNumber - 1];

                    timeToSpawn = levelData.TimeToSpawn;
                    secondsToWin = levelData.SecondsToWin;
                    minutesToWin = levelData.MinutesToWin;
                    enemyTypesInfo = levelData.EnemyTypesInfo;

                    CalculateTimeToSurvive();

                    timeManager.secondsToWin = mainTimeToSurvive % SecondsInMinute;
                    timeManager.minutesToWin = mainTimeToSurvive / SecondsInMinute;

                    enemySpawner.enemyTypes = enemyTypesInfo;
                    enemySpawner.timerSeconds = timeToSpawn;
                }
                else
                {
                    Debug.LogError("Уровень не найден в JSON файле: " + jsonFileName);
                }
            }
            else
            {
                Debug.LogError("Указанный JSON файл не найден: " + jsonFileName);
            }
        }
    }
}