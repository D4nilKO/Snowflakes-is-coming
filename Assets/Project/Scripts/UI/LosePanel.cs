using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Architecture.UI
{
    public class LosePanel : OutcomeUiPanel
    {
        [field: SerializeField]
        public Button ShowAdButton { get; private set; }

        [SerializeField]
        private Button _restartButton;

        public event Action RestartRequested;

        protected override void AddListeners()
        {
            base.AddListeners();

            ShowAdButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();

            ShowAdButton.onClick.AddListener(ShowRewardAd);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnRestartButtonClicked()
        {
            RestartRequested?.Invoke();
        }

        private void ShowRewardAd()
        {
            YandexGame.RewVideoShow(0);
        }
    }
}