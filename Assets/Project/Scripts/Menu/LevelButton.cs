using Project.Data;
using Project.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Menu
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        private const float inactiveColorAlpha = 0.5f;

        [SerializeField]
        private TextMeshProUGUI _levelText;

        private Button _button;

        private SceneLoader _sceneLoader;
        private ProgressData _progressData;

        private Color _startedColor;
        private Color _inactiveColor;

        private int _levelNumber;

        public void Initialize(SceneLoader sceneLoader, ProgressData progressData)
        {
            this.ValidateSerializedFields();

            _button = GetComponent<Button>();

            _sceneLoader = sceneLoader;
            _progressData = progressData;

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnLevelButtonClicked);

            GetColors();
        }

        public void Setup(int levelNumber, bool isUnlocked)
        {
            SetLevelNumber(levelNumber);
            _button.interactable = isUnlocked;

            _levelText.color = isUnlocked
                ? _startedColor
                : _inactiveColor;
        }

        private void GetColors()
        {
            _startedColor = _levelText.color;
            _inactiveColor = _startedColor;
            _inactiveColor.a = inactiveColorAlpha;
        }

        private void SetLevelNumber(int levelNumber)
        {
            if (levelNumber <= 0)
            {
                Debug.LogError("Уровень должен быть больше нуля");
                return;
            }

            if (levelNumber > _progressData.MaxLevelsCount)
            {
                Debug.LogError($"Уровень не существует: {levelNumber} > {_progressData.MaxLevelsCount}");
                return;
            }

            _levelNumber = levelNumber;
            _levelText.text = levelNumber.ToString();
        }

        private void OnLevelButtonClicked()
        {
            _progressData.SetCurrentLevel(_levelNumber);
            MetricaSender.SendWithId(MetricaId.LevelStartFromMenuId, _levelNumber.ToString());
            _sceneLoader.LoadBaseGame();
        }
    }
}