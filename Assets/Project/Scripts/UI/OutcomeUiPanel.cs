using Project.Menu;
using Project.Timing;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Architecture.UI
{
    public class OutcomeUiPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _menuButton;

        private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(SceneLoader sceneLoader, PauseHandler pauseHandler)
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
        }

        private void BackToMenu()
        {
            _sceneLoader.LoadMainMenu();
        }
    }
}