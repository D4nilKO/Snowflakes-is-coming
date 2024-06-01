using UnityEngine;

namespace Project.Components.Scripts.Data
{
    public class ProgressData : MonoBehaviour
    {
        private int _maxLevelsCount;
        
        public int CurrentLevelNumber { get; private set; } = 1;
        public int UnlockedLevelNumber { get; private set; }

        public void IncreaseCurrentLevel()
        {
            if (CurrentLevelNumber != _maxLevelsCount)
            {
                UnlockedLevelNumber = ++CurrentLevelNumber;
            }
        }

        public void SetMaxLevelsCount(int maxLevelsCount)
        {
            if (maxLevelsCount <= 0)
            {
                Debug.LogError("Максимальное количество уровней должно быть больше нуля");
                return;
            }

            _maxLevelsCount = maxLevelsCount;
        }
    }
}