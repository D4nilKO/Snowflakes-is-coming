using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Data.Collections
{
    public class CollectionManager
    {
        public List<CollectionModel> Collections { get; private set; } = new List<CollectionModel>();
        public event Action<CollectionModel> OnCollectionCompleted;

        public void AddToCollection(CollectableSnowflakeData snowflakeData)
        {
            var collectionModel = Collections.FirstOrDefault(c => c.CollectionData == snowflakeData.ParentCollection);

            if (collectionModel != null && collectionModel.AddSnowflake(snowflakeData))
            {
                OnCollectionCompleted?.Invoke(collectionModel);
                collectionModel.ActivateAbility();
            }
        }
    }
}