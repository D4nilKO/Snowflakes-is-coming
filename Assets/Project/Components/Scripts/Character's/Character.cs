using UnityEngine;

namespace Project.Components.Scripts.Character_s
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
            
            DontDestroyOnLoad(gameObject);
        }

        public void Move()
        {
            if (Time.timeScale == 0f)
                return;

            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = GetWorldPosition(mousePosition);

            ClampAndSetPosition(worldPosition);
        }

        private Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            return hit.collider != null ? hit.point : transform.position;
        }

        private void ClampAndSetPosition(Vector2 newPosition)
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