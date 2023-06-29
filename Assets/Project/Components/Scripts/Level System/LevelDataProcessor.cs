using UnityEngine;
using System.Text;
using Project.Components.Scripts.Enemies;
using Project.Components.Scripts.Level_System;

public class LevelDataProcessor : MonoBehaviour
{
    public TextAsset levelDataJson;
    public LevelDataList levelDataCollection;
    public float difficultyCoefficient;

    private void Start()
    {
        if (levelDataJson != null)
        {
            ProcessLevelData();
        }
        else
        {
            Debug.LogError("JSON file not assigned!");
        }
    }

    private void ProcessLevelData()
    {
        // Check if JSON data is available
        if (string.IsNullOrEmpty(levelDataJson.text))
        {
            Debug.LogError("No JSON data found!");
            return;
        }

        // Deserialize JSON data
        levelDataCollection = JsonUtility.FromJson<LevelDataList>(levelDataJson.text);

        if (levelDataCollection != null && levelDataCollection.levels != null)
        {
            // Output difficulty for each level
            foreach (LevelData levelData in levelDataCollection.levels)
            {
                float levelDifficulty = CalculateLevelDifficulty(levelData);
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Level {0} difficulty: {1}", levelData.numberOfLevel, levelDifficulty);
                Debug.Log(sb.ToString());
            }
        }
        else
        {
            Debug.LogError("Failed to parse JSON data!");
        }
    }

    private float CalculateLevelDifficulty(LevelData levelData)
    {
        float levelDifficulty = 0f;
        int enemyTypesCount = levelData.enemyTypesInfo.Count;

        // Iterate over enemy types
        for (int i = 0; i < enemyTypesCount; i++)
        {
            EnemyTypeInfo enemyTypeInfo = levelData.enemyTypesInfo[i];
            int enemyType = i + 1;
            float enemyDifficulty = 1 + (enemyType - 1) * difficultyCoefficient;

            // Add contribution to level difficulty
            levelDifficulty += enemyTypeInfo.maxSpawnCount * enemyDifficulty;
        }

        levelDifficulty *= (1 + (enemyTypesCount - 1) * difficultyCoefficient);
        levelDifficulty *= levelData.timeToSpawn;
        levelDifficulty += (levelData.minutesToWin * 60 + levelData.secondsToWin) * difficultyCoefficient;

        return levelDifficulty;
    }
}