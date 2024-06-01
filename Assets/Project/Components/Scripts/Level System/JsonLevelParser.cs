using System;
using Project.Components.Scripts.Data;
using UnityEngine;

namespace Project.Components.Scripts.Level_System
{
    [DisallowMultipleComponent]
    public class JsonLevelParser : MonoBehaviour
    {
        // Разделить отвественности
        [SerializeField] private string _jsonFileName;
        [SerializeField] private ProgressData _progressData;

        [SerializeField] [Header("Для ознакомления, загружается в начале игры")]
        private LevelData _currentLevelData;

        private LevelDataList _levelDataList;

        public event Action<LevelData> LevelSettingsReady;

        private void Start()
        {
            ParseJsonFile();
            FetchLevelSettings();
            Debug.Log("level settings start");
        }

        private void ParseJsonFile()
        {
            TextAsset json = Resources.Load<TextAsset>(_jsonFileName);

            if (json == null)
            {
                Debug.LogError($"Указанный JSON файл не найден: {_jsonFileName}");
                return;
            }

            _levelDataList = JsonUtility.FromJson<LevelDataList>(json.text);
        }

        public void FetchLevelSettings()
        {
            int maxLevelsCount = _levelDataList.Levels.Count;

            if (maxLevelsCount <= 0)
            {
                Debug.LogError("Уровень не загрузился, проверьте названия переменных внутри JSON и pure класов.");
                return;
            }

            _progressData.SetMaxLevelsCount(maxLevelsCount);
            int currentLevelNumber = _progressData.CurrentLevelNumber;

            if (currentLevelNumber > maxLevelsCount)
            {
                Debug.LogError($"Уровень {currentLevelNumber} не найден в JSON файле: {_jsonFileName}");
                return;
            }

            Debug.Log($"Данные уровня №{currentLevelNumber} загружены");

            _currentLevelData = _levelDataList.Levels[currentLevelNumber - 1];
            LevelSettingsReady?.Invoke(_currentLevelData);
        }
    }
}