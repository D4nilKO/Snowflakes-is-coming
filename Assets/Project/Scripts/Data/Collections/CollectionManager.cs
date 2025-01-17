using System;
using Project.Entities.Collectables;

namespace Project.Data.Collections
{
    public class CollectionManager
    {
        public event Action<CollectionModel> OnCollectionCompleted;

        public void AddToCollection(CollectableSnowflake snowflake)
        {
            
        }
    }
}