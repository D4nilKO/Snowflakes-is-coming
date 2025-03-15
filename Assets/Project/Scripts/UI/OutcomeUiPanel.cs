using System;
using Project.Menu;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.UI
{
    public class OutcomeUiPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _menuButton;

        [SerializeField]
        private Button _restartButton;

        private SceneLoader _sceneLoader;

        public event Action RestartRequested;

        [Inject]
        public void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        protected virtual void Awake()
        {
            this.ValidateSerializedFields();
            AddListeners();
            gameObject.SetActive(false);
        }

        protected virtual void AddListeners()
        {
            _menuButton.onClick.RemoveAllListeners();
            _menuButton.onClick.AddListener(BackToMenu);

            _restartButton.onClick.RemoveAllListeners();
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnRestartButtonClicked()
        {
            RestartRequested?.Invoke();
        }

        private void BackToMenu()
        {
            _sceneLoader.LoadMainMenu();
        }
    }
}