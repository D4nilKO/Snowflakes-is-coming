namespace Project.Components.Scripts.Test
{
    using UnityEngine;

    [RequireComponent(typeof(EdgeCollider2D))]
    public class ScreenBounds : MonoBehaviour
    {
        void Start()
        {
            UpdateBounds();
        }

        void UpdateBounds()
        {
            Camera cam = Camera.main;
            EdgeCollider2D edge = GetComponent<EdgeCollider2D>();

            // Получаем размер экрана в мировых координатах
            Vector2 screenBottomLeft = cam.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 screenTopRight = cam.ViewportToWorldPoint(new Vector2(1, 1));

            Vector2[] edgePoints = new Vector2[5];

            // Левая нижняя точка
            edgePoints[0] = screenBottomLeft;
            // Правая нижняя точка
            edgePoints[1] = new Vector2(screenTopRight.x, screenBottomLeft.y);
            // Правая верхняя точка
            edgePoints[2] = screenTopRight;
            // Левая верхняя точка
            edgePoints[3] = new Vector2(screenBottomLeft.x, screenTopRight.y);
            // Замыкаем в левую нижнюю точку
            edgePoints[4] = screenBottomLeft;

            edge.points = edgePoints;
        }
    }
}