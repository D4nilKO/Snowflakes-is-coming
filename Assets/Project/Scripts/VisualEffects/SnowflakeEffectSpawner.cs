using UnityEngine;
using static NTC.Global.Pool.NightPool;

namespace Project.VisualEffects
{
    public class SnowflakeEffectSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _despawnEffect;

        [SerializeField]
        private GameObject _bounceEffect;

        public void DespawnEffect(Vector3 position)
        {
            Spawn(_despawnEffect, position);
        }

        public void SpawnBounceEffect(Collision2D collision)
        {
            ContactPoint2D contact = collision.contacts[0];
            Vector2 position = contact.point;
            Vector2 normal = contact.normal;

            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;

            Spawn(_bounceEffect, position, Quaternion.Euler(0, 0, angle + 90f));
        }
    }
}