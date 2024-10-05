using System;
using System.Collections.Generic;

// С этим файлом нужно быть аккуартнее, так как названия полей и классов
// внутри JSON и pure класов должны быть одинаковыми
namespace Project.Components.Scripts.Level_System.LevelStructure
{
    [Serializable]
    public class LevelData
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

        public bool IsEqual(LevelData newLevel)
        {
            if (GetTimeToSurvive() != newLevel.GetTimeToSurvive())
            {
                return false;
            }

            if (TimeToSpawn != newLevel.TimeToSpawn)
            {
                return false;
            }

            if (SecondsToWin != newLevel.SecondsToWin)
            {
                return false;
            }

            if (EnemyTypesEquals(newLevel) == false)
            {
                return false;
            }

            return true;
        }

        public bool EnemyTypesEquals(LevelData other)
        {
            if (EnemyTypesInfo.Count != other.EnemyTypesInfo.Count)
            {
                return false;
            }

            for (int i = 0; i < EnemyTypesInfo.Count; i++)
            {
                if (EnemyTypesInfo[i].IsEquals(other.EnemyTypesInfo[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}