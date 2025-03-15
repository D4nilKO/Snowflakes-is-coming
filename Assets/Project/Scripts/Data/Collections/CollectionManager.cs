using System;
using System.Collections.Generic;
using Project.Entities.Collectables;
using Project.Abilities;
using Project.Services.MessageSystem;
using YG;

namespace Project.Data.Collections
{
    public class CollectionManager
    {
        public event Action<Ability> OnCollectionCompleted;

        private Dictionary<CollectionData, CollectionModel> _collectionModels = new();

        private IAbilityFactory _abilityFactory;
        private IScreenMessageHandler _messageHandler;

        public CollectionManager(IAbilityFactory abilityFactory, IScreenMessageHandler messageHandler)
        {
            _abilityFactory = abilityFactory;
            _messageHandler = messageHandler;
        }

        public void RegisterCollection(CollectionData collectionData)
        {
            if (_collectionModels.ContainsKey(collectionData) == false)
            {
                Ability ability = _abilityFactory.CreateAbility(collectionData.UnlockableAbilityData);
                List<string> snowflakesNames = YandexGame.savesData.CollectedSnowflakes;

                collectionData.TryGetSnowflakesByNames(snowflakesNames, out List<CollectableSnowflake> snowflakes);
                _collectionModels[collectionData] = new CollectionModel(collectionData, ability, snowflakes);
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
            bool isNew = YandexGame.savesData.UnlockCollectableSnowflake(snowflake);

            string key = GetMessageKey(isNew, isCompleted, out string addText, model);
            _messageHandler.ShowMessage(key, addText);

            if (isCompleted == false)
                return;

            if (YandexGame.savesData.UnlockAbility(model.Ability))
                OnCollectionCompleted?.Invoke(model.Ability);
        }

        private string GetMessageKey(bool isNew, bool isCompleted, out string addText, CollectionModel model)
        {
            addText = $" {model.CollectedAmount}/{model.AllCollectedCount}";

            if (isNew)
            {
                if (isCompleted)
                {
                    addText = "";
                    return MessageKeys.MessageKeyCompleted;
                }

                return MessageKeys.MessageKeyCollected;
            }

            return MessageKeys.MessageKeyAlreadyCollected;
        }
    }
}