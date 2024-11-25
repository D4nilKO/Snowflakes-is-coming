using Project.Timing;
using UnityEngine;
using YG;

namespace Project.Services
{
    public class RewardAdHandler : MonoBehaviour
    {
        [SerializeField]
        private PauseHandler _pauseHandler;

        [SerializeField]
        private GameObject _rewardAdButton;

        [SerializeField]
        private GameObject _continueButton;

        [SerializeField]
        private GameObject _replayButton;

        public void Initialize()
        {
            _rewardAdButton.SetActive(true);
            _continueButton.SetActive(false);
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
            _pauseHandler.Pause();

            _rewardAdButton.SetActive(false);
            _continueButton.SetActive(true);
            _replayButton.SetActive(true);
        }
        
        private void NotRewarded()
        {
            _pauseHandler.Pause();
        }
    }
}