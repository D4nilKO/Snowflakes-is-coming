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
            Debug.Log("Training Constructed");
        }

        public void Start()
        {
            Debug.Log("Training");
            bool isShow = YandexGame.savesData.IsSawTraining;
            _trainingPanel.SetActive(isShow);
        }
    }
}