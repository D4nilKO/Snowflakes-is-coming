using System;
using System.Collections.Generic;
using System.Linq;
using Project.Components.Scripts.Level_System.LevelCreatingSystem;
using Project.Components.Scripts.Level_System.LevelStructure;
using UnityEngine;

namespace Project.Components.Scripts.Level_System.LevelValidation
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

            var emptyEnemies = FindLevelsWithEmptyEnemies(levels);
            if (emptyEnemies.Count > 0)
            {
                Debug.LogError("Levels with empty enemies found:");
                foreach (var level in emptyEnemies)
                {
                    Debug.LogError($"Level {level.NumberOfLevel} has enemies with MaxSpawnCount = 0");
                }
            }
            else
            {
                Debug.Log("No levels with empty enemies found.");
            }

            var duplicateLevels = FindDuplicateLevels(levels);
            if (duplicateLevels.Count > 0)
            {
                Debug.LogError("Duplicate levels found:");
                foreach (var level in duplicateLevels)
                {
                    Debug.LogError($"Level {level.NumberOfLevel} is duplicate.");
                }
            }
            else
            {
                Debug.Log("No duplicate levels found.");
            }

            var invalidEnemyNames = FindLevelsWithDuplicateEnemyNames(levels);
            if (invalidEnemyNames.Count > 0)
            {
                Debug.LogError("Levels with duplicate enemy names found:");
                foreach (var level in invalidEnemyNames)
                {
                    Debug.LogError($"Level {level.NumberOfLevel} has duplicate enemy names.");
                }
            }
            else
            {
                Debug.Log("No levels with duplicate enemy names found.");
            }

            var invalidSpawnCounts = FindLevelsWithInvalidSpawnCounts(levels);
            if (invalidSpawnCounts.Count > 0)
            {
                Debug.LogError("Levels with invalid spawn counts found:");
                foreach (var level in invalidSpawnCounts)
                {
                    Debug.LogError($"Level {level.NumberOfLevel} has enemies with invalid spawn counts.");
                }
            }
            else
            {
                Debug.Log("No levels with invalid spawn counts found.");
            }

            var invalidTimes = FindLevelsWithInvalidTimes(levels);
            if (invalidTimes.Count > 0)
            {
                Debug.LogError("Levels with invalid times found:");
                foreach (var level in invalidTimes)
                {
                    Debug.LogError($"Level {level.NumberOfLevel} has invalid spawn time or win time.");
                }
            }
            else
            {
                Debug.Log("No levels with invalid times found.");
            }

            var totalEnemyCounts = FindLevelsWithInvalidTotalEnemyCount(levels);
            if (totalEnemyCounts.Count > 0)
            {
                Debug.LogError("Levels with invalid total enemy count found:");
                foreach (var level in totalEnemyCounts)
                {
                    Debug.LogError($"Level {level.NumberOfLevel} has invalid total enemy count.");
                }
            }
            else
            {
                Debug.Log("No levels with invalid total enemy count found.");
            }
        }

        private List<LevelData> FindLevelsWithEmptyEnemies(List<LevelData> levels)
        {
            return levels
                .Where(level => level.EnemyTypesInfo.Any(enemy => enemy.MaxSpawnCount == 0))
                .ToList();
        }

        private List<LevelData> FindDuplicateLevels(List<LevelData> levels)
        {
            var duplicateLevels = new List<LevelData>();

            var levelGroups = levels
                .GroupBy(level => new
                {
                    level.TimeToSpawn,
                    level.SecondsToWin,
                    Enemies = level.EnemyTypesInfo
                        .OrderBy(enemy => enemy.EnemyPrefabName)
                        .Select(enemy => new { enemy.EnemyPrefabName, enemy.MaxSpawnCount })
                        .ToList()
                });

            foreach (var group in levelGroups)
            {
                if (group.Count() > 1)
                {
                    duplicateLevels.AddRange(group);
                }
            }

            return duplicateLevels;
        }

        private List<LevelData> FindLevelsWithDuplicateEnemyNames(List<LevelData> levels)
        {
            return levels
                .Where(level => level.EnemyTypesInfo
                    .GroupBy(e => e.EnemyPrefabName)
                    .Any(group => group.Count() > 1))
                .ToList();
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