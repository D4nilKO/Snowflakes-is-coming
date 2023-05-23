using UnityEngine;

namespace Project.Components.Scripts
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public abstract class EnemyBase : MonoBehaviour
    {
        public float speed = 5f; // Скорость движения объекта

        protected Vector2 Direction; // Направление движения
        protected Rigidbody2D Rb2D; // Компонент Rigidbody2D
        protected Collider2D ObjectCollider; // Компонент Collider2D

        protected static float ScreenWidth;
        protected static float ScreenHeight;

        protected virtual void Awake()
        {
            Rb2D = GetComponent<Rigidbody2D>(); // Получение компонента Rigidbody2D
            ObjectCollider = GetComponent<Collider2D>(); // Получение компонента Collider2D
        }
        
        protected virtual void Start()
        {
            CollectCameraParameters(Camera.main);
            UpdateRbVelocity();
        }

        protected virtual void UpdateRbVelocity()
        {
            Rb2D.velocity = Direction * speed; // Установка начальной скорости
        }

        private static void CollectCameraParameters(Camera _camera)
        {
            ScreenWidth = _camera.orthographicSize * _camera.aspect * 2f; // Получение ширины экрана
            ScreenHeight = _camera.orthographicSize * 2f; // Получение высоты экрана
        }
        
        public abstract void Move();
        
        protected Vector2 GetRandomDirection()
        {
            return Random.insideUnitCircle.normalized;
        }
        
    }
}