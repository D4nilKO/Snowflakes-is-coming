using System.Collections.Generic;
using Project.Abilities;
using Project.Entities.Collectables;

namespace Project.Data.Collections
{
    public class CollectionModel
    {
        private HashSet<CollectableSnowflake> _collectedSnowflakes = new();
        private readonly CollectionData _data;
        private readonly Ability _ability;

        public CollectionModel(CollectionData data, Ability ability)
        {
            _data = data;
            _ability = ability;
        }

        public bool AddSnowflake(CollectableSnowflake snowflake)
        {
            bool isAdded = _collectedSnowflakes.Add(snowflake);
            bool isCompleted = _collectedSnowflakes.Count == _data.Snowflakes.Count;

            if (isAdded)
            {
                _ability?.ReActivate();
            }

            return isCompleted;
        }
    }
}