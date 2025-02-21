using System;
using UnityEngine;

namespace Project.NewPhysics
{
    public class ScreenResizeHandler : MonoBehaviour
    {
        private int _lastScreenWidth;
        private int _lastScreenHeight;

        public event Action ScreenResized;

        private void Awake()
        {
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
        }

        private void LateUpdate()
        {
            if (CheckResizeScreen())
            {
                ScreenResized?.Invoke();
            }
        }

        private bool CheckResizeScreen()
        {
            if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
            {
                _lastScreenWidth = Screen.width;
                _lastScreenHeight = Screen.height;

                return true;
            }

            return false;
        }
    }
}