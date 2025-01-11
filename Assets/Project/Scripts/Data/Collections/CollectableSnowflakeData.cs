using UnityEngine;

namespace Project.Data.Collections
{
    public class CollectableSnowflakeData : ScriptableObject
    {
        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        [field: SerializeField]
        public int Value { get; private set; }
        
        [field: SerializeField]
        public Collection ParentCollection { get; private set; }
    }
}