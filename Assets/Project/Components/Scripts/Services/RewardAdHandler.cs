using Project.GameState;
using Project.Timing;
using UnityEngine;
using YG;

namespace Project.Services
{
    public class RewardAdHandler : MonoBehaviour
    {
        [SerializeField]
        private GameOutcome _gameOutcome;
        
        [SerializeField]
        private PauseHandler _pauseHandler;
        
        [SerializeField]
        private GameObject _rewardAdButton;
        
        [SerializeField]
        private GameObject _continueButton;

        public void Initialize()
        {
            _rewardAdButton.SetActive(true);
            _continueButton.SetActive(false);
        }

        private void Awake()
        {
            if (_gameOutcome == null || _pauseHandler == null || _rewardAdButton == null || _continueButton == null)
            {
                Debug.LogError("rewardAdHandler is empty");
            }
        }

        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += Rewarded;
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= Rewarded;
        }

        private void Rewarded(int id)
        {
            _pauseHandler.Pause();
            
            _rewardAdButton.SetActive(false);
            _continueButton.SetActive(true);
            //
        }
    }
}