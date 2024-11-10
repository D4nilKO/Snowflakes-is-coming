using UnityEngine;

namespace Project.Test
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class ScreenBounds : MonoBehaviour
    {
        [SerializeField] private ScreenResizeHandler _screenResizeHandler;

        private Camera _cameraMain;
        private EdgeCollider2D _edge;

        private void Awake()
        {
            _cameraMain = Camera.main;
            _edge = GetComponent<EdgeCollider2D>();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void Start()
        {
            UpdateBounds();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if (_screenResizeHandler != null)
                _screenResizeHandler.ScreenResized += UpdateBounds;
        }

        private void UnsubscribeEvents()
        {
            if (_screenResizeHandler != null)
                _screenResizeHandler.ScreenResized -= UpdateBounds;
        }

        private void UpdateBounds()
        {
            // Получаем размер экрана в мировых координатах
            Vector2 screenBottomLeft = _cameraMain.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 screenTopRight = _cameraMain.ViewportToWorldPoint(new Vector2(1, 1));

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

            _edge.points = edgePoints;
        }
    }
}