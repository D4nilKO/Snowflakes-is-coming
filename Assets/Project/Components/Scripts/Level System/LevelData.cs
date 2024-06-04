using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Project.Components.Scripts.Entities.Enemies;

// С этим файлом нужно быть аккуартнее, так как названия полей и классов
// внутри JSON и pure класов должны быть одинаковыми
namespace Project.Components.Scripts.Level_System
{
    [Serializable]
    public class LevelData
    {
        public LevelData(int numberOfLevel, int timeToSpawn, int secondsToWin,
            [NotNull] List<EnemyTypeInfo> enemyTypesInfo)
        {
            NumberOfLevel = numberOfLevel;
            TimeToSpawn = timeToSpawn;
            SecondsToWin = secondsToWin;
            EnemyTypesInfo = enemyTypesInfo ?? throw new ArgumentNullException(nameof(enemyTypesInfo));
        }

        public readonly int NumberOfLevel;
        public readonly int TimeToSpawn;
        public readonly int SecondsToWin;
        
        public IReadOnlyList<EnemyTypeInfo> EnemyTypesInfo;

        public float TimeToSurvive => EnemyTypesInfo.Sum(t => t.MaxSpawnCount * TimeToSpawn) + SecondsToWin;
    }
}