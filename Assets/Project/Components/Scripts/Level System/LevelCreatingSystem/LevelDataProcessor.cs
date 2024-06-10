using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Project.Components.Scripts.Level_System;
using Project.Components.Scripts.Level_System.LevelStructure;
using UnityEngine;

public class LevelDataProcessor : MonoBehaviour
{
    [SerializeField] private JsonLevelParser _jsonLevelParser;
    [SerializeField] private float _difficultyCoefficient;

    [SerializeField] private LevelDataList _levelDataCollection;

    public void Calculate()
    {
        ProcessLevelData(_jsonLevelParser.GetLevelDataList());
    }

    public void CalculateModifiedLevel()
    {
        ProcessLevelData(_levelDataCollection);
        SaveLevelsToFile();
    }
    
    private void SaveLevelsToFile()
    {
        string json = JsonConvert.SerializeObject(_levelDataCollection, Formatting.Indented);
        string filePath = Path.Combine(Application.streamingAssetsPath, "TestLevels");

        File.WriteAllText(filePath, json);
        Debug.Log("Levels saved to: " + filePath);
    }

    private void ProcessLevelData(LevelDataList levelDataList)
    {
        if (levelDataList != null && levelDataList.Levels != null)
        {
            _levelDataCollection = levelDataList;

            // Output difficulty for each level
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

        // Итерируемся по типам врагов
        for (int i = 0; i < enemyTypesCount; i++)
        {
            EnemyTypeInfo enemyTypeInfo = levelData.EnemyTypesInfo[i];

            int enemyTypeLevel = int.Parse(enemyTypeInfo.EnemyPrefabName[^1].ToString());
            float enemyDifficulty = enemyTypeLevel * _difficultyCoefficient;

            // Итерируемся по каждому врагу в данном типе
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