using UnityEngine;

namespace Project.Components.Scripts
{
    public class Character : Entity
    {
        #region CheckOutOfBoundsFields

        private Bounds bounds;
        private float halfObjectWidth;
        private float halfObjectHeight;

        private float minX;
        private float maxX;
        private float minY;
        private float maxY;

        #endregion

        private void Start()
        {
            bounds = objectCollider.bounds;
            halfObjectWidth = bounds.extents.x;
            halfObjectHeight = bounds.extents.y;

            minX = -ScreenWidth * 0.5f + halfObjectWidth;
            maxX = ScreenWidth * 0.5f - halfObjectWidth;
            minY = -ScreenHeight * 0.5f + halfObjectHeight;
            maxY = ScreenHeight * 0.5f - halfObjectHeight;
        }

        public void Move()
        {
            if (Time.timeScale == 0f)
                return;

            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            CheckOutOfBounds(mousePosition);
        }

        private void CheckOutOfBounds(Vector2 newPosition)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            transform.position = newPosition;
        }

        public void ChangeSkin(Sprite newSkin)
        {
            spriteRenderer.sprite = newSkin;
        }

        public void ChangeSize(float newSize)
        {
            Size = newSize;
        }
    }
}