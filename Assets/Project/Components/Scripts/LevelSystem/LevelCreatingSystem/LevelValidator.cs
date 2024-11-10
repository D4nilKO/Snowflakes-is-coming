using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project.LevelSystem.LevelStructure;
using UnityEngine;

namespace Project.LevelSystem.LevelCreatingSystem
{
    public class LevelValidator : MonoBehaviour
    {
        [SerializeField] private JsonLevelParser _jsonLevelParser;
        [SerializeField] private LevelCreatingParameters _parameters;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                ValidateLevels(_jsonLevelParser.GetLevelDataList());
            }
        }

        private void ValidateLevels(LevelDataList levelDataList)
        {
            if (levelDataList?.Levels == null)
            {
                Debug.LogError("Invalid file format or no levels found.");
                return;
            }

            var levels = levelDataList.Levels;
            var logs = new StringBuilder();

            var emptyEnemies = FindLevelsWithEmptyEnemies(levels);
            if (emptyEnemies.Count > 0)
            {
                logs.AppendLine("Levels with empty enemies found:");
                foreach (var level in emptyEnemies)
                {
                    logs.AppendLine($"Level {level.NumberOfLevel} has enemies with MaxSpawnCount = 0");
                }
            }

            if (ValidLevelsCount(levels) == false)
            {
                logs.AppendLine("Not all levels have the same number of enemies.");

                logs.AppendLine($"Number of levels: {levels.Count}");
                logs.Append($" Number of combinations: {_parameters.GetMaxCombinationsCount()}");
            }

            var duplicateLevels = FindConsecutiveDuplicateEnemies(levels);
            if (duplicateLevels.Count > 0)
            {
                logs.AppendLine("Duplicate levels found:");
                foreach (var level in duplicateLevels)
                {
                    logs.AppendLine($"Level {level.NumberOfLevel} is duplicate.");
                }
            }

            var invalidSpawnCounts = FindLevelsWithInvalidSpawnCounts(levels);
            if (invalidSpawnCounts.Count > 0)
            {
                logs.AppendLine("Levels with invalid spawn counts found:");
                foreach (var level in invalidSpawnCounts)
                {
                    logs.AppendLine($"Level {level.NumberOfLevel} has enemies with invalid spawn counts.");
                }
            }

            var invalidTimes = FindLevelsWithInvalidTimes(levels);
            if (invalidTimes.Count > 0)
            {
                logs.AppendLine("Levels with invalid times found:");
                foreach (var level in invalidTimes)
                {
                    logs.AppendLine($"Level {level.NumberOfLevel} has invalid spawn time or win time.");
                }
            }

            var totalEnemyCounts = FindLevelsWithInvalidTotalEnemyCount(levels);
            if (totalEnemyCounts.Count > 0)
            {
                logs.AppendLine("Levels with invalid total enemy count found:");
                foreach (var level in totalEnemyCounts)
                {
                    logs.AppendLine($"Level {level.NumberOfLevel} has invalid total enemy count.");
                }
            }

            if (logs.Length > 0)
            {
                Debug.LogError(logs.ToString());
            }
            else
            {
                Debug.Log("All tests passed.");
            }
        }

        private List<LevelData> FindLevelsWithEmptyEnemies(List<LevelData> levels)
        {
            return levels
                .Where(level => level.EnemyTypesInfo.Any(enemy => enemy.MaxSpawnCount == 0))
                .ToList();
        }

        private bool ValidLevelsCount(List<LevelData> levels)
        {
            return _parameters.GetMaxCombinationsCount() == (ulong)levels.Count;
        }

        private List<LevelData> FindConsecutiveDuplicateEnemies(List<LevelData> levels)
        {
            var consecutiveDuplicateEnemies = new List<LevelData>();

            foreach (var level in levels)
            {
                var duplicateEnemies = new List<EnemyTypeInfo>();
                var currentEnemy = level.EnemyTypesInfo.FirstOrDefault();

                for (int i = 1; i < level.EnemyTypesInfo.Count; i++)
                {
                    var nextEnemy = level.EnemyTypesInfo[i];
                    if (currentEnemy.EnemyPrefabName == nextEnemy.EnemyPrefabName)
                    {
                        duplicateEnemies.Add(currentEnemy);
                        duplicateEnemies.Add(nextEnemy);
                    }
                    else
                    {
                        if (duplicateEnemies.Count > 1)
                        {
                            consecutiveDuplicateEnemies.Add(level);
                            break;
                        }

                        duplicateEnemies.Clear();
                    }

                    currentEnemy = nextEnemy;
                }
            }

            return consecutiveDuplicateEnemies;
        }

        private List<LevelData> FindLevelsWithInvalidSpawnCounts(List<LevelData> levels)
        {
            return levels
                .Where(level => level.EnemyTypesInfo
                    .Any(e => e.MaxSpawnCount < _parameters._minSpawnCount ||
                              e.MaxSpawnCount > _parameters._maxSpawnCount))
                .ToList();
        }

        private List<LevelData> FindLevelsWithInvalidTimes(List<LevelData> levels)
        {
            return levels
                .Where(level => level.TimeToSpawn < _parameters._minSpawnTime ||
                                level.TimeToSpawn > _parameters._maxSpawnTime ||
                                level.SecondsToWin < _parameters._minSecondsToWin ||
                                level.SecondsToWin > _parameters._maxSecondsToWin)
                .ToList();
        }

        private List<LevelData> FindLevelsWithInvalidTotalEnemyCount(List<LevelData> levels)
        {
            return levels
                .Where(level => level.EnemyTypesInfo.Sum(e => e.MaxSpawnCount) > _parameters._maxSpawnCount)
                .ToList();
        }
    }
}