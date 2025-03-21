using System.Collections.Generic;
using System.Linq;
using Project.Abilities;
using Project.Entities.Collectables;

namespace Project.Data.Collections
{
    public class CollectionModel
    {
        private readonly CollectionData _data;

        private HashSet<CollectableSnowflake> _snowflakes = new();

        public List<CollectableSnowflake> Snowflakes => _snowflakes.ToList();
        public int CollectedAmount => _snowflakes.Count;
        public int AllCollectedCount => _data.Snowflakes.Count;
        public Ability Ability { get; }

        public CollectionModel(CollectionData data, Ability ability,
            ICollection<CollectableSnowflake> savedCollectedSnowflakes)
        {
            _data = data;
            Ability = ability;

            if (savedCollectedSnowflakes is null || savedCollectedSnowflakes.Any() == false)
                return;

            _snowflakes = savedCollectedSnowflakes.ToHashSet();
        }

        /// <summary>
        /// Добавляет снежинку в коллекцию и возвращает true, если она завершена.
        /// </summary>
        /// <param name="snowflake">Коллекционная снежинка</param>
        /// <returns>True - если коллекция завершена, false - если нет.</returns>
        public bool AddSnowflake(CollectableSnowflake snowflake)
        {
            if (_snowflakes.Any(temporarySnowflake => temporarySnowflake.name.Contains(snowflake.name)) == false)
            {
                _snowflakes.Add(snowflake);
            }

            Ability?.ReActivate();

            bool isCompleted = CollectedAmount >= AllCollectedCount;
            return isCompleted;
        }
    }
}