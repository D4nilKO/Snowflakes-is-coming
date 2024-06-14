using System;
using System.Collections.Generic;

// С этим файлом нужно быть аккуартнее, так как названия полей и классов
// внутри JSON и pure класов должны быть одинаковыми
namespace Project.Components.Scripts.Level_System.LevelStructure
{
    [Serializable]
    public struct LevelData
    {
        public LevelData(int numberOfLevel, int timeToSpawn, int secondsToWin, List<EnemyTypeInfo> enemyTypesInfo)
        {
            NumberOfLevel = numberOfLevel;
            TimeToSpawn = timeToSpawn;
            SecondsToWin = secondsToWin;
            EnemyTypesInfo = enemyTypesInfo;
        }

        public int NumberOfLevel;
        public int TimeToSpawn;
        public int SecondsToWin;

        public List<EnemyTypeInfo> EnemyTypesInfo;

        public int GetTimeToSurvive()
        {
            return (TimeToSpawn * EnemyTypesInfo.Count) + SecondsToWin;
        }
    }
}