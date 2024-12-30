using Project.LevelSystem;
using Project.Menu;
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

        protected LevelInitializer LevelInitializer;

        [Inject]
        public void Construct(LevelInitializer levelInitializer, SceneLoader sceneLoader)
        {
            LevelInitializer = levelInitializer;
            _sceneLoader = sceneLoader;
        }

        private void Awake()
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