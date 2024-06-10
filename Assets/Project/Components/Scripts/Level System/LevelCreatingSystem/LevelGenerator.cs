using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Project.Components.Scripts.Level_System.LevelStructure;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int _numberOfLevels;
    [SerializeField] private int _numberOfEnemyTypes;
    [SerializeField] private int _maxSpawnCount;
    [SerializeField] private int _minSpawnCount;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private string _savePath;
    [SerializeField] private string _fileName;

    private LevelDataList _levelDataList;

    private void Start()
    {
        GenerateLevels();
        SaveLevelsToFile();
    }

    private void GenerateLevels()
    {
        _levelDataList = new LevelDataList();
        _levelDataList.Levels = new List<LevelData>();

        for (int i = 1; i <= _numberOfLevels; i++)
        {
            LevelData levelData = GenerateLevel(i);
            _levelDataList.Levels.Add(levelData);
        }
    }

    private LevelData GenerateLevel(int levelNumber)
    {
        LevelData levelData = new ();
        levelData.NumberOfLevel = levelNumber;
        levelData.TimeToSpawn = (int)Random.Range(_minSpawnTime, _maxSpawnTime);
        levelData.SecondsToWin = CalculateSecondsToWin(levelNumber);

        levelData.EnemyTypesInfo = new List<EnemyTypeInfo>();
        int remainingSpawnCount = Random.Range(_minSpawnCount, _maxSpawnCount);

        for (int i = 1; i <= _numberOfEnemyTypes; i++)
        {
            EnemyTypeInfo enemyTypeInfo = GenerateEnemyTypeInfo(i, remainingSpawnCount);
            levelData.EnemyTypesInfo.Add(enemyTypeInfo);
            remainingSpawnCount -= enemyTypeInfo.MaxSpawnCount;

            if (remainingSpawnCount <= 0)
                break;
        }

        return levelData;
    }

    private int CalculateSecondsToWin(int levelNumber)
    {
        // You can adjust the formula to fit your desired difficulty progression
        return Mathf.RoundToInt(10 - (levelNumber * 0.5f));
    }

    private EnemyTypeInfo GenerateEnemyTypeInfo(int enemyType, int remainingSpawnCount)
    {
        EnemyTypeInfo enemyTypeInfo = new();
        enemyTypeInfo.EnemyPrefabName = "Enemy " + enemyType;
        enemyTypeInfo.MaxSpawnCount = Random.Range(1, Mathf.Min(remainingSpawnCount, _maxSpawnCount));

        return enemyTypeInfo;
    }

    private void SaveLevelsToFile()
    {
        string json = JsonConvert.SerializeObject(_levelDataList, Formatting.Indented);
        string filePath = Path.Combine(_savePath, _fileName);

        File.WriteAllText(filePath, json);
        Debug.Log("Levels saved to: " + filePath);
    }
}