using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Project.Components.Scripts.Enemies;
using Project.Components.Scripts.Level_System;

public class LevelGenerator : MonoBehaviour
{
    public int numberOfLevels;
    public int numberOfEnemyTypes;
    public int maxSpawnCount;
    public int minSpawnCount;
    public float maxSpawnTime;
    public float minSpawnTime;
    public string savePath;
    public string fileName;

    private LevelDataList levelDataList;

    private void Start()
    {
        GenerateLevels();
        SaveLevelsToFile();
    }

    private void GenerateLevels()
    {
        levelDataList = new LevelDataList();
        levelDataList.levels = new List<LevelData>();

        for (int i = 1; i <= numberOfLevels; i++)
        {
            LevelData levelData = GenerateLevel(i);
            levelDataList.levels.Add(levelData);
        }
    }

    private LevelData GenerateLevel(int levelNumber)
    {
        LevelData levelData = new LevelData();
        levelData.numberOfLevel = levelNumber;
        levelData.timeToSpawn = (int)Random.Range(minSpawnTime, maxSpawnTime);
        levelData.secondsToWin = CalculateSecondsToWin(levelNumber);
        levelData.minutesToWin = CalculateMinutesToWin(levelData.secondsToWin);

        levelData.enemyTypesInfo = new List<EnemyTypeInfo>();
        int remainingSpawnCount = Random.Range(minSpawnCount, maxSpawnCount);

        for (int i = 1; i <= numberOfEnemyTypes; i++)
        {
            EnemyTypeInfo enemyTypeInfo = GenerateEnemyTypeInfo(i, remainingSpawnCount);
            levelData.enemyTypesInfo.Add(enemyTypeInfo);
            remainingSpawnCount -= enemyTypeInfo.maxSpawnCount;

            if (remainingSpawnCount <= 0)
                break;
        }

        return levelData;
    }

    private int CalculateSecondsToWin(int levelNumber)
    {
        // You can adjust the formula to fit your desired difficulty progression
        return Mathf.RoundToInt(10 - levelNumber * 0.5f);
    }

    private int CalculateMinutesToWin(int secondsToWin)
    {
        return secondsToWin / 60;
    }

    private EnemyTypeInfo GenerateEnemyTypeInfo(int enemyType, int remainingSpawnCount)
    {
        EnemyTypeInfo enemyTypeInfo = new EnemyTypeInfo();
        enemyTypeInfo.enemyPrefabName = "Enemy " + enemyType;
        enemyTypeInfo.maxSpawnCount = Random.Range(1, Mathf.Min(remainingSpawnCount, maxSpawnCount));

        return enemyTypeInfo;
    }

    private void SaveLevelsToFile()
    {
        string json = JsonConvert.SerializeObject(levelDataList, Formatting.Indented);
        string filePath = Path.Combine(savePath, fileName);

        File.WriteAllText(filePath, json);
        Debug.Log("Levels saved to: " + filePath);
    }
}