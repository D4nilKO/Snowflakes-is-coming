using Project.GameState;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using YG;

namespace Project.Services
{
    public class Training : IStartable
    {
        private GameObject _trainingPanel;

        [Inject]
        private void Construct(TrainingPanel trainingPanel)
        {
            _trainingPanel = trainingPanel.gameObject;
        }

        public void Start()
        {
            bool isSawTraining = YandexGame.savesData.IsSawTraining;
            _trainingPanel.SetActive(!isSawTraining);

            YandexGame.savesData.IsSawTraining = true;
            YandexGame.SaveProgress();
        }
    }
}