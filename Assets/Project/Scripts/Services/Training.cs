using System;
using Project.Entities.Enemies;
using Project.GameState;
using Project.NewPhysics;
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
        private ScreenResizeHandler _screenResizeHandler;

        [Inject]
        private void Construct(TrainingPanel trainingPanel, PauseHandler pauseHandler, EnemyContainer enemyContainer,
            ScreenResizeHandler screenResizeHandler)
        {
            _trainingPanel = trainingPanel;
            _pauseHandler = pauseHandler;
            _enemyContainer = enemyContainer;
            _screenResizeHandler = screenResizeHandler;
        }

        public void Start()
        {
            Subscribe();

            bool isSawTraining = YandexGame.savesData.IsSawTraining;
            _trainingPanel.gameObject.SetActive(!isSawTraining);
            SetSpawnerArrowPosition();

            YandexGame.savesData.IsSawTraining = true;
            YandexGame.SaveProgress();
        }

        private void SetSpawnerArrowPosition()
        {
            _trainingPanel.SpawnerArrow.transform.position = _enemyContainer.GetNewPosition();
        }

        private void Subscribe()
        {
            _pauseHandler.InGamePauseHide += Hide;
            _screenResizeHandler.ScreenResized += SetSpawnerArrowPosition;
        }

        private void Unsubscribe()
        {
            _pauseHandler.InGamePauseHide -= Hide;
            _screenResizeHandler.ScreenResized -= SetSpawnerArrowPosition;
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