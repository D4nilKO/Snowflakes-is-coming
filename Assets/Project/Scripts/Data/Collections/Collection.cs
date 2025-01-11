using System.Collections.Generic;
using System.Linq;
using Project.Entities.Character;
using UnityEngine;

namespace Project.Data.Collections
{
    [CreateAssetMenu(fileName = "_Collection", menuName = "Data/Collection", order = 0)]
    public class Collection : ScriptableObject
    {
        [field: SerializeField]
        public string CollectionName { get; private set; }

        [SerializeField]
        private List<CollectableSnowflakeData> _snowflakes;
        
        public List<CollectableSnowflakeData> Snowflakes => _snowflakes.ToList();
        
        public Ability UnlockableAbility { get; private set; }
    }
}