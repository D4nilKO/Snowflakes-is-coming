using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Project.Components.Scripts.Level_System.LevelStructure;
using UnityEngine;

namespace Project.Components.Scripts.Level_System.LevelCreatingSystem
{
    public class NewLevelGenerator : MonoBehaviour
    {
        [SerializeField] private LevelCreatingParameters _parameters;

        [FolderPath] [SerializeField] private string _savePath;

        [SerializeField] private string _fileName;

        private LevelDataList _levelDataList;
        private int _index = 1;
        private string _separator = ",";

        private void Awake()
        {
            _levelDataList = new LevelDataList();
            _levelDataList.Levels = new List<LevelData>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                GenerateAndSaveLevels();
            }
        }

        #region Generate Levels

        private void GenerateAndSaveLevels()
        {
            for (int spawnTime = _parameters._minSpawnTime;
                 spawnTime <= _parameters._maxSpawnTime;
                 spawnTime++)
            {
                for (int secondsToWin = _parameters._minSecondsToWin;
                     secondsToWin <= _parameters._maxSecondsToWin;
                     secondsToWin++)
                {
                    for (int maxEnemyTypes = 1; maxEnemyTypes <= _parameters._numberOfEnemyTypes; maxEnemyTypes++)
                    {
                        for (int enemyType = 1; enemyType <= _parameters._numberOfEnemyTypes; enemyType++)
                        {
                            var levelData = new LevelData
                            (
                                0,
                                spawnTime,
                                secondsToWin,
                                new List<EnemyTypeInfo>()
                            );

                            for (int spawnCount = _parameters._minSpawnCount;
                                 spawnCount <= _parameters._maxSpawnCount;
                                 spawnCount++)
                            {
                                var enemy = new EnemyTypeInfo();

                                enemy.EnemyPrefabName = $"Enemy {enemyType}";
                                enemy.MaxSpawnCount = spawnCount;

                                levelData.EnemyTypesInfo.Add(enemy);
                            }

                            AddLevelToList(levelData);
                        }
                    }
                }
            }

            SaveLevelsToFile(_levelDataList);
        }

        private List<string> MergeEnemyInCombinations(List<string> combination)
        {
        }

        private List<EnemyTypeInfo> MergeStringIntoEnemies(string combination)
        {
            var split = combination.Split(_separator);
            var enemyTypesCount = split.Distinct().Count();

            var a = new EnemyTypeInfo[enemyTypesCount];

            foreach (EnemyTypeInfo typeInfo in a)
            {
                typeInfo.MaxSpawnCount = 1;
            }

            for (int i = 0; i < split.Length - 1; i++)
            {
                if (split[i] == split[i + 1])
                {
                    a[i].MaxSpawnCount++;
                }
            }

            return a.ToList();
        }

        private List<EnemyTypeInfo> GetEnemiesCombination(string combination)
        {
            var a = MergeStringIntoEnemies(combination);

            return combination
                .Split(',')
                .Select(int.Parse)
                .Select(enemyType => new EnemyTypeInfo
                {
                    EnemyPrefabName = $"Enemy {enemyType}",
                    MaxSpawnCount = _parameters._maxSpawnCount,
                })
                .ToList();
        }

        private List<string> GetAllNumberCombinations(int n, int m)
        {
            var results = new List<string>();
            GenerateNumberCombinations(results, new List<int>(), n, m, 1);
            return results;
        }

        private void GenerateNumberCombinations(List<string> results, List<int> current, int n, int m, int start)
        {
            if (current.Count == m)
            {
                results.Add(string.Join(_separator, current));
                return;
            }

            for (int i = start; i <= n; i++)
            {
                current.Add(i);
                GenerateNumberCombinations(results, current, n, m, i);
                current.RemoveAt(current.Count - 1);
            }
        }

        #endregion

        private void AddLevelToList(LevelData levelData)
        {
            levelData.NumberOfLevel = _index++;
            _levelDataList.Levels.Add(levelData);
        }

        private void SaveLevelsToFile(LevelDataList levelDataList)
        {
            if (Directory.Exists(_savePath) == false)
            {
                Directory.CreateDirectory(_savePath);
            }

            if (!Path.GetExtension(_fileName).Equals(".json", System.StringComparison.OrdinalIgnoreCase))
            {
                _fileName = Path.ChangeExtension(_fileName, ".json");
            }

            var fullPath = Path.Combine(_savePath, _fileName ?? "levels.json");

            var json = JsonUtility.ToJson(levelDataList, true);
            File.WriteAllText(fullPath, json);

            Debug.Log($"Levels saved to {fullPath}");
        }
    }
}