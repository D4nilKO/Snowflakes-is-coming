using System;
using Project.LevelSystem;
using Project.Menu;
using UnityEngine;
using UnityEngine.UI;
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

        [SerializeField]
        private Button _menuButton;

        private LevelInitializer _levelInitializer;
        private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(LevelInitializer levelInitializer, SceneLoader sceneLoader)
        {
            _levelInitializer = levelInitializer;
            _sceneLoader = sceneLoader;

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

            _menuButton.onClick.RemoveAllListeners();
            _menuButton.onClick.AddListener(() => _sceneLoader.LoadMainMenu());
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