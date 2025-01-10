using System;
using Project.LevelSystem;
using UnityEngine;
using VContainer;
using YG;

namespace Project.Services
{
    public class FinalPanel : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private GameObject _reviewPanel;

        [SerializeField]
        private GameObject _outOfLevelsPanel;

        private LevelInitializer _levelInitializer;

        [Inject]
        public void Construct(LevelInitializer levelInitializer)
        {
            _levelInitializer = levelInitializer;
            SubscribeEvents();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        private void Awake()
        {
            this.ValidateSerializedFields();

            _reviewPanel.SetActive(false);
            _outOfLevelsPanel.SetActive(false);
        }

        private void SubscribeEvents()
        {
            _levelInitializer.LastLevelReached += OnLastLevelReached;
        }

        private void UnsubscribeEvents()
        {
            _levelInitializer.LastLevelReached -= OnLastLevelReached;
        }

        private void OnLastLevelReached()
        {
            gameObject.SetActive(true);
            ShowRightOnePanel();
        }

        private void ShowRightOnePanel()
        {
            bool reviewCanShow = YandexGame.EnvironmentData.reviewCanShow;

            _reviewPanel.SetActive(reviewCanShow);
            _outOfLevelsPanel.SetActive(!reviewCanShow);
        }
    }
}