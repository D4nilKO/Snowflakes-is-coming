using Project.Timing;
using SingularityGroup.HotReload;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Architecture.UI
{
    public class InGamePausePanel : MonoBehaviour
    {
        [SerializeField]
        private Button _continueButton;

        private void Awake()
        {
            this.ValidateSerializedFields();
        }

        public void AddListeners(PauseHandler pauseHandler)
        {
            _continueButton.onClick.RemoveAllListeners();
            _continueButton.onClick.AddListener(pauseHandler.InGameResume);
        }
    }
}