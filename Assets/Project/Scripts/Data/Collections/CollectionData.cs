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

        [field: SerializeField]
        public List<CollectableSnowflake> Snowflakes { get; private set; }

        [field: SerializeField]
        public AbilityData UnlockableAbilityData { get; private set; }

        public bool TryGetSnowflakesByNames(ICollection<string> names, out List<CollectableSnowflake> snowflakes)
        {
            snowflakes = null;

            if (names == null || names.Any() == false)
                return false;

            snowflakes = Snowflakes.Where(snowflake => names.Contains(snowflake.name)).ToList();
            return snowflakes.Any();
        }

        private void OnValidate()
        {
            foreach (CollectableSnowflake snowflake in Snowflakes)
            {
                snowflake.ParentCollectionData = this;
            }
        }
    }
}