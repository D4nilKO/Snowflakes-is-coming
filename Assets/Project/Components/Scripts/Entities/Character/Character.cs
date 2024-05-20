using UnityEngine;

namespace Project.Components.Scripts.Character_s
{
    public class Character : Entity
    {
        private Bounds bounds;
        private float halfObjectWidth;
        private float halfObjectHeight;

        private float minX;
        private float maxX;
        private float minY;
        private float maxY;

        private void Start()
        {
            bounds = ObjectCollider.bounds;
            halfObjectWidth = bounds.extents.x;
            halfObjectHeight = bounds.extents.y;

            minX = -ScreenWidth * 0.5f + halfObjectWidth;
            maxX = ScreenWidth * 0.5f - halfObjectWidth;
            minY = -ScreenHeight * 0.5f + halfObjectHeight;
            maxY = ScreenHeight * 0.5f - halfObjectHeight;
            
            DontDestroyOnLoad(gameObject);
        }

        public void Move()
        {
            Vector2 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            CheckOutOfBounds(mousePosition);
        }

        private void CheckOutOfBounds(Vector2 newPosition)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            
            transform.position = Vector2.MoveTowards(transform.position, newPosition, 2.5f);
        }

        public void ChangeSize(float newSize)
        {
            Size = newSize;
        }
    }
}