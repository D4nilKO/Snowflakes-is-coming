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
            
            string addText = $" {model.CollectedAmount}/{model.AllCollectedCount}";

            string key = isNew
                ? MessageKeys.MessageKeyCollected
                : MessageKeys.MessageKeyAlreadyCollected;

            if (isCompleted)
            {
                key = MessageKeys.MessageKeyCompleted;
                addText = "";
            }

            _messageHandler.ShowMessage(key, addText);

            if (isCompleted == false)
                return;

            bool isNewUnlock = YandexGame.savesData.UnlockAbility(model.Ability);

            if (isNewUnlock == false)
                return;

            OnCollectionCompleted?.Invoke(model.Ability);
        }
    }
}