using System.Linq;
using Project.Abilities;
using Project.NewPhysics;
using UnityEngine;
using VContainer;
using YG;

namespace Project.Entities.Character
{
    public class Player : Entity, IMovable
    {
        private const float maxDistanceDelta = 2.5f;

        public static Player Instance { get; private set; }

        private ScreenBounds _screenBounds;
        private ScreenResizeHandler _screenResizeHandler;
        private AbilityPanel _abilityPanel;

        private Rect _possibleInputRect;

        private Bounds _bounds;
        private float _halfObjectWidth;
        private float _halfObjectHeight;

        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        [Inject]
        private void Construct(ScreenBounds screenBounds, ScreenResizeHandler screenResizeHandler,
            AbilityPanel abilityPanel)
        {
            _screenBounds = screenBounds;
            _screenResizeHandler = screenResizeHandler;
            _abilityPanel = abilityPanel;
        }

        public void Start()
        {
            SetBounds();
            Instance = this;

            _screenResizeHandler.ScreenResized += SetBounds;
            _abilityPanel.OnPanelResized += SetBounds;
        }

        private void OnDestroy()
        {
            _screenResizeHandler.ScreenResized -= SetBounds;
            _abilityPanel.OnPanelResized -= SetBounds;
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            Vector2 inputPosition = Input.mousePosition;

            if (YandexGame.EnvironmentData.isMobile)
            {
                if (Input.touchCount > 0)
                {
                    Touch primaryTouch = Input.GetTouch(0);

                    if (primaryTouch.phase is TouchPhase.Moved or TouchPhase.Stationary)
                    {
                        inputPosition = primaryTouch.position;
                    }
                }
            }

            HandleInput(inputPosition);
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

        private bool IsInViewport(Vector2 screenPosition)
        {
            return _screenBounds.BoundsRect.Contains(screenPosition);
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

        private void HandleInput(Vector2 inputPosition)
        {
            if (IsInViewport(inputPosition) == false)
                ClampInputPosition(inputPosition);

            Vector2 worldPosition = MainCamera.ScreenToWorldPoint(inputPosition);

            if (ValidatePosition(worldPosition) == false)
                return;

            CheckOutOfBounds(worldPosition);
        }

        private void ClampInputPosition(Vector2 inputPosition)
        {
            _possibleInputRect = new Rect(_minX, _minY, _maxX - _minX, _maxY - _minY);
            inputPosition.x = Mathf.Clamp(inputPosition.x, _possibleInputRect.xMin, _possibleInputRect.xMax);
            inputPosition.y = Mathf.Clamp(inputPosition.y, _possibleInputRect.yMin, _possibleInputRect.yMax);
        }
    }
}