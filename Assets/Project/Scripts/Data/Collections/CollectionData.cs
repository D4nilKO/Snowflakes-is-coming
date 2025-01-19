using System.Collections.Generic;
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

        [field: SerializeField]
        public List<CollectableSnowflake> Snowflakes { get; private set; }

        [field: SerializeField]
        public AbilityData UnlockableAbilityData { get; private set; }

        private void OnValidate()
        {
            foreach (var snowflake in Snowflakes)
            {
                snowflake.ParentCollectionData = this;
            }
        }
    }
}