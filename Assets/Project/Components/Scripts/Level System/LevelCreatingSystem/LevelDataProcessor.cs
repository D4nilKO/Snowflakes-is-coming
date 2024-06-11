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

        private void CalculateLevels()
        {
            ProcessLevelData(_jsonLevelParser.GetLevelDataList());
        }

        private void CalculateModifiedLevels()
        {
            ProcessLevelData(_levelDataCollection);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CalculateLevels();
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                CalculateModifiedLevels();
            }
        }

        private void ProcessLevelData(LevelDataList levelDataList)
        {
            if (levelDataList is { Levels: not null })
            {
                _levelDataCollection = levelDataList;

                foreach (LevelData levelData in levelDataList.Levels)
                {
                    float levelDifficulty = CalculateLevelDifficulty(levelData);
                    StringBuilder sb = new();
                    sb.Append($"Level {levelData.NumberOfLevel} difficulty: {levelDifficulty}");
                    Debug.Log(sb.ToString());
                }
            }
            else
            {
                Debug.LogError("Failed to process level data");
            }
        }

        private float CalculateLevelDifficulty(LevelData levelData)
        {
            float levelDifficulty = 0f;
            int enemyTypesCount = levelData.EnemyTypesInfo.Count;
            float timeToWin = levelData.TimeToSurvive;
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