﻿// using UnityEngine;
// using System.Collections.Generic;
// using System.IO;
// using Newtonsoft.Json;
// using Project.Components.Scripts.Entities.Enemies;
// using Project.Components.Scripts.Level_System;
//
// public class LevelGenerator : MonoBehaviour
// {
//     public int numberOfLevels;
//     public int numberOfEnemyTypes;
//     public int maxSpawnCount;
//     public int minSpawnCount;
//     public float maxSpawnTime;
//     public float minSpawnTime;
//     public string savePath;
//     public string fileName;
//
//     private LevelDataList levelDataList;
//
//     private void Start()
//     {
//         GenerateLevels();
//         SaveLevelsToFile();
//     }
//
//     private void GenerateLevels()
//     {
//         levelDataList = new LevelDataList();
//         levelDataList.Levels = new List<LevelData>();
//
//         for (int i = 1; i <= numberOfLevels; i++)
//         {
//             LevelData levelData = GenerateLevel(i);
//             levelDataList.Levels.Add(levelData);
//         }
//     }
//
//     private LevelData GenerateLevel(int levelNumber)
//     {
//         LevelData levelData = new LevelData();
//         levelData.NumberOfLevel = levelNumber;
//         levelData.TimeToSpawn = (int)Random.Range(minSpawnTime, maxSpawnTime);
//         levelData.SecondsToWin = CalculateSecondsToWin(levelNumber);
//
//         levelData.EnemyTypesInfo = new List<EnemyTypeInfo>();
//         int remainingSpawnCount = Random.Range(minSpawnCount, maxSpawnCount);
//
//         for (int i = 1; i <= numberOfEnemyTypes; i++)
//         {
//             EnemyTypeInfo enemyTypeInfo = GenerateEnemyTypeInfo(i, remainingSpawnCount);
//             levelData.EnemyTypesInfo.Add(enemyTypeInfo);
//             remainingSpawnCount -= enemyTypeInfo.MaxSpawnCount;
//
//             if (remainingSpawnCount <= 0)
//                 break;
//         }
//
//         return levelData;
//     }
//
//     private int CalculateSecondsToWin(int levelNumber)
//     {
//         // You can adjust the formula to fit your desired difficulty progression
//         return Mathf.RoundToInt(10 - levelNumber * 0.5f);
//     }
//
//     private EnemyTypeInfo GenerateEnemyTypeInfo(int enemyType, int remainingSpawnCount)
//     {
//         EnemyTypeInfo enemyTypeInfo = new EnemyTypeInfo();
//         enemyTypeInfo.EnemyPrefabName = "Enemy " + enemyType;
//         enemyTypeInfo.MaxSpawnCount = Random.Range(1, Mathf.Min(remainingSpawnCount, maxSpawnCount));
//
//         return enemyTypeInfo;
//     }
//
//     private void SaveLevelsToFile()
//     {
//         string json = JsonConvert.SerializeObject(levelDataList, Formatting.Indented);
//         string filePath = Path.Combine(savePath, fileName);
//
//         File.WriteAllText(filePath, json);
//         Debug.Log("Levels saved to: " + filePath);
//     }
// }