using System.Collections.Generic;
using System.IO;
using System.Linq;
using Project.Components.Scripts.Level_System.LevelStructure;
using UnityEngine;

namespace Project.Components.Scripts.Level_System.LevelCreatingSystem
{
    public class NewLevelGenerator : MonoBehaviour
    {
        [FolderPath] [SerializeField] private string _savePath;
        [SerializeField] private string _fileName;
        [SerializeField] private LevelCreatingParameters _parameters;

        private EnemyNumberСombinator _enemyNumberСombinator = new();

        private LevelDataList _levelDataList;

        private int _index = 1;
        private string _separator = ",";
        private string _enemyPrefix = "Enemy ";

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
            var allCombinations =
                _enemyNumberСombinator.GetAllCombinations(_parameters._numberOfEnemyTypes, _parameters._maxSpawnCount);

            if (allCombinations == null || allCombinations.Count == 0)
            {
                Debug.LogError("No combinations found");
                return;
            }

            for (int spawnTime = _parameters._minSpawnTime;
                 spawnTime <= _parameters._maxSpawnTime;
                 spawnTime++)
            {
                for (int secondsToWin = _parameters._minSecondsToWin;
                     secondsToWin <= _parameters._maxSecondsToWin;
                     secondsToWin++)
                {
                    foreach (List<string> combination in allCombinations)
                    {
                        if (combination == null || combination.Count == 0)
                        {
                            Debug.LogError("Empty combination found");
                            continue;
                        }

                        foreach (string s in combination)
                        {
                            if (string.IsNullOrEmpty(s))
                            {
                                Debug.LogError("Empty string found in combination");
                                continue;
                            }

                            CreateLevel(spawnTime, secondsToWin, s);
                        }
                    }
                }
            }

            if (_levelDataList == null || _levelDataList.LevelsCount == 0)
            {
                Debug.LogError("No levels generated");
                return;
            }

            SaveLevelsToFile(_levelDataList);

            Debug.Log($"Levels count: {_levelDataList.LevelsCount}");
        }

        private void CreateLevel(int spawnTime, int secondsToWin, string combination)
        {
            AddLevelToList(new LevelData(
                _index,
                spawnTime,
                secondsToWin,
                GetEnemyTypesInfoFromString(combination)
            ));
        }

        private List<EnemyTypeInfo> GetEnemyTypesInfoFromString(string combination)
        {
            string[] split = combination.Split(_separator);
            var typeInfos = new List<EnemyTypeInfo>();

            for (int i = 0; i < split.Length; i++)
            {
                EnemyTypeInfo enemyTypeInfo =
                    typeInfos.FirstOrDefault(x => x.EnemyPrefabName == $"{_enemyPrefix}{split[i]}");

                if (typeInfos.Count == 0 || enemyTypeInfo.EnemyPrefabName != $"{_enemyPrefix}{split[i]}")
                {
                    typeInfos.Add(new EnemyTypeInfo
                    {
                        EnemyPrefabName = $"{_enemyPrefix}{split[i]}",
                        MaxSpawnCount = 1
                    });
                }
                else
                {
                    enemyTypeInfo.MaxSpawnCount++;
                }
            }

            return typeInfos;
        }

        // private void Test(int countEnemyTypes, int countOfNumberCells)
        // {
        //     Debug.Log("Start test");
        //
        //     var allCombinations = GetAllCombinations(countEnemyTypes, countOfNumberCells);
        //
        //     Debug.Log($"Combinations count: {allCombinations.Count}");
        //
        //     foreach (string s in allCombinations.SelectMany(variable => variable))
        //     {
        //         Debug.Log(s);
        //     }
        //
        //     Debug.Log("End test");
        // }

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