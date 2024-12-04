using UnityEngine;

namespace Project.Entities.Enemies
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Entity))]
    public class SizeChanger : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 5f)]
        private float _changeRate = 1f;

        [SerializeField]
        private float _minSize;

        [SerializeField]
        private float _maxSize;

        private Entity _entity;

        private bool _isWorking;

        private void Awake()
        {
            _entity = GetComponent<Entity>();

            CheckFields();
        }

        private void CheckFields()
        {
            if (_minSize >= _maxSize)
            {
                Debug.LogError($"Задан некорректный диапазон изменения размера: {_minSize} - {_maxSize}");
                return;
            }

            _isWorking = true;
        }

        private void Update()
        {
            if (_isWorking == false)
                return;

            ChangeSize();
        }

        private void OnValidate()
        {
            CheckFields();
        }

        private void ChangeSize()
        {
            float value = Mathf.PingPong(Time.time * _changeRate, 1f);
            float newSize = Mathf.Lerp(_minSize, _maxSize, value);

            _entity.Size = newSize;
        }
    }
}