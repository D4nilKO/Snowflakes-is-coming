using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.LevelSystem.LevelStructure
{
    [Serializable]
    public class LevelDataList
    {
        public List<LevelData> Levels = new();

        public LevelData GetLevel(int index) => Levels[index];

        public int Count
        {
            get
            {
                if (Levels == null)
                {
                    throw new InvalidOperationException();
                }

                return Levels.Count;
            }
        }

        public bool CheckLevelUniqueness(LevelData newLevel)
        {
            return Levels.All(oldLevel => newLevel.IsEqual(oldLevel) == false);
        }
        
    }
}