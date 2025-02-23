using System.Collections.Generic;
using System.Linq;
using NTC.Global.Pool;
using Project.NewPhysics;
using Sirenix.Utilities;
using UnityEngine;
using VContainer;

namespace Project.Entities.Enemies
{
    public class EnemyContainer : MonoBehaviour
    {
        [SerializeField, Range(0f, 0.5f), Header("0 - Left, 0.5 - Center")]
        private float _containerPercentPosition;

        private float _slowFactor;

        private Camera _mainCamera;
        private ScreenResizeHandler _screenResizeHandler;

        private HashSet<EnemyBase> _activeEnemies = new();

        [Inject]
        private void Construct(ScreenResizeHandler screenResizeHandler)
        {
            _screenResizeHandler = screenResizeHandler;
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
            SetPosition();
            _screenResizeHandler.ScreenResized += SetPosition;
        }

        private void OnDestroy()
        {
            _screenResizeHandler.ScreenResized -= SetPosition;
        }

        private void OnValidate()
        {
            if (Application.isPlaying == false)
                return;

            SetPosition();
        }

        private void RemoveNullEnemies()
        {
            _activeEnemies.RemoveWhere(enemy => enemy == null);
        }

        private void SetPosition()
        {
            transform.position = GetNewPosition();
        }

        public Vector3 GetNewPosition()
        {
            if (_mainCamera == null)
                return Vector3.zero;

            Vector3 viewPortPosition = new(_containerPercentPosition, 0.5f, _mainCamera.nearClipPlane);
            Vector3 worldPoint = _mainCamera.ViewportToWorldPoint(viewPortPosition);
            worldPoint = new Vector3(worldPoint.x, transform.position.y, transform.position.z);
            return worldPoint;
        }

        public void SetSlowFactor(float slowFactor)
        {
            _slowFactor = slowFactor;
        }

        public void AddEnemy(EnemyBase enemy)
        {
            _activeEnemies.Add(enemy);
            enemy.SlowFactor = _slowFactor;
        }

        public void ClearActiveEnemies()
        {
            _activeEnemies.AddRange(GetComponentsInChildren<EnemyBase>());
            _activeEnemies.RemoveWhere(enemy => enemy == null);

            foreach (var enemy in _activeEnemies)
            {
                NightPool.Despawn(enemy);
            }
        }

        public List<EnemyBase> GetEnemies()
        {
            RemoveNullEnemies();
            return _activeEnemies.ToList();
        }
    }
}