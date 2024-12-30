using System.Linq;
using Project.Settings;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Services
{
    public class DevPanelSwitcher : ITickable, IStartable
    {
        private const int inputBufferLimit = 10;

        private string _inputBuffer = "";

        private DevPanelSettings _settings;
        private GameObject _panel;

        [Inject]
        public void Construct(DevPanel panel, DevPanelSettings settings)
        {
            _panel = panel.gameObject;
            _settings = settings;
        }

        public void Start()
        {
#if UNITY_EDITOR
            ToggleDevPanel(true);
#else
            ToggleDevPanel(false);
#endif
        }

        public void Tick()
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

            if (_inputBuffer.Length > inputBufferLimit)
            {
                _inputBuffer = _inputBuffer[^inputBufferLimit..];
            }
        }

        private void ProcessInputBuffer()
        {
            if (_inputBuffer.EndsWith(_settings.ShowCommand))
            {
                ToggleDevPanel(true);
                ClearInputBuffer();
            }
            else if (_inputBuffer.EndsWith(_settings.HideCommand))
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
            if (_panel.activeSelf == isActive)
                return;

            _panel.SetActive(isActive);

            string state = isActive ? "Activated" : "Deactivated";
            Debug.Log($"Dev Panel {state}");
            MetricaSender.SendWithId(MetricaId.TriggerId, $"Dev Panel {state}");
        }
    }
}