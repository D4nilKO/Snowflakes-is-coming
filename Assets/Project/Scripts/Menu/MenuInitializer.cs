using UnityEngine;

namespace Project.Menu
{
    public class MenuInitializer : MonoBehaviour
    {
        [SerializeField]
        private LevelSelector _levelSelector;

        [SerializeField]
        private GameObject _levelSelectorPanel;

        [SerializeField]
        private GameObject _menuButtons;

        private void Start()
        {
            this.ValidateSerializedFields();

            _levelSelectorPanel.SetActive(false);
            _menuButtons.SetActive(true);

            // перенести подальше от начала игры, чтобы она быстрее загружалась
            Invoke(nameof(InitializeLevelSelector), 0.5f);
        }

        private void InitializeLevelSelector()
        {
            _levelSelector.Initialize();
        }
    }
}