using System.Collections.Generic;
using UnityEngine;

namespace Project.Components.Scripts.Level_System.LevelCreatingSystem
{
    [CreateAssetMenu(fileName = "LevelCreatingParameters", menuName = "Level System/LevelCreatingParameters")]
    public class LevelCreatingParameters : ScriptableObject
    {
        public int _numberOfEnemyTypes;
        public int _minSpawnCount;
        public int _maxSpawnCount;
        public int _minSpawnTime;
        public int _maxSpawnTime;
        public int _minSecondsToWin;
        public int _maxSecondsToWin;


        public int GetMaxCombinationsCount()
        {
            int count1 = (int)((Mathf.Pow(_numberOfEnemyTypes, _maxSpawnCount + 1) - _numberOfEnemyTypes) / 2);
            int count2 = (_maxSpawnTime - _minSpawnTime) * (_maxSecondsToWin - _minSecondsToWin);

            return count1 * count2;
        }
    }
}