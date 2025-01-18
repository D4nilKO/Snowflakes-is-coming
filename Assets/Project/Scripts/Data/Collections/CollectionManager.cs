using System;
using System.Collections.Generic;
using Project.Entities.Collectables;
using Project.Abilities;

namespace Project.Data.Collections
{
    public class CollectionManager
    {
        public event Action<CollectionModel> OnCollectionCompleted;

        private readonly Dictionary<CollectionData, CollectionModel> _collectionModels = new();
        private readonly IAbilityFactory _abilityFactory;

        public CollectionManager(IAbilityFactory abilityFactory)
        {
            _abilityFactory = abilityFactory;
        }

        public void RegisterCollection(CollectionData collectionData)
        {
            if (!_collectionModels.ContainsKey(collectionData))
            {
                // Создаем способность через фабрику на основе UnlockableAbilityData
                Ability ability = _abilityFactory.CreateAbility(collectionData.UnlockableAbilityData);
                _collectionModels[collectionData] = new CollectionModel(collectionData, ability);
            }
        }

        public void AddToCollection(CollectableSnowflake snowflake)
        {
            if (snowflake.ParentCollectionData is null ||
                !_collectionModels.TryGetValue(snowflake.ParentCollectionData, out var model))
                return;

            bool isCompleted = model.AddSnowflake(snowflake);
            if (isCompleted)
            {
                OnCollectionCompleted?.Invoke(model);
            }
        }
    }
}