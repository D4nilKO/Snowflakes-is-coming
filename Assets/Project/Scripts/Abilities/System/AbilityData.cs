using UnityEngine;

namespace Project.Abilities
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "Data/AbilityData", order = 0)]
    public class AbilityData : ScriptableObject
    {
        public string AbilityName;
        public Sprite Icon;

        public string LabelRu;
        public string LabelEn;
        public string LabelTr;

        [field: SerializeField, Min(1f)]
        public float Cooldown { get; private set; }
    }
}