
using System;
using UnityEngine;

public class ScreenResizeHandler : MonoBehaviour
{
    private int _lastScreenWidth;
    private int _lastScreenHeight;
    
    public event Action OnScreenResize;

    private void Start()
    {
        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;
    }

    private void Update()
    {
        if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
        {
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;

            OnScreenResize();
        }
    }
}