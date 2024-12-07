using System;
using Project.Data;
using Project.LevelSystem.LevelStructure;
using Project.Timing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.LevelSystem
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private JsonLevelParser _jsonLevelParser;

        [SerializeField]
        private PauseHandler _pauseHandler;

        [SerializeField, Header("Ниже данные для ознакомления, загружаются в начале игры")]
        private LevelData _currentLevelData;

        [ShowInInspector]
        private int TimeToWin => _timeToWin;

        private int _timeToWin;

        private LevelDataList _levelDataList;

        public event Action<LevelData> LevelSettingsReady;

        public void FetchCurrentLevelSettings()
        {
            _levelDataList = _jsonLevelParser.GetLevelDataList();

            FetchLevelSettings(_levelDataList, ProgressData.CurrentLevelNumber);
        }

        private void Awake()
        {
            this.ValidateSerializedFields();
        }

        private void Start()
        {
            FetchCurrentLevelSettings();
            _pauseHandler.InGamePause();
        }

        private void FetchLevelSettings(LevelDataList levelDataList, int levelNumber)
        {
            if (levelDataList == null)
            {
                Debug.LogError("LevelDataList is null");
                return;
            }

            if (levelDataList.Count == 0)
            {
                Debug.LogError("LevelDataList is empty");
                return;
            }

            int maxLevelsCount = levelDataList.Count;

            ProgressData.SetMaxLevelsCount(maxLevelsCount);

            if (levelNumber > maxLevelsCount)
            {
                Debug.LogError($"Уровень {levelNumber} не найден в JSON файле");
                return;
            }

            Debug.Log($"Данные уровня №{levelNumber} загружены");

            _currentLevelData = levelDataList.GetLevel(levelNumber - 1);
            LevelSettingsReady?.Invoke(_currentLevelData);

            _timeToWin = _currentLevelData.GetTimeToSurvive();

            _pauseHandler.ForceInGamePause();
        }
    }
}