using System.Collections.Generic;
using Project.Timing;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Services
{
    public class RewardAdHandler : MonoBehaviour
    {
        [SerializeField]
        private Button _watchAdButton;

        [SerializeField]
        private Button _reviveButton;

        [SerializeField]
        private List<Button> _otherButtons;

        private bool _isRevived;

        public void Initialize()
        {
            _isRevived = false;
            SetActiveRewardButtons(false);
            SetActiveOtherButtons(true);
        }

        private void Awake()
        {
            this.ValidateSerializedFields();
        }

        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += Rewarded;
            YandexGame.ErrorVideoEvent += NotRewarded;
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= Rewarded;
            YandexGame.ErrorVideoEvent -= NotRewarded;
        }

        private void Rewarded(int id)
        {
            PauseHandler.Pause();

            SetActiveRewardButtons(true);
            SetActiveOtherButtons(false);
        }

        private void NotRewarded()
        {
            PauseHandler.Pause();
        }

        private void SetActiveOtherButtons(bool isActive)
        {
            foreach (Button button in _otherButtons)
            {
                button.gameObject.SetActive(isActive);
            }
        }

        private void SetActiveRewardButtons(bool isWatchedAd)
        {
            if (_isRevived == false)
            {
                _watchAdButton.gameObject.SetActive(!isWatchedAd);
            }

            _reviveButton.gameObject.SetActive(isWatchedAd);
        }

        public void OnRevive()
        {
            _isRevived = true;
            SetActiveRewardButtons(false);
            SetActiveOtherButtons(true);
        }
    }
}