using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Components.Scripts.Entities.Enemies
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Entity))]
    public class SizeChanger : MonoBehaviour
    {
        [FormerlySerializedAs("changeRate")] [SerializeField] private float _changeRate;

        [FormerlySerializedAs("minSize")] [SerializeField] private float _minSize;
        [FormerlySerializedAs("maxSize")] [SerializeField] private float _maxSize;

        private Entity _entity;

        private void Awake()
        {
            _entity = GetComponent<Entity>();
        }

        private void Update()
        {
            ChangeSize();
        }

        private void ChangeSize()
        {
            float value = Mathf.PingPong(Time.time * _changeRate, 1f);
            float newSize = Mathf.Lerp(_minSize, _maxSize, value);
            
            _entity.Size = newSize;
        }
    }
}