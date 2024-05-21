using System;
using System.Collections.Generic;
using Project.Components.Scripts.Entities.Enemies;

namespace Project.Components.Scripts.Level_System
{
    [Serializable]
    public class LevelData
    {
        public int NumberOfLevel;
        public int TimeToSpawn;
        public int SecondsToWin;
        public int MinutesToWin;
        public List<EnemyTypeInfo> EnemyTypesInfo;
    }

    [Serializable]
    public class LevelDataList
    {
        public List<LevelData> Levels;
    }
}