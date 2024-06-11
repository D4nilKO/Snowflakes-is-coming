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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                GenerateAndSaveLevels();
            }
        }

        #region Generate Levels

        private List<LevelData> GenerateAllLevelCombinations(int totalSpawnCount, int spawnTime, int secondsToWin)
        {
            var combinations = new List<LevelData>();

            // Генерация всех возможных комбинаций
            for (int totalEnemies = _parameters._minSpawnCount;
                 totalEnemies <= _parameters._maxSpawnCount;
                 totalEnemies++)
            {
                var enemyTypesInfo = new List<List<EnemyTypeInfo>>();

                // Генерация врагов с различными комбинациями
                GenerateEnemyCombinations(enemyTypesInfo, totalEnemies, 0, new List<EnemyTypeInfo>());

                // Фильтрация и добавление комбинаций
                foreach (var enemyCombination in enemyTypesInfo)
                {
                    // Удаление врагов с нулевым количеством
                    var validCombination = enemyCombination.Where(enemy => enemy.MaxSpawnCount > 0).ToList();

                    if (validCombination.Any())
                    {
                        combinations.Add(new LevelData
                        {
                            NumberOfLevel = combinations.Count + 1,
                            TimeToSpawn = spawnTime,
                            SecondsToWin = secondsToWin,
                            EnemyTypesInfo = validCombination
                        });
                    }
                }
            }

            return combinations;
        }

        private void GenerateEnemyCombinations(List<List<EnemyTypeInfo>> result, int remainingEnemies,
            int currentTypeIndex, List<EnemyTypeInfo> currentCombination)
        {
            if (currentTypeIndex == _parameters._numberOfEnemyTypes)
            {
                if (remainingEnemies == 0)
                {
                    result.Add(new List<EnemyTypeInfo>(currentCombination));
                }

                return;
            }

            for (int i = 0; i <= remainingEnemies; i++)
            {
                var newCombination = new List<EnemyTypeInfo>(currentCombination);
                newCombination.Add(new EnemyTypeInfo
                {
                    EnemyPrefabName = $"Enemy {currentTypeIndex + 1}",
                    MaxSpawnCount = i
                });
                GenerateEnemyCombinations(result, remainingEnemies - i, currentTypeIndex + 1, newCombination);
            }
        }

        private void GenerateAndSaveLevels()
        {
            var levels = new List<LevelData>();

            for (int spawnTime = _parameters._minSpawnTime;
                 spawnTime <= _parameters._maxSpawnTime;
                 spawnTime++)
            {
                for (int secondsToWin = _parameters._minSecondsToWin;
                     secondsToWin <= _parameters._maxSecondsToWin;
                     secondsToWin++)
                {
                    levels.AddRange(GenerateAllLevelCombinations(_parameters._maxSpawnCount, spawnTime, secondsToWin));
                }
            }

            SaveLevelsToFile(levels);
        }

        #endregion

        private void SaveLevelsToFile(List<LevelData> levels)
        {
            var levelDataList = new LevelDataList { Levels = levels };

            if (!Directory.Exists(_savePath))
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