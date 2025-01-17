using System.Collections.Generic;
using Project.Abilities;
using Project.Entities.Collectables;

namespace Project.Data.Collections
{
    public class CollectionModel
    {
        private HashSet<CollectableSnowflake> _collectedSnowflakes = new();

        public bool AddSnowflake(CollectableSnowflake snowflakeData)
        {
            bool isAdded = _collectedSnowflakes.Add(snowflakeData);
            bool isCompleted = _collectedSnowflakes.Count == snowflakeData.ParentCollectionData.Snowflakes.Count;

            return isAdded && isCompleted;
        }

        public void ActivateAbility() { }
    }
}