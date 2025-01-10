using Project.WindowSystem;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Menu
{
    public class MainMenuPanel : BaseWindow
    {
        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _levelsButton;

        [Inject]
        public void Construct(SceneLoader sceneLoader)
        {
            this.ValidateSerializedFields();
            IsStartWindow = true;

            _playButton.onClick.AddListener(sceneLoader.LoadBaseGame);
            _levelsButton.onClick.AddListener(OnLevelsButtonClicked);
        }

        private void OnLevelsButtonClicked()
        {
            WindowManager.ShowWindow("Levels");
        }
    }
}