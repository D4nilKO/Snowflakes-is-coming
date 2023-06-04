using System;
using Project.Components.Scripts.Enemies;
using System.Collections.Generic;

namespace Project.Components.Scripts.Level_System
{
    [Serializable]
    public class LevelData
    {
        public int numberOfLevel;
        public int timeToSpawn;
        public int secondsToWin;
        public int minutesToWin;
        public List<EnemyTypeInfo> enemyTypesInfo;
    }

    [Serializable]
    public class LevelDataList
    {
        public List<LevelData> levels;
    }
}