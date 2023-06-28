// using UnityEngine;
// using System.Collections.Generic;
// using System.IO;
// using Newtonsoft.Json;
// using Project.Components.Scripts.Enemies;
//
// #if UNITY_EDITOR
// using UnityEditor;
// #endif
//
// public class LevelGenerator : MonoBehaviour
// {
//     public int numberOfLevels;
//     public int startingLevel;
//     public int maxEnemiesPerLevel;
//     public int initialSpawnCount;
//     public int spawnCountIncrement;
//     public float timeToSpawn;
//     public int secondsToWin;
//     public int minutesToWin;
//     public List<EnemyTypeInfo> enemyTypes;
//
//     private List<LevelData> levels;
//
//     private void Start()
//     {
//         GenerateLevels();
//         SaveLevelsToFile();
//     }
//
//     private void GenerateLevels()
//     {
//         levels = new List<LevelData>();
//
//         int enemyTypeIndex = 0;
//         int totalSpawnCount = 0;
//
//         for (int i = 0; i < numberOfLevels; i++)
//         {
//             LevelData levelData = new LevelData();
//             levelData.numberOfLevel = startingLevel + i;
//             levelData.timeToSpawn = timeToSpawn;
//             levelData.secondsToWin = secondsToWin;
//             levelData.minutesToWin = minutesToWin;
//
//             levelData.enemyTypesInfo = new List<EnemyTypeInfo>();
//
//             int maxEnemiesRemaining = maxEnemiesPerLevel - totalSpawnCount;
//
//             while (maxEnemiesRemaining > 0)
//             {
//                 EnemyTypeInfo enemyTypeInfo = enemyTypes[enemyTypeIndex];
//
//                 int maxSpawnCountForType = Mathf.Min(enemyTypeInfo.maxSpawnCount, maxEnemiesRemaining);
//                 int spawnCountForType = Mathf.Min(maxSpawnCountForTyp);
//
//                 if (spawnCountForType > 0)
//                 {
//                     EnemyTypeInfo newEnemyTypeInfo = new EnemyTypeInfo();
//                     newEnemyTypeInfo.enemyPrefabName = enemyTypeInfo.enemyPrefabName;
//                     newEnemyTypeInfo.maxSpawnCount = spawnCountForType;
//
//                     levelData.enemyTypesInfo.Add(newEnemyTypeInfo);
//
//                     totalSpawnCount += spawnCountForType;
//                     maxEnemiesRemaining -= spawnCountForType;
//
//                     if (maxEnemiesRemaining == 0)
//                         break;
//                 }
//
//                 enemyTypeIndex++;
//                 if (enemyTypeIndex >= enemyTypes.Count)
//                     enemyTypeIndex = 0;
//             }
//
//             levels.Add(levelData);
//
//             if (totalSpawnCount >= maxEnemiesPerLevel)
//                 break;
//         }
//     }
//
//     private void SaveLevelsToFile()
//     {
//         LevelCollectionData collectionData = new LevelCollectionData();
//         collectionData.levels = levels;
//
//         string jsonData = JsonConvert.SerializeObject(collectionData, Formatting.Indented);
//         string filePath = Application.dataPath + "/Levels/level_data.json";
//         File.WriteAllText(filePath, jsonData);
//
// #if UNITY_EDITOR
//         AssetDatabase.Refresh();
// #endif
//
//         Debug.Log("Levels saved to: " + filePath);
//     }
// }
//
// [System.Serializable]
// public class LevelData
// {
//     public int numberOfLevel;
//     public float timeToSpawn;
//     public int secondsToWin;
//     public int minutesToWin;
//     public List<EnemyTypeInfo> enemyTypesInfo;
// }
//
// [System.Serializable]
// public class LevelCollectionData
// {
//     public List<LevelData> levels;
// }
