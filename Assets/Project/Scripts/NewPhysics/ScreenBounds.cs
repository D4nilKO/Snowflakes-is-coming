using System;
using Project.Abilities;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

namespace Project.NewPhysics
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class ScreenBounds : MonoBehaviour
    {
        [SerializeField]
        private float _cornerOffset = 1.0f;

        private Camera _cameraMain;
        private ScreenResizeHandler _screenResizeHandler;
        private AbilityPanel _abilityPanel;
        private EdgeCollider2D _edge;

        public Vector2[] Points => _edge.points;
        public Rect BoundsRect { get; private set; } = new(Vector2.zero, Vector2.zero);

        [Inject]
        public void Construct(ScreenResizeHandler screenResizeHandler, AbilityPanel abilityPanel)
        {
            _screenResizeHandler = screenResizeHandler;
            _abilityPanel = abilityPanel;
        }

        private void Awake()
        {
            _cameraMain = Camera.main;
            _edge = GetComponent<EdgeCollider2D>();

            UpdateBounds();

            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void OnValidate()
        {
            if (Application.isPlaying == false)
                return;

            UpdateBounds();
        }

        private void SubscribeEvents()
        {
            _screenResizeHandler.ScreenResized += UpdateBounds;
            _abilityPanel.OnPanelResized += UpdateBounds;
        }

        private void UnsubscribeEvents()
        {
            _screenResizeHandler.ScreenResized -= UpdateBounds;
            _abilityPanel.OnPanelResized -= UpdateBounds;
        }

        private void UpdateBounds()
        {
            Vector2 screenBottomLeft = _cameraMain.ViewportToWorldPoint(Vector2.zero);
            Vector2 screenTopRight = _cameraMain.ViewportToWorldPoint(Vector2.one);

            float panelWidth = _abilityPanel.GetPanelWidth();

            var screenSize = new Vector2(panelWidth, 0f);
            float leftX = _cameraMain.ScreenToWorldPoint(screenSize).x;
            float rightX = screenTopRight.x;
            float bottomY = screenBottomLeft.y;
            float topY = screenTopRight.y;

            var rectX = _cameraMain.pixelWidth - panelWidth;
            float rectY = _cameraMain.pixelHeight;
            BoundsRect = new Rect(new Vector2(panelWidth, 0f), new Vector2(rectX, rectY));

            Vector2[] edgePoints = new Vector2[9];

            // Нижний левый угол с отступом
            edgePoints[0] = new Vector2(leftX + _cornerOffset, bottomY);

            // Нижний правый угол с отступом
            edgePoints[1] = new Vector2(rightX - _cornerOffset, bottomY);

            // Правый нижний угол с отступом вверх
            edgePoints[2] = new Vector2(rightX, bottomY + _cornerOffset);

            // Правый верхний угол с отступом вниз
            edgePoints[3] = new Vector2(rightX, topY - _cornerOffset);

            // Верхний правый угол с отступом влево
            edgePoints[4] = new Vector2(rightX - _cornerOffset, topY);

            // Верхний левый угол с отступом вправо
            edgePoints[5] = new Vector2(leftX + _cornerOffset, topY);

            // Левый верхний угол с отступом вниз
            edgePoints[6] = new Vector2(leftX, topY - _cornerOffset);

            // Левый нижний угол с отступом вверх
            edgePoints[7] = new Vector2(leftX, bottomY + _cornerOffset);

            // Замыкаем контур
            edgePoints[8] = edgePoints[0];

            _edge.points = edgePoints;
        }
    }
}