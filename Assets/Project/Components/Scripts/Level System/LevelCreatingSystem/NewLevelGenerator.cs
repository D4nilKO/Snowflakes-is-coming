using System.Collections.Generic;
using System.IO;
using System.Linq;
using Project.Components.Scripts.Level_System.LevelStructure;
using UnityEngine;

namespace Project.Components.Scripts.Level_System.LevelCreatingSystem
{
    public class NewLevelGenerator : MonoBehaviour
    {
        [SerializeField] private int _numberOfEnemyTypes;

        [SerializeField] private int _minSpawnCount;
        [SerializeField] private int _maxSpawnCount;

        [SerializeField] private int _minSpawnTime;
        [SerializeField] private int _maxSpawnTime;

        [SerializeField] private int _minSecondsToWin;
        [SerializeField] private int _maxSecondsToWin;

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
            int levelNumber = 1;

            for (int spawnCount = _minSpawnCount; spawnCount <= _maxSpawnCount; spawnCount++)
            {
                var enemyTypesInfo = new List<EnemyTypeInfo>();

                for (int i = 0; i < _numberOfEnemyTypes; i++)
                {
                    enemyTypesInfo.Add(new EnemyTypeInfo
                    {
                        EnemyPrefabName = $"Enemy {i + 1}",
                        MaxSpawnCount = 0
                    });
                }

                GenerateCombinations(combinations, enemyTypesInfo, 0, totalSpawnCount, levelNumber++, spawnTime,
                    secondsToWin);
            }

            return combinations;
        }

        private void GenerateCombinations(List<LevelData> combinations, List<EnemyTypeInfo> currentCombination,
            int currentTypeIndex, int remainingSpawnCount, int levelNumber, int spawnTime, int secondsToWin)
        {
            if (currentTypeIndex == _numberOfEnemyTypes)
            {
                if (remainingSpawnCount == 0 && currentCombination.Any(e => e.MaxSpawnCount > 0))
                {
                    combinations.Add(new LevelData
                    {
                        NumberOfLevel = levelNumber,
                        TimeToSpawn = spawnTime,
                        SecondsToWin = secondsToWin,
                        EnemyTypesInfo = currentCombination.Where(e => e.MaxSpawnCount > 0).ToList()
                    });
                }

                return;
            }

            for (int i = 0; i <= remainingSpawnCount; i++)
            {
                currentCombination[currentTypeIndex].MaxSpawnCount = i;
                GenerateCombinations(combinations, currentCombination, currentTypeIndex + 1, remainingSpawnCount - i,
                    levelNumber, spawnTime, secondsToWin);
            }
        }

        private void GenerateAndSaveLevels()
        {
            var levels = new List<LevelData>();

            for (int spawnTime = _minSpawnTime; spawnTime <= _maxSpawnTime; spawnTime++)
            {
                for (int secondsToWin = _minSecondsToWin; secondsToWin <= _maxSecondsToWin; secondsToWin++)
                {
                    levels.AddRange(GenerateAllLevelCombinations(_maxSpawnCount, spawnTime, secondsToWin));
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