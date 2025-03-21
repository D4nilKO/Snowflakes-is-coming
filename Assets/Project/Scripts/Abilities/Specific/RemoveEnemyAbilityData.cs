using UnityEngine;

namespace Project.Abilities
{
    [CreateAssetMenu(fileName = "RemoveEnemy_AbilityData", menuName = "Data/Abilities/RemoveEnemy")]
    public class RemoveEnemyAbilityData : AbilityData
    {
        [field: SerializeField, Range(1, 10)]
        public int Count = 2;
    }
}