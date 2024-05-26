using System;
using System.Collections.Generic;
using System.Linq;
using Project.Components.Scripts.Entities.Enemies;

namespace Project.Components.Scripts.Level_System
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

    [Serializable]
    public class LevelDataList
    {
        public List<LevelData> Levels;
    }
}