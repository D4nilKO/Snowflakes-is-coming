using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public abstract class EnemyBase : MonoBehaviour
    {
        [Range(0f, 20f)] public float speed = 5f; // Скорость движения объекта

        private const float StandardSize = 0.2f;
        private float _size;
        public float Size
        {
            get => _size;
            set
            {
                gameObject.transform.localScale = new Vector3(value, value, 1f);
                _size = value;
            }
        }
        
        private protected Vector2 Direction; // Направление движения
        private protected Rigidbody2D Rb2D; // Компонент Rigidbody2D
        private protected Collider2D ObjectCollider; // Компонент Collider2D

        protected static float ScreenWidth;
        protected static float ScreenHeight;
        
        private protected float ObjectHeight;
        private protected float ObjectWidth;

        protected virtual void Awake()
        {
            Rb2D = GetComponent<Rigidbody2D>(); // Получение компонента Rigidbody2D
            ObjectCollider = GetComponent<Collider2D>(); // Получение компонента Collider2D
            Size = StandardSize;
        }

        protected virtual void Start()
        {
            TakeObjectSize();
            UpdateRbVelocity();
        }

        private void OnEnable()
        {
            TakeObjectSize();
            UpdateRbVelocity();
        }

        public abstract void Move();

        protected virtual void UpdateRbVelocity()
        {
            Rb2D.velocity = Direction * speed; // Установка начальной скорости
        }

        protected void TakeObjectSize()
        {
            var bounds = ObjectCollider.bounds;
            ObjectWidth = bounds.size.x;
            ObjectHeight = bounds.size.y;
        }

        public static void TakeCameraSize(Camera _camera)
        {
            ScreenWidth = _camera.orthographicSize * _camera.aspect * 2f; // Получение ширины экрана
            ScreenHeight = _camera.orthographicSize * 2f; // Получение высоты экрана
        }

        protected Vector2 GetRandomDirection()
        {
            return Random.insideUnitCircle.normalized;
        }
    }
}