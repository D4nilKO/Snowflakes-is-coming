using System.Collections;
using UnityEngine;

namespace Project.Components.Scripts.Enemies
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Entity))]
    public class SizeChanger : MonoBehaviour
    {
        [SerializeField] private float changeRate;

        [SerializeField] private float minSize;
        [SerializeField] private float maxSize;

        private Entity entity;

        private void Awake()
        {
            entity = GetComponent<Entity>();
        }

        private void Update()
        {
            ChangeSize();
        }

        private void ChangeSize()
        {
            float t = Mathf.PingPong(Time.time * changeRate, 1f);
            float newSize = Mathf.Lerp(minSize, maxSize, t);
            entity.Size = newSize;
        }
    }
}