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

        [SerializeField]
        private Button _button;

        private SceneLoader _sceneLoader;
        private Color _startedColor;
        private Color _inactiveColor;

        private int _levelNumber;

        private void Awake()
        {
            this.ValidateSerializedFields();
        }

        public void Initialize(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;

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

            if (levelNumber > ProgressData.MaxLevelsCount)
            {
                Debug.LogError($"Уровень не существует: {levelNumber} > {ProgressData.MaxLevelsCount}");
                return;
            }

            _levelNumber = levelNumber;
            _levelText.text = levelNumber.ToString();
        }

        private void OnLevelButtonClicked()
        {
            ProgressData.SetCurrentLevel(_levelNumber);
            MetricaSender.SendWithId(MetricaId.LevelStartFromMenuId, _levelNumber.ToString());
            _sceneLoader.LoadBaseGame();
        }
    }
}