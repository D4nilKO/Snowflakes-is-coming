using UnityEngine;

namespace Project.Components.Scripts.Entities.Character
{
    public class Player : Entity, IMovable
    {
        private Bounds _bounds;
        private float _halfObjectWidth;
        private float _halfObjectHeight;

        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        private void SetBounds()
        {
            _bounds = ObjectCollider.bounds;
            _halfObjectWidth = _bounds.extents.x;
            _halfObjectHeight = _bounds.extents.y;

            _minX = -ScreenWidth * 0.5f + _halfObjectWidth;
            _maxX = ScreenWidth * 0.5f - _halfObjectWidth;
            _minY = -ScreenHeight * 0.5f + _halfObjectHeight;
            _maxY = ScreenHeight * 0.5f - _halfObjectHeight;
        }

        private void CheckOutOfBounds(Vector2 newPosition)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, _minY, _maxY);

            transform.position = Vector2.MoveTowards(transform.position, newPosition, 2.5f);
        }

        public void Move()
        {
            Vector2 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            CheckOutOfBounds(mousePosition);
        }

        public void Initialize()
        {
            Debug.Log("character init");
            
            SetBounds();
        }
    }
}