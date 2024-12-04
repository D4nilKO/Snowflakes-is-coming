using UnityEngine;

namespace Project.Utility
{
    public static class UserUtils
    {
        public static void FetchCameraSize(Camera _camera, out float screenWidth, out float screenHeight)
        {
            screenWidth = _camera.orthographicSize * _camera.aspect * 2f;
            screenHeight = _camera.orthographicSize * 2f;
        }
    }
}