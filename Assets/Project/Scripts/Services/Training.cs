using System;
using Project.Entities.Enemies;
using Project.GameState;
using Project.Timing;
using VContainer;
using VContainer.Unity;
using YG;

namespace Project.Services
{
    public class Training : IStartable, IDisposable
    {
        private TrainingPanel _trainingPanel;
        private PauseHandler _pauseHandler;
        private EnemyContainer _enemyContainer;

        [Inject]
        private void Construct(TrainingPanel trainingPanel, PauseHandler pauseHandler, EnemyContainer enemyContainer)
        {
            _trainingPanel = trainingPanel;
            _pauseHandler = pauseHandler;
            _enemyContainer = enemyContainer;
        }

        public void Start()
        {
            Subscribe();

            bool isSawTraining = YandexGame.savesData.IsSawTraining;
            _trainingPanel.gameObject.SetActive(!isSawTraining);
            _trainingPanel.SpawnerArrow.transform.position = _enemyContainer.GetNewPosition();

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
            if (_trainingPanel.gameObject.activeInHierarchy)
            {
                _trainingPanel.gameObject.SetActive(false);
                Unsubscribe();
            }
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}