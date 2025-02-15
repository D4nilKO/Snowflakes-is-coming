using System;
using System.Collections.Generic;
using Project.Entities.Collectables;
using Project.Abilities;
using UnityEngine;
using YG;

namespace Project.Data.Collections
{
    public class CollectionManager
    {
        public event Action<CollectionModel> OnCollectionCompleted;

        private Dictionary<CollectionData, CollectionModel> _collectionModels = new();
        private IAbilityFactory _abilityFactory;

        public CollectionManager(IAbilityFactory abilityFactory)
        {
            _abilityFactory = abilityFactory;
        }

        public void RegisterCollection(CollectionData collectionData)
        {
            if (_collectionModels.ContainsKey(collectionData) == false)
            {
                // Создаем способность через фабрику на основе UnlockableAbilityData
                Ability ability = _abilityFactory.CreateAbility(collectionData.UnlockableAbilityData);
                _collectionModels[collectionData] = new CollectionModel(collectionData, ability);
            }
        }

        public void AddToCollection(CollectableSnowflake snowflake)
        {
            if (snowflake.ParentCollectionData is null)
                return;

            bool hasModel = _collectionModels.TryGetValue(snowflake.ParentCollectionData, out CollectionModel model);

            if (hasModel == false)
                return;

            bool isCompleted = model.AddSnowflake(snowflake);

            if (isCompleted)
            {
                OnCollectionCompleted?.Invoke(model);
                Debug.Log($"Коллекция {model.AbilityName} завершена");
                YandexGame.savesData.UnlockAbility(model);
            }
        }
    }
}