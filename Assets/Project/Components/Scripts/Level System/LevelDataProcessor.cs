using UnityEngine;
using System.Text;
using Project.Components.Scripts.Entities.Enemies;
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

        if (levelDataCollection != null && levelDataCollection.Levels != null)
        {
            // Output difficulty for each level
            foreach (LevelData levelData in levelDataCollection.Levels)
            {
                float levelDifficulty = CalculateLevelDifficulty(levelData);
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Level {0} difficulty: {1}", levelData.NumberOfLevel, levelDifficulty);
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
        int enemyTypesCount = levelData.EnemyTypesInfo.Count;

        // Итерируемся по типам врагов
        for (int i = 0; i < enemyTypesCount; i++)
        {
            EnemyTypeInfo enemyTypeInfo = levelData.EnemyTypesInfo[i];
            int enemyType = i + 1;
            float enemyDifficulty = 1 + (enemyType - 1) * difficultyCoefficient;

            // Рассчитываем время, проведенное с каждым врагом
            float timeSpent = 0f;
            if (i == 0)
            {
                // Первый враг тратит timeToSpawn
                timeSpent = levelData.TimeToSpawn;
            }
            else if (i == enemyTypesCount - 1)
            {
                // Последний враг тратит secondsToWin и minutesToWin
                timeSpent = levelData.MinutesToWin * 60 + levelData.SecondsToWin;
            }
            else
            {
                // Промежуточные враги тратят суммарное время до них
                for (int j = 0; j < i; j++)
                {
                    timeSpent += levelData.EnemyTypesInfo[j].MaxSpawnCount * levelData.TimeToSpawn;
                }
            }

            // Добавляем вклад в сложность уровня
            levelDifficulty += enemyTypeInfo.MaxSpawnCount * enemyDifficulty * timeSpent;
        }

        // Добавляем вклад последнего врага в сложность уровня
        EnemyTypeInfo lastEnemyTypeInfo = levelData.EnemyTypesInfo[enemyTypesCount - 1];
        levelDifficulty += lastEnemyTypeInfo.MaxSpawnCount * (1 + (enemyTypesCount - 1) * difficultyCoefficient) *
                           (levelData.MinutesToWin * 60 + levelData.SecondsToWin);

        return levelDifficulty;
    }
}