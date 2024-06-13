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

        public ulong GetMaxCombinationsCount()
        {
            ulong count1 = (ulong)(Mathf.Pow(_numberOfEnemyTypes, _maxSpawnCount + 1) - _numberOfEnemyTypes);
            ulong count2 = (ulong)((_maxSpawnTime - _minSpawnTime + 1) * (_maxSecondsToWin - _minSecondsToWin + 1));

            return count1 * count2;
        }
    }
}