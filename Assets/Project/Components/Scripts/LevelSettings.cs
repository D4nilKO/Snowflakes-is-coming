using System.Collections.Generic;
using System.IO;
using System.Linq;
using Project.Components.Scripts.Data;
using Project.Components.Scripts.Enemies;
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

        [SerializeField] 
        private int minutesToWin;

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
            mainTimeToSurvive = enemyTypesInfo.Sum(t => t.maxSpawnCount * timeToSpawn);

            mainTimeToSurvive += secondsToWin + (minutesToWin * SecondsInMinute);
        }

        private void LoadLevelSettings()
        {
            var filePath = Path.Combine(Application.dataPath, jsonFileName);

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var levelDataList = JsonUtility.FromJson<LevelDataList>(json);

                print("Уровень : " + GameData.currentLevelNumber);
                GameData.maxLevelscount = levelDataList.levels.Count;

                if (GameData.currentLevelNumber <= levelDataList.levels.Count)
                {
                    var levelData = levelDataList.levels[GameData.currentLevelNumber - 1];

                    timeToSpawn = levelData.timeToSpawn;
                    secondsToWin = levelData.secondsToWin;
                    minutesToWin = levelData.minutesToWin;
                    enemyTypesInfo = levelData.enemyTypesInfo;

                    CalculateTimeToSurvive();

                    timeManager.secondsToWin = (mainTimeToSurvive % SecondsInMinute);
                    timeManager.minutesToWin = (mainTimeToSurvive / SecondsInMinute);

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
                Debug.LogError("Указанный JSON файл не найден: " + filePath);
            }
        }
    }
}