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
        [SerializeField]
        private TextMeshProUGUI _levelText;

        [SerializeField]
        private Button _button;

        private SceneLoader _sceneLoader;

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
        }

        public void Setup(int levelNumber, bool isUnlocked)
        {
            SetLevelNumber(levelNumber);
            _button.interactable = isUnlocked;
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