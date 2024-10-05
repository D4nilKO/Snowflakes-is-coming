using System.Collections.Generic;
using System.Text;
using Project.Components.Scripts.Level_System.LevelStructure;
using UnityEngine;

namespace Project.Components.Scripts.Level_System.LevelCreatingSystem
{
    public class LevelDataProcessor : MonoBehaviour
    {
        [SerializeField] private JsonLevelParser _jsonLevelParser;
        [SerializeField] private float _difficultyCoefficient;

        [SerializeField] private LevelDataList _levelDataCollection;

        private Dictionary<LevelData, float> _levelsDictionary = new();

        private void CalculateLevels()
        {
            ProcessLevelData(_jsonLevelParser.GetLevelDataList());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CalculateLevels();
                DisplayLevelsDifficulty(_levelsDictionary);
            }
        }

        public Dictionary<LevelData, float> GetLevelsDifficultyData()
        {
            if (_levelsDictionary.Count == 0)
            {
                ProcessLevelData(_levelDataCollection);
            }

            return _levelsDictionary;
        }

        public void DisplayLevelsDifficulty(Dictionary<LevelData, float> levelsWithDifficulty)
        {
            foreach (KeyValuePair<LevelData, float> level in levelsWithDifficulty)
            {
                Debug.Log($"Уровень {level.Key.NumberOfLevel} --- сложность: {level.Value} --- время: {level.Key.GetTimeToSurvive()}");
            }
        }

        private void ProcessLevelData(LevelDataList levelDataList)
        {
            if (levelDataList is { Levels: not null })
            {
                foreach (LevelData levelData in levelDataList.Levels)
                {
                    float difficulty = CalculateLevelDifficulty(levelData);
                    _levelsDictionary.Add(levelData, difficulty);
                }
            }
            else
            {
                Debug.LogError("Failed to fill dictionary");
            }
        }

        private float CalculateLevelDifficulty(LevelData levelData)
        {
            float levelDifficulty = 0f;
            int enemyTypesCount = levelData.EnemyTypesInfo.Count;
            float timeToWin = levelData.GetTimeToSurvive();
            float timeLeft = timeToWin;

            for (int i = 0; i < enemyTypesCount; i++)
            {
                EnemyTypeInfo enemyTypeInfo = levelData.EnemyTypesInfo[i];

                int enemyTypeLevel = int.Parse(enemyTypeInfo.EnemyPrefabName[^1].ToString());
                float enemyDifficulty = enemyTypeLevel * _difficultyCoefficient;

                for (int j = 0; j < enemyTypeInfo.MaxSpawnCount; j++)
                {
                    levelDifficulty += enemyDifficulty * timeLeft;
                    timeLeft -= levelData.TimeToSpawn;

                    if (timeLeft < 0)
                    {
                        Debug.LogError("Time left less than 0");
                        break;
                    }
                }
            }

            return levelDifficulty;
        }
    }
}