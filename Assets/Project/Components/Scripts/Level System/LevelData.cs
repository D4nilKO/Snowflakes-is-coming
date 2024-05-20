using System;
using System.Collections.Generic;
using Project.Components.Scripts.Entities.Enemies;
using UnityEngine.Serialization;

namespace Project.Components.Scripts.Level_System
{
    [Serializable]
    public class LevelData
    {
        [FormerlySerializedAs("numberOfLevel")] public int NumberOfLevel;
        [FormerlySerializedAs("timeToSpawn")] public int TimeToSpawn;
        [FormerlySerializedAs("secondsToWin")] public int SecondsToWin;
        [FormerlySerializedAs("minutesToWin")] public int MinutesToWin;
        [FormerlySerializedAs("enemyTypesInfo")] public List<EnemyTypeInfo> EnemyTypesInfo;
    }

    [Serializable]
    public class LevelDataList
    {
        [FormerlySerializedAs("levels")] public List<LevelData> Levels;
    }
}