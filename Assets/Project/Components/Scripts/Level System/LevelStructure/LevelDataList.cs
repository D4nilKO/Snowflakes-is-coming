using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Project.Components.Scripts.Level_System.LevelStructure
{
    [Serializable]
    public class LevelDataList
    {
        public LevelDataList()
        {
            Levels = new List<LevelData>();
        }

        public List<LevelData> Levels;

        public LevelData GetLevel(int index) => Levels[index];

        public int LevelsCount
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
    }
}