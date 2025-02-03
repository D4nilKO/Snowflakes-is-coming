using UnityEngine;

namespace Project.Abilities
{
    [CreateAssetMenu(fileName = "SlowMode_AbilityData", menuName = "Data/Abilities/SlowMode", order = 0)]
    public class SlowModeAbilityData : AbilityData
    {
        [field: SerializeField, Min(0f)]
        public float Duration { get; private set; }

        [field: SerializeField, Range(0, 1f)]
        public float SlowFactor { get; private set; }
    }
}