using UnityEngine;

namespace Project.Components.Scripts.Level_System
{
    public class JsonLevelParser : MonoBehaviour
    {
        [SerializeField] private string _jsonFileName;

        private LevelDataList _levelDataList;

        public LevelDataList GetLevelDataList()
        {
            return _levelDataList.Clone();
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

            int maxLevelsCount = _levelDataList.LevelsCount;

            if (maxLevelsCount <= 0)
            {
                Debug.LogError("Уровни не загрузились, проверьте названия переменных внутри JSON и pure класов.");
            }
        }
    }
}