// using System.Collections.Generic;
// using Newtonsoft.Json;
// using Project.Components.Scripts.Enemies;
// using UnityEngine;
//
// namespace Project.Components.Scripts.Level_System
// {
//     public class LevelGenerator : MonoBehaviour
//     {
//         public int NumberOfLevels;
//         public int NumberOfEnemyTypes;
//         public int MinEnemiesPerLevel;
//         public int MaxEnemiesPerLevel;
//         public float MinSpawnTime;
//         public float MaxSpawnTime;
//         public string SavePath;
//         public string FileName;
//
//         private System.Random random;
//
//         private void Start()
//         {
//             random = new System.Random();
//             GenerateLevels();
//         }
//
//         private void GenerateLevels()
//         {
//             LevelDataList levelDataList = new LevelDataList();
//             levelDataList.levels = new List<LevelData>();
//
//             float currentSpawnTime = MinSpawnTime;
//             int currentEnemyCount = MinEnemiesPerLevel;
//
//             for (int i = 1; i <= NumberOfLevels; i++)
//             {
//                 LevelData levelData = new LevelData();
//                 levelData.numberOfLevel = i;
//                 levelData.timeToSpawn = (int)currentSpawnTime;
//                 levelData.enemyTypesInfo = GenerateEnemyTypesInfo(currentEnemyCount);
//
//                 levelDataList.levels.Add(levelData);
//
//                 currentSpawnTime = Mathf.Lerp(MinSpawnTime, MaxSpawnTime, (float)i / NumberOfLevels);
//                 currentEnemyCount =
//                     Mathf.RoundToInt(Mathf.Lerp(MinEnemiesPerLevel, MaxEnemiesPerLevel, (float)i / NumberOfLevels));
//             }
//
//             string jsonData = JsonConvert.SerializeObject(levelDataList, Formatting.Indented);
//             System.IO.File.WriteAllText(SavePath + "/" + FileName, jsonData);
//
//             Debug.Log("Level data saved to: " + SavePath + "/" + FileName);
//         }
//
//         private List<EnemyTypeInfo> GenerateEnemyTypesInfo()
//         {
//             List<EnemyTypeInfo> enemyTypesInfo = new List<EnemyTypeInfo>();
//
//             for (int i = 1; i <= NumberOfEnemyTypes; i++)
//             {
//                 EnemyTypeInfo enemyTypeInfo = new EnemyTypeInfo();
//                 enemyTypeInfo.enemyPrefabName = "Enemy" + i;
//                 enemyTypeInfo.maxSpawnCount = random.Next(MinEnemiesPerLevel, MaxEnemiesPerLevel + 1);
//
//                 if (enemyTypeInfo.maxSpawnCount < MinEnemiesPerLevel)
//                 {
//                     enemyTypeInfo.maxSpawnCount = MinEnemiesPerLevel;
//                 }
//
//                 enemyTypesInfo.Add(enemyTypeInfo);
//             }
//
//             return enemyTypesInfo;
//         }
//     }
// }