using System.Linq;
using UnityEngine;

namespace Project.Services
{
    public class DevPanelManager : MonoBehaviour
    {
        [SerializeField, Header("Dev Panel Settings")]
        private GameObject _devPanel;

        [SerializeField]
        private int _inputBufferLimit = 10;

        [SerializeField]
        private string _showCommand = "dev39611";

        [SerializeField]
        private string _hideCommand = "hide";

        private string _inputBuffer = "";

        private void Start()
        {
#if UNITY_EDITOR
            ToggleDevPanel(true);
#else
            ToggleDevPanel(false);
#endif
        }

        private void Update()
        {
            HandleKeyboardInput();
        }

        private void HandleKeyboardInput()
        {
            if (Input.anyKeyDown == false)
                return;

            foreach (char c in Input.inputString.Where(char.IsLetter))
            {
                AddToInputBuffer(c);
            }

            ProcessInputBuffer();
        }

        private void AddToInputBuffer(char character)
        {
            _inputBuffer += char.ToLower(character);

            if (_inputBuffer.Length > _inputBufferLimit)
            {
                _inputBuffer = _inputBuffer.Substring(_inputBuffer.Length - _inputBufferLimit);
            }
        }

        private void ProcessInputBuffer()
        {
            if (_inputBuffer.EndsWith(_showCommand))
            {
                ToggleDevPanel(true);
                ClearInputBuffer();
            }
            else if (_inputBuffer.EndsWith(_hideCommand))
            {
                ToggleDevPanel(false);
                ClearInputBuffer();
            }
        }

        private void ClearInputBuffer()
        {
            _inputBuffer = string.Empty;
        }

        private void ToggleDevPanel(bool isActive)
        {
            if (_devPanel.activeSelf == isActive)
                return;

            _devPanel.SetActive(isActive);

            string state = isActive ? "Activated" : "Deactivated";
            Debug.Log($"Dev Panel {state}");
            MetricaSender.SendWithId(MetricaId.TriggerId, $"Dev Panel {state}");
        }
    }
}