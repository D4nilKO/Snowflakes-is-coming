using System.Linq;
using Project.NewPhysics;
using UnityEngine;
using VContainer;

namespace Project.Entities.Character
{
    public class Player : Entity, IMovable
    {
        private const float maxDistanceDelta = 2.5f;

        public static Player Instance { get; private set; }

        private ScreenBounds _screenBounds;
        private ScreenResizeHandler _screenResizeHandler;

        private Bounds _bounds;
        private float _halfObjectWidth;
        private float _halfObjectHeight;

        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        [Inject]
        private void Construct(ScreenBounds screenBounds, ScreenResizeHandler screenResizeHandler)
        {
            _screenBounds = screenBounds;
            _screenResizeHandler = screenResizeHandler;
        }

        public void Start()
        {
            SetBounds();
            Instance = this;

            _screenResizeHandler.ScreenResized += SetBounds;
        }

        private void OnDestroy()
        {
            _screenResizeHandler.ScreenResized -= SetBounds;
        }

        private void FixedUpdate()
        {
            Move();
        }

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

        private void SetBounds()
        {
            if (_screenBounds == null)
            {
                Debug.LogError("ScreenBounds не установлен!");
                return;
            }

            Vector2[] edgePoints = _screenBounds.Points;

            if (edgePoints == null)
            {
                Debug.LogError("EdgePoints не установлены!");
                return;
            }

            if (edgePoints.Length < 4)
            {
                Debug.LogError($"Недостаточно точек! {edgePoints.Length}");
                return;
            }

            _bounds = ObjectCollider.bounds;
            _halfObjectWidth = _bounds.extents.x;
            _halfObjectHeight = _bounds.extents.y;

            _minX = edgePoints.Min(p => p.x) + _halfObjectWidth;
            _maxX = edgePoints.Max(p => p.x) - _halfObjectWidth;
            _minY = edgePoints.Min(p => p.y) + _halfObjectHeight;
            _maxY = edgePoints.Max(p => p.y) - _halfObjectHeight;
        }

        private bool IsInViewport(Vector2 screenPosition, Camera _camera)
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

            transform.position = Vector2.MoveTowards(transform.position, newPosition, maxDistanceDelta);
        }
    }
}