using NTC.Global.Pool;
using Project.Data.Collections;
using Project.Entities.Enemies;
using Project.Services.MessageSystem;
using UnityEngine;
using VContainer;

namespace Project.Entities.Collectables
{
    public class CollectableSnowflake : EnemyBase, ICollectable
    {
        // Не трогать, так надо.
        // Это поле проставляется скриптом, когда формируется коллекция в инспекторе.
        // А скрыто оно, чтобы не сбить ссылку руками.
        [HideInInspector]
        public CollectionData ParentCollectionData;

        private CollectionManager _collectionManager;
        private IScreenMessageHandler _screenMessageHandler;

        [Inject]
        public void Construct(CollectionManager collectionManager, IScreenMessageHandler screenMessageHandler)
        {
            _collectionManager = collectionManager;
            _screenMessageHandler = screenMessageHandler;
        }

        public void Initialize()
        {
            _screenMessageHandler.ShowMessage(MessageKeys.MessageKeySpawned);
        }

        public void Collect()
        {
            _collectionManager.AddToCollection(this);
            NightPool.Despawn(this);
        }
    }
}