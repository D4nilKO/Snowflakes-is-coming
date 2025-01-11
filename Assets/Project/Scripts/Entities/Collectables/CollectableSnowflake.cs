using Project.Data.Collections;
using Project.Entities.Enemies;
using VContainer;

namespace Project.Entities.Collectables
{
    public class CollectableSnowflake : EnemyBase, ICollectable
    {
        public int Id { set; get; }
        
        private CollectionManager _collectionManager;

        [Inject]
        public void Construct(CollectionManager collectionManager)
        {
            _collectionManager = collectionManager;
        }

        public void Collect()
        {
            
        }
    }
}