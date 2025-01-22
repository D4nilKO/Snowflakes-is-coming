using System;
using Project.GameState;
using Project.Timing;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using YG;

namespace Project.Services
{
    public class Training : IStartable, IDisposable
    {
        private GameObject _trainingPanel;
        private PauseHandler _pauseHandler;

        [Inject]
        private void Construct(TrainingPanel trainingPanel, PauseHandler pauseHandler)
        {
            _trainingPanel = trainingPanel.gameObject;
            _pauseHandler = pauseHandler;
        }

        public void Start()
        {
            Subscribe();

            bool isSawTraining = YandexGame.savesData.IsSawTraining;
            _trainingPanel.SetActive(!isSawTraining);

            YandexGame.savesData.IsSawTraining = true;
            YandexGame.SaveProgress();
        }

        private void Subscribe()
        {
            _pauseHandler.InGamePauseHide += Hide;
        }

        private void Unsubscribe()
        {
            _pauseHandler.InGamePauseHide -= Hide;
        }

        private void Hide()
        {
            if (_trainingPanel.activeInHierarchy)
            {
                _trainingPanel.SetActive(false);
                Unsubscribe();
            }
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}