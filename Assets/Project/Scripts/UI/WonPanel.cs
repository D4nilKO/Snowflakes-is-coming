using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Architecture.UI
{
    public class WonPanel : OutcomeUiPanel
    {
        [SerializeField]
        private Button _nextLevelButton;

        public Action LoadNextLevelRequested;

        protected override void AddListeners()
        {
            base.AddListeners();

            _nextLevelButton.onClick.RemoveAllListeners();
            _nextLevelButton.onClick.AddListener(OnLoadNextLevelClicked);
        }

        private void OnLoadNextLevelClicked()
        {
            LoadNextLevelRequested?.Invoke();
            gameObject.SetActive(false);
            YandexGame.FullscreenShow();
        }
    }
}