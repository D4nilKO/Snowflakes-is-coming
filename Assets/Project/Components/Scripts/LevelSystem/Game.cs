﻿using System;
using Project.Data;
using Project.LevelSystem.LevelStructure;
using UnityEngine;

namespace Project.LevelSystem
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private JsonLevelParser _jsonLevelParser;

        [SerializeField]
        private ProgressData _progressData;

        [SerializeField, Header("Ниже данные для ознакомления, загружаются в начале игры")]
        private LevelData _currentLevelData;

        private LevelDataList _levelDataList;

        public event Action<LevelData> LevelSettingsReady;

        public void FetchCurrentLevelSettings()
        {
            _levelDataList = _jsonLevelParser.GetLevelDataList();

            FetchLevelSettings(_levelDataList, _progressData.CurrentLevelNumber);
        }

        private void FetchLevelSettings(LevelDataList levelDataList, int levelNumber)
        {
            if (levelDataList == null)
            {
                Debug.LogError("LevelDataList is null");
                return;
            }

            if (levelDataList.LevelsCount == 0)
            {
                Debug.LogError("LevelDataList is empty");
                return;
            }

            int maxLevelsCount = levelDataList.LevelsCount;

            _progressData.SetMaxLevelsCount(maxLevelsCount);

            if (levelNumber > maxLevelsCount)
            {
                Debug.LogError($"Уровень {levelNumber} не найден в JSON файле");
                return;
            }

            Debug.Log($"Данные уровня №{levelNumber} загружены");

            _currentLevelData = levelDataList.GetLevel(levelNumber - 1);
            LevelSettingsReady?.Invoke(_currentLevelData);
        }

        private void Start()
        {
            FetchCurrentLevelSettings();
            Debug.Log("level settings start");
        }
    }
}