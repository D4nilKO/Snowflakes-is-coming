using System.Collections.Generic;
using System.Linq;
using Project.LevelSystem.LevelStructure;
using UnityEngine;

namespace Project.LevelSystem.LevelCreatingSystem
{
    public class LevelChooser : MonoBehaviour
    {
        [SerializeField] private LevelDataProcessor _levelDataProcessor;

        private LevelDataList _levelDataList;
        private Dictionary<LevelData, float> _levelsDictionary;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SortLevels();
            }
        }

        private void SortLevels()
        {
            _levelsDictionary = _levelDataProcessor.GetLevelsDifficultyData();

            _levelsDictionary = _levelsDictionary
                .OrderBy(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);

            ChangeLevelsNumeration();

            _levelDataProcessor.DisplayLevelsDifficulty(_levelsDictionary);
        }

        private void ChangeLevelsNumeration()
        {
            for (int i = 0; i < _levelsDictionary.Keys.Count; i++)
            {
                LevelData element = _levelsDictionary.Keys.ElementAt(i);
                element.NumberOfLevel = i + 1;
            }
        }
    }
}