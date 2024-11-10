using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.LevelSystem.LevelCreatingSystem
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

        [Button("Вывести максимальное количество комбинаций")]
        public void DisplayMaxCombinationsCount()
        {
            Debug.Log($"Максимальное количество комбинаций: {GetMaxCombinationsCount()}");
        }

        public ulong GetMaxCombinationsCount()
        {
            ulong count1 = 0;

            for (int i = _minSpawnCount; i <= _maxSpawnCount; i++)
            {
                count1 += (ulong)Mathf.Pow(_numberOfEnemyTypes, i);
            }

            ulong count2 = (ulong)((_maxSpawnTime - _minSpawnTime + 1) * (_maxSecondsToWin - _minSecondsToWin + 1));

            return count1 * count2;
        }
    }
}