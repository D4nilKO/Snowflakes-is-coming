using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Architecture.UI
{
    public class WonPanel : OutcomeUiPanel
    {
        [SerializeField]
        private Button _nextLevelButton;

        private void Awake()
        {
            this.ValidateSerializedFields();
            gameObject.SetActive(false);
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            _nextLevelButton.onClick.RemoveAllListeners();
            _nextLevelButton.onClick.AddListener(LoadNextLevel);
        }

        private void LoadNextLevel()
        {
            LevelInitializer.LoadNextLevel();
            gameObject.SetActive(false);
            YandexGame.FullscreenShow();
        }
    }
}