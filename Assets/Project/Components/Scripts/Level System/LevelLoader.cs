using Project.Components.Scripts.Character_s;
using Project.Components.Scripts.Entities.Enemies;
using UnityEngine;

namespace Project.Components.Scripts.Level_System
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private Character _player;
        [SerializeField] private LevelSettings _levelSettings;

        private void LoadLevel()
        {
            
        }
    }
}