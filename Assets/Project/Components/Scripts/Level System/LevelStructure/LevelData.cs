using System;
using System.Collections.Generic;
using System.Linq;

// С этим файлом нужно быть аккуартнее, так как названия полей и классов
// внутри JSON и pure класов должны быть одинаковыми
namespace Project.Components.Scripts.Level_System.LevelStructure
{
    [Serializable]
    public class LevelData
    {
        public int NumberOfLevel;
        public int TimeToSpawn;
        public int SecondsToWin;

        public List<EnemyTypeInfo> EnemyTypesInfo;

        public float TimeToSurvive => EnemyTypesInfo.Sum(t => t.MaxSpawnCount * TimeToSpawn) + SecondsToWin;
    }
}