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

        // Итерируемся по типам врагов
        for (int i = 0; i < enemyTypesCount; i++)
        {
            EnemyTypeInfo enemyTypeInfo = levelData.enemyTypesInfo[i];
            int enemyType = i + 1;
            float enemyDifficulty = 1 + (enemyType - 1) * difficultyCoefficient;

            // Рассчитываем время, проведенное с каждым врагом
            float timeSpent = 0f;
            if (i == 0)
            {
                // Первый враг тратит timeToSpawn
                timeSpent = levelData.timeToSpawn;
            }
            else if (i == enemyTypesCount - 1)
            {
                // Последний враг тратит secondsToWin и minutesToWin
                timeSpent = levelData.minutesToWin * 60 + levelData.secondsToWin;
            }
            else
            {
                // Промежуточные враги тратят суммарное время до них
                for (int j = 0; j < i; j++)
                {
                    timeSpent += levelData.enemyTypesInfo[j].maxSpawnCount * levelData.timeToSpawn;
                }
            }

            // Добавляем вклад в сложность уровня
            levelDifficulty += enemyTypeInfo.maxSpawnCount * enemyDifficulty * timeSpent;
        }

        // Добавляем вклад последнего врага в сложность уровня
        EnemyTypeInfo lastEnemyTypeInfo = levelData.enemyTypesInfo[enemyTypesCount - 1];
        levelDifficulty += lastEnemyTypeInfo.maxSpawnCount * (1 + (enemyTypesCount - 1) * difficultyCoefficient) *
                           (levelData.minutesToWin * 60 + levelData.secondsToWin);

        return levelDifficulty;
    }
}