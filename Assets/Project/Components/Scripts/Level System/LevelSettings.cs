using System;
using System.Collections.Generic;
using System.Linq;
using Project.Components.Scripts.Data;
using Project.Components.Scripts.Entities.Enemies;
using UnityEngine;

namespace Project.Components.Scripts.Level_System
{
    [DisallowMultipleComponent]
    public class LevelSettings : MonoBehaviour
    {
        [SerializeField] private string _jsonFileName;

        [SerializeField] private LevelData _currentLevelData;
        
        [Header("Период спавна врагов")] [SerializeField]
        private int _timeToSpawn;
        
        [Header("Время, которое нужно ДОПОЛНИТЕЛЬНО продержаться")]
        [SerializeField]
        private int _secondsToWin;

        [SerializeField] [Space(10)]
        private List<EnemyTypeInfo> _enemyTypesInfo;

        private TimeManager _timeManager;
        private EnemySpawner _enemySpawner;
        
        private int _mainTimeToSurvive;
        private int _secondsInMinute = 60;
        
        public event Action<LevelData> LevelSettingsReady;

        private void Awake()
        {
            _timeManager = FindObjectOfType<TimeManager>();
            _enemySpawner = FindObjectOfType<EnemySpawner>();

            LoadLevelSettings();
        }

        private void CalculateTimeToSurvive()
        {
            _mainTimeToSurvive = _enemyTypesInfo.Sum(t => t.MaxSpawnCount * _timeToSpawn);
            _mainTimeToSurvive += _secondsToWin;
        }

        private void LoadLevelSettings()
        {
            TextAsset json = Resources.Load<TextAsset>(_jsonFileName);

            if (json == null)
            {
                Debug.LogError($"Указанный JSON файл не найден: {_jsonFileName}");
                return;
            }

            LevelDataList levelDataList = JsonUtility.FromJson<LevelDataList>(json.text);
            int maxLevelsCount = levelDataList.Levels.Count;

            if (maxLevelsCount == 0)
            {
                Debug.LogError("Уровень не загрузился, проверьте нейминг переменных внутри JSON и pure класов.");
                return;
            }
            
            GameData.MaxLevelsCount = maxLevelsCount;
            int currentLevelNumber = GameData.CurrentLevelNumber;

            if (currentLevelNumber > maxLevelsCount)
            {
                Debug.LogError($"Уровень {currentLevelNumber} не найден в JSON файле: {_jsonFileName}");
                return;
            }

            LevelData levelData = levelDataList.Levels[currentLevelNumber - 1];
            _currentLevelData = levelData;
            LevelSettingsReady?.Invoke(levelData);

            _timeToSpawn = levelData.TimeToSpawn;
            _secondsToWin = levelData.SecondsToWin;
            _enemyTypesInfo = levelData.EnemyTypesInfo;

            CalculateTimeToSurvive();

            _timeManager.secondsToWin = _mainTimeToSurvive % _secondsInMinute;

            _enemySpawner.SetStartedParameters(_enemyTypesInfo, _timeToSpawn);
        }
    }
}