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
            if (YandexGame.EnvironmentData.isMobile)
            {
                HandleMobileInput();
            }
            else
            {
                HandlePCInput();
            }
        }

        private void HandlePCInput()
        {
            Vector2 inputPosition = Input.mousePosition;
            HandleInput(inputPosition);
        }

        private void HandleMobileInput()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (_abilityPanel.IsTouchOnPanel(touch.position))
                {
                    continue;
                }

                if (touch.phase is TouchPhase.Moved or TouchPhase.Stationary or TouchPhase.Began)
                {
                    HandleInput(touch.position);
                    break;
                }
            }
        }

        private void SetBounds()
        {
            if (_screenBounds == null || _screenBounds.Points == null)
            {
                Debug.LogError("ScreenBounds или точки не установлены!");
                return;
            }

            Vector2[] edgePoints = _screenBounds.Points;

            if (edgePoints.Length < 4)
            {
                Debug.LogError($"Недостаточно точек! {edgePoints.Length}");
                return;
            }

            Bounds bounds = ObjectCollider.bounds;
            _halfObjectWidth = bounds.extents.x;
            _halfObjectHeight = bounds.extents.y;

            _minX = edgePoints.Min(p => p.x) + _halfObjectWidth;
            _maxX = edgePoints.Max(p => p.x) - _halfObjectWidth;
            _minY = edgePoints.Min(p => p.y) + _halfObjectHeight;
            _maxY = edgePoints.Max(p => p.y) - _halfObjectHeight;
        }

        private void HandleInput(Vector2 inputPosition)
        {
            if (IsInViewport(inputPosition) == false)
                ClampInputPosition(ref inputPosition);

            Vector2 worldPosition = MainCamera.ScreenToWorldPoint(inputPosition);

            if (ValidatePosition(worldPosition) == false)
                return;

            CheckOutOfBounds(worldPosition);
        }

        private void CheckOutOfBounds(Vector2 newPosition)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, _minY, _maxY);

            transform.position = Vector2.MoveTowards(transform.position, newPosition, maxDistanceDelta);
        }

        private void ClampInputPosition(ref Vector2 inputPosition)
        {
            inputPosition.x = Mathf.Clamp(inputPosition.x, 0, Screen.width);
            inputPosition.y = Mathf.Clamp(inputPosition.y, 0, Screen.height);
        }

        private bool IsInViewport(Vector2 screenPosition)
        {
            return _screenBounds.BoundsRect.Contains(screenPosition);
        }

        private bool ValidatePosition(Vector2 position)
        {
            return !(float.IsNaN(position.x) || float.IsNaN(position.y) ||
                float.IsInfinity(position.x) || float.IsInfinity(position.y));
        }
    }
}