using System.Collections.Generic;

namespace Project.Data.Collections
{
    public class CollectionModel
    {
        public Collection CollectionData { get; private set; }
        private HashSet<CollectableSnowflakeData> _collectedSnowflakes = new();

        public CollectionModel(Collection collectionData)
        {
            CollectionData = collectionData;
        }

        public bool AddSnowflake(CollectableSnowflakeData snowflakeData)
        {
            bool isAdded = _collectedSnowflakes.Add(snowflakeData);
            bool isCompleted = _collectedSnowflakes.Count == CollectionData.Snowflakes.Count;

            return isAdded && isCompleted;
        }

        public void ActivateAbility()
        {
            CollectionData.UnlockableAbility?.Activate();
        }
    }
}