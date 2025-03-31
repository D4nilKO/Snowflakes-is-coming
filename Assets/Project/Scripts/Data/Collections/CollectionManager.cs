using System;
using System.Collections.Generic;
using System.Linq;
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
                Ability ability = _abilityFactory.Create(collectionData.UnlockableAbilityData);
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
            bool isNew = SaveUnlockedCollectableSnowflake(snowflake);

            string key = GetMessageKey(isNew, isCompleted, out string addText, model);
            _messageHandler.ShowMessage(key, addText);

            if (isCompleted == false)
                return;

            if (SaveUnlockedAbility(model.Ability))
                OnCollectionCompleted?.Invoke(model.Ability);
        }

        public bool SaveUnlockedCollectableSnowflake(CollectableSnowflake snowflake)
        {
            string objectName = snowflake.name.Replace("(Clone)", "");

            if (YandexGame.savesData.CollectedSnowflakes.Contains(objectName))
                return false;

            YandexGame.savesData.CollectedSnowflakes.Add(objectName);
            YandexGame.SaveProgress();

            return true;
        }

        public bool SaveUnlockedAbility(Ability ability)
        {
            string abilityName = ability.CommonData.AbilityName;

            if (YandexGame.savesData.UnlockedAbilityNames.Contains(abilityName))
                return false;

            YandexGame.savesData.UnlockedAbilityNames.Add(abilityName);
            YandexGame.SaveProgress();

            return true;
        }

        public void UnlockAllAbilities()
        {
            foreach (CollectableSnowflake snowflake in _collectionModels.Keys.SelectMany(data => data.Snowflakes))
                AddToCollection(snowflake);
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