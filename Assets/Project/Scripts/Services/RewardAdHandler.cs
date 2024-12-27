using Project.Entities.Character;
using Project.GameState;
using Project.Timing;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Services
{
    public class RewardAdHandler : MonoBehaviour
    {
        [SerializeField]
        private GameOutcome _gameOutcome;

        [SerializeField]
        private GameObject _gameOverCanvas;

        [SerializeField]
        private PauseHandler _pauseHandler;

        [SerializeField]
        private CharacterCollisionHandler _characterCollisionHandler;

        [SerializeField]
        private Button _watchAdButton;

        private bool _isRevived;

        public void Initialize()
        {
            _isRevived = false;
            SetActiveRewardButtons(false);
        }

        public void ShowRewardAd()
        {
            YandexGame.RewVideoShow(0);
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
            _pauseHandler.ForceInGamePause();
            _gameOverCanvas.SetActive(false);

            SetActiveRewardButtons(true);
        }

        private void NotRewarded()
        {
            PauseHandler.Pause();
        }

        private void SetActiveRewardButtons(bool isWatchedAd)
        {
            if (_isRevived == false)
            {
                _watchAdButton.gameObject.SetActive(!isWatchedAd);
            }
        }
    }
}