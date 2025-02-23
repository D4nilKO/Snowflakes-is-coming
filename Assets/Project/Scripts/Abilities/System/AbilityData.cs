using UnityEngine;

namespace Project.Abilities
{
    public class AbilityData : ScriptableObject
    {
        public string AbilityName;
        public Sprite Icon;

        [field: SerializeField, Min(1f)]
        public float Cooldown { get; private set; }
    }
}