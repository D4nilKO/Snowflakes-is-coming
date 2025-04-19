using System;
using System.Collections.Generic;
using System.Linq;
using NTC.Global.Pool;
using Project.Entities.Collectables;
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

        private IObjectResolver _container;

        public List<EnemyBase> ActiveEnemies { get; private set; } = new();

        [Inject]
        private void Construct(IObjectResolver container, ScreenResizeHandler screenResizeHandler)
        {
            _container = container;
            _screenResizeHandler = screenResizeHandler;
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
            SetPosition();
            _screenResizeHandler.ScreenResized += SetPosition;
        }

        private void Start()
        {
            FindAndAddEnemies();
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
            ActiveEnemies.RemoveAll(enemy => enemy == null);
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
            ActiveEnemies.Add(enemy);
            enemy.SlowFactor = _slowFactor;
        }

        private void FindAndAddEnemies()
        {
            var enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None)
                .Where(enemy => enemy.isActiveAndEnabled);

            foreach (var enemy in enemies)
            {
                _container.Inject(enemy);
                AddEnemy(enemy);
                enemy.OnSpawn();
            }
        }

        public void DespawnActiveEnemies()
        {
            if (ActiveEnemies.Count == 0)
                return;

            RemoveNullEnemies();

            foreach (var enemy in ActiveEnemies)
                NightPool.Despawn(enemy);

            ActiveEnemies.Clear();
        }

        public List<EnemyBase> GetEnemies()
        {
            RemoveNullEnemies();
            return ActiveEnemies.ToList();
        }

        public void DespawnRandomEnemy(int count)
        {
            if (count <= 0)
            {
                Debug.LogError("Count must be greater than 0");
                return;
            }

            var temp = ActiveEnemies.Where(enemy => enemy is not CollectableSnowflake).ToList();

            if (temp.Count == 0)
            {
                Debug.LogWarning("No valid enemies to despawn.");
                return;
            }

            count = Mathf.Min(count, temp.Count);

            for (int i = 0; i < count; i++)
            {
                int index = 0;
                EnemyBase enemy = temp[index];

                NightPool.Despawn(enemy);
                ActiveEnemies.Remove(enemy);
                temp.RemoveAt(index);
            }
        }
    }
}