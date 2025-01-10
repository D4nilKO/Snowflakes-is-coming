using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace Project.WindowSystem
{
    public class WindowManager : IStartable
    {
        private readonly Dictionary<string, IWindow> _windows = new();
        private readonly Stack<IWindow> _windowStack = new();

        private IWindow _currentWindow;
        private IWindow _startWindow;

        public void Start()
        {
            if (_startWindow == null)
            {
                Debug.LogError("Start window not found.");
                return;
            }

            if (_windows.ContainsValue(_startWindow) == false)
            {
                Debug.LogError($"Start window {_startWindow.GetId()} is not registered.");
                return;
            }

            HideAll();
            ShowWindow(_startWindow.GetId());
        }

        private void HideAll()
        {
            foreach (var window in _windows.Values)
            {
                window.Hide();
            }
        }

        public void RegisterWindow(string windowId, IWindow window, bool isStartWindow)
        {
            if (!_windows.TryAdd(windowId, window))
            {
                Debug.LogWarning($"Window with ID {windowId} is already registered.");
                return;
            }

            if (isStartWindow)
            {
                if (_startWindow != null)
                {
                    Debug.LogWarning("Multiple start windows defined. The last one will be used.");
                }
                _startWindow = window;
            }
        }

        public void UnregisterWindow(string windowId)
        {
            if (_windows.TryGetValue(windowId, out var window))
            {
                if (_currentWindow == window)
                {
                    _currentWindow = null;
                }

                _windows.Remove(windowId);
            }
            else
            {
                Debug.LogError($"Window with ID {windowId} not found during unregister.");
            }
        }

        public void ShowWindow(string windowId, bool hidePrevious = true)
        {
            if (_windows.TryGetValue(windowId, out var window))
            {
                Debug.Log($"Showing window: {windowId}");

                if (hidePrevious && _currentWindow != null)
                {
                    Debug.Log($"Hiding current window: {_currentWindow.GetId()}");
                    _currentWindow.Hide();
                }

                if (_currentWindow != null)
                {
                    _windowStack.Push(_currentWindow);
                }

                window.Show();
                _currentWindow = window;
            }
            else
            {
                Debug.LogError($"Window with ID {windowId} not found.");
            }
        }

        public void ShowPreviousWindow(bool hideCurrent = true)
        {
            if (_windowStack.Count > 0)
            {
                if (hideCurrent && _currentWindow != null)
                {
                    _currentWindow.Hide();
                }

                _currentWindow = _windowStack.Pop();
                _currentWindow.Show();
            }
            else
            {
                Debug.LogError("No previous window to show.");
            }
        }

        public void HideWindow(string windowId)
        {
            if (_windows.TryGetValue(windowId, out var window))
            {
                window.Hide();
                Debug.Log($"{windowId} hide");
            }
            else
            {
                Debug.LogError($"Window with ID {windowId} not found.");
            }
        }

        public void HideCurrentWindow()
        {
            if (_currentWindow != null)
            {
                _currentWindow.Hide();
                _currentWindow = null;
            }
        }

        public void CloseAllWindows()
        {
            while (_windowStack.Count > 0)
            {
                _windowStack.Pop().Hide();
            }

            if (_currentWindow != null)
            {
                _currentWindow.Hide();
                _currentWindow = null;
            }
        }

        public IWindow GetCurrentWindow()
        {
            return _currentWindow;
        }
    }
}