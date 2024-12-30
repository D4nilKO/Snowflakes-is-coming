using System;
using Project.Architecture.UI;
using Project.GameState;
using Project.Timing;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using YG;

namespace Project.Services
{
    public class RewardAdHandler : IDisposable
    {
        private GameOutcome _gameOutcome;

        private GameObject _losePanel;

        private Button _watchAdButton;

        private bool _isRevived;

        [Inject]
        public void Construct(GameOutcome gameOutcome, LosePanel losePanel)
        {
            _gameOutcome = gameOutcome;
            _watchAdButton = losePanel.ShowAdButton;
            _losePanel = losePanel.gameObject;

            SubscribeEvents();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        public void Initialize()
        {
            _isRevived = false;
            SetActiveRewardButton(false);
        }

        private void SubscribeEvents()
        {
            YandexGame.RewardVideoEvent += Rewarded;
            YandexGame.ErrorVideoEvent += NotRewarded;
        }

        private void UnsubscribeEvents()
        {
            YandexGame.RewardVideoEvent -= Rewarded;
            YandexGame.ErrorVideoEvent -= NotRewarded;
        }

        private void Rewarded(int id)
        {
            _losePanel.SetActive(false);
            _gameOutcome.RevivePlayer();

            SetActiveRewardButton(true);
        }

        private void NotRewarded()
        {
            PauseHandler.Pause();
        }

        private void SetActiveRewardButton(bool isWatchedAd)
        {
            if (_isRevived == false)
            {
                _watchAdButton.gameObject.SetActive(!isWatchedAd);
            }
        }
    }
}