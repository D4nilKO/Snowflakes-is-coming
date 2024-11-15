using UnityEngine;

namespace Project.Entities.Character
{
    [DisallowMultipleComponent]
    public class Player : Entity, IMovable
    {
        private Bounds _bounds;
        private float _halfObjectWidth;
        private float _halfObjectHeight;

        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        public void Move()
        {
            Vector2 mousePosition = Input.mousePosition;

            if (IsInViewport(mousePosition, MainCamera))
            {
                Vector2 worldPosition = MainCamera.ScreenToWorldPoint(mousePosition);

                if (ValidatePosition(worldPosition) == false)
                    return;

                CheckOutOfBounds(worldPosition);
            }
        }

        public void Initialize()
        {
            Debug.Log("character init");

            SetBounds();
        }

        private void SetBounds()
        {
            _bounds = ObjectCollider.bounds;
            _halfObjectWidth = _bounds.extents.x;
            _halfObjectHeight = _bounds.extents.y;

            _minX = (-ScreenWidth * 0.5f) + _halfObjectWidth;
            _maxX = (ScreenWidth * 0.5f) - _halfObjectWidth;
            _minY = (-ScreenHeight * 0.5f) + _halfObjectHeight;
            _maxY = (ScreenHeight * 0.5f) - _halfObjectHeight;
        }

        public bool IsInViewport(Vector2 screenPosition, Camera _camera)
        {
            Rect viewportRect = new(0, 0, _camera.pixelWidth, _camera.pixelHeight);
            return viewportRect.Contains(screenPosition);
        }

        private bool ValidatePosition(Vector2 position)
        {
            bool isNaN = float.IsNaN(position.x) || float.IsNaN(position.y);
            bool isInfinity = float.IsInfinity(position.x) || float.IsInfinity(position.y);

            return isNaN == false && isInfinity == false;
        }

        private void CheckOutOfBounds(Vector2 newPosition)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, _minY, _maxY);

            transform.position = Vector2.MoveTowards(transform.position, newPosition, 2.5f);
        }
    }
}