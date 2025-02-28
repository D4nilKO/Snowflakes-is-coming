using System.Collections.Generic;
using System.Linq;
using Project.Abilities;
using Project.Entities.Collectables;

namespace Project.Data.Collections
{
    public class CollectionModel
    {
        private readonly CollectionData _data;

        private HashSet<CollectableSnowflake> _collectedSnowflakes = new();

        public string AbilityName => _data.CollectionName;
        public int CollectedAmount => _collectedSnowflakes.Count;
        public int AllCollectedCount => _data.Snowflakes.Count;
        public Ability Ability { get; }

        public CollectionModel(CollectionData data, Ability ability,
            ICollection<CollectableSnowflake> savedCollectedSnowflakes)
        {
            _data = data;
            Ability = ability;

            if (savedCollectedSnowflakes is null || savedCollectedSnowflakes.Any() == false)
                return;

            _collectedSnowflakes = savedCollectedSnowflakes.ToHashSet();
        }

        public bool AddSnowflake(CollectableSnowflake snowflake)
        {
            bool isAdded = _collectedSnowflakes.Add(snowflake);
            bool isCompleted = CollectedAmount == AllCollectedCount;

            if (isAdded)
            {
                Ability?.ReActivate();
            }

            return isCompleted;
        }
    }
}