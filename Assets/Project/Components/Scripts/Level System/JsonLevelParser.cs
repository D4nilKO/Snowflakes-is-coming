using System;
using Project.Components.Scripts.Level_System.LevelStructure;
using UnityEngine;

namespace Project.Components.Scripts.Level_System
{
    public class JsonLevelParser : MonoBehaviour
    {
        [SerializeField] private string _jsonFileName;

        private LevelDataList _levelDataList;

        public LevelDataList GetLevelDataList()
        {
            if (_levelDataList != null)
            {
                return _levelDataList;
            }

            if (!TryParseJsonFile())
            {
                throw new InvalidOperationException("Failed to load levels from JSON file");
            }

            return _levelDataList;
        }

        private bool TryParseJsonFile()
        {
            if (_jsonFileName == string.Empty)
            {
                Debug.LogError("Пустое имя JSON файла");
                return false;
            }

            TextAsset json = Resources.Load<TextAsset>(_jsonFileName);

            if (json == null)
            {
                Debug.LogError($"Указанный JSON файл не найден: {_jsonFileName}");
                return false;
            }
            
            _levelDataList = JsonUtility.FromJson<LevelDataList>(json.text);

            if (_levelDataList == null)
            {
                Debug.LogError($"Не удалось загрузить JSON файл: {_jsonFileName}");
                return false;
            }

            int maxLevelsCount = _levelDataList.LevelsCount;

            if (maxLevelsCount <= 0)
            {
                Debug.LogError("Уровни не загрузились, проверьте названия переменных внутри JSON и pure класов.");
                return false;
            }

            return true;
        }
    }
}