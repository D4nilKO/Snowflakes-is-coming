using System;
using UnityEngine;
using static NTC.Pool.NightPool;

namespace Project.VisualEffects
{
    public class SnowflakeEffectSpawner : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _despawnEffect;

        [SerializeField]
        private ParticleSystem _bounceEffect;

        private void Awake()
        {
            this.ValidateSerializedFields();
        }

        public void DespawnEffect(Vector3 position)
        {
            ParticleSystem spawnedEffect = Spawn(_despawnEffect, position, Quaternion.identity);
            Despawn(spawnedEffect, _despawnEffect.main.duration);
        }

        public void SpawnBounceEffect(Collision2D collision)
        {
            ContactPoint2D contact = collision.contacts[0];
            Vector2 position = contact.point;
            Vector2 normal = contact.normal;

            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;

            ParticleSystem spawnedEffect = Spawn(_bounceEffect, position, Quaternion.Euler(0, 0, angle + 90f));
            Despawn(spawnedEffect, _despawnEffect.main.duration);
        }
    }
}