using NTC.Global.Pool;
using Project.Data.Collections;
using Project.Entities.Enemies;
using UnityEngine;
using VContainer;

namespace Project.Entities.Collectables
{
    public class CollectableSnowflake : EnemyBase, ICollectable
    {
        [HideInInspector]
        public CollectionData ParentCollectionData;

        private CollectionManager _collectionManager;

        [Inject]
        public void Construct(CollectionManager collectionManager)
        {
            _collectionManager = collectionManager;
        }

        public void Collect()
        {
            _collectionManager.AddToCollection(this);
            NightPool.Despawn(this);
        }
    }
}