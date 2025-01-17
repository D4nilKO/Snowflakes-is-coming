using System.Collections.Generic;
using System.Linq;
using Project.Abilities;
using Project.Entities.Collectables;
using UnityEngine;

namespace Project.Data.Collections
{
    [CreateAssetMenu(fileName = "_CollectionData", menuName = "Data/Collection", order = 0)]
    public class CollectionData : ScriptableObject
    {
        [field: SerializeField]
        public string CollectionName { get; private set; }

        [SerializeField]
        private List<CollectableSnowflake> _snowflakes;

        public List<CollectableSnowflake> Snowflakes => _snowflakes.ToList();

        public AbilityData UnlockableAbilityData;
    }
}