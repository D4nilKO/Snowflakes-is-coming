using Sirenix.OdinInspector;
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

            SetActivePanels();
            _levelSelector.StartInitializeCoroutine();
        }

        [Button("Set Active Panels")]
        public void SetActivePanels()
        {
            _levelSelectorPanel.SetActive(false);
            _menuButtons.SetActive(true);
        }
    }
}