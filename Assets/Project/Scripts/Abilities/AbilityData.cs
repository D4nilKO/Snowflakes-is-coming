using UnityEngine;

namespace Project.Abilities
{
    [CreateAssetMenu(fileName = "_AbilityData", menuName = "Project/AbilityData", order = 0)]
    public class AbilityData : ScriptableObject
    {
        public string AbilityName;
        public Sprite Icon;
    }
}