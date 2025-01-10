using System;
using Project.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Project.WindowSystem
{
    public abstract class BaseWindow : MonoBehaviour, IWindow
    {
        [SerializeField, Header("BaseWindow fields")]
        private string _windowId;

        [SerializeField, Footer("---------------------")]
        protected bool IsStartWindow;

        protected WindowManager WindowManager { get; private set; }

        public event Action OnShow;
        public event Action OnHide;

        [Inject]
        public void Construct(WindowManager windowManager)
        {
            WindowManager = windowManager;

            if (WindowManager == null)
            {
                Debug.LogError("WindowManager is not provided.");
                return;
            }

            if (_windowId != null && WindowManager.GetCurrentWindow()?.GetId() == _windowId)
            {
                Debug.LogWarning($"Window {name} is already registered.");
                return;
            }

            WindowManager.RegisterWindow(GetId(), this, IsStartWindow);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow?.Invoke();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke();
        }

        public virtual string GetId()
        {
            if (string.IsNullOrEmpty(_windowId) && string.IsNullOrWhiteSpace(_windowId))
            {
                Debug.LogWarning($"Window {name} has no ID. Using default name.");
                return GetType().Name;
            }

            return _windowId;
        }

        private void OnDestroy()
        {
            WindowManager?.UnregisterWindow(GetId());
        }
    }
}