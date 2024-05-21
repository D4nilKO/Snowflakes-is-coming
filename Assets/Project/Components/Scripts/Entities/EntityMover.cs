using System.Collections.Generic;
using System.Linq;
using Project.Components.Scripts.Character_s;
using Project.Components.Scripts.Data;
using Project.Components.Scripts.Entities.Enemies;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class EntityMover : MonoBehaviour
    {
        [SerializeField] private GameObject _characterPrefab;
        
        private List<EnemyBase> _enemies;
        private Character _character;

        private void Awake()
        {
            InitializeCharacter();
        }

        private void Start()
        {
            _enemies = FindObjectsOfType<EnemyBase>().ToList();
        }

        private void FixedUpdate()
        {
            foreach (EnemyBase enemy in _enemies.Where(enemy => enemy.isActiveAndEnabled))
            {
                enemy.Move();
                enemy.Rotate();
            }

            _character.Move();
        }

        private void InitializeCharacter()
        {
            if (GameData.IsCharacterSpawned == false)
            {
                GameObject characterObject = Instantiate(_characterPrefab);
                GameData.IsCharacterSpawned = true;
                
                _character = characterObject.GetComponent<Character>();
                CharacterCollisionHandler collisionHandler = characterObject.GetComponent<CharacterCollisionHandler>();
                collisionHandler.Awake();
            }
            else
            {
                _character = FindObjectOfType<Character>();
            }

            _character.Awake();
        }
        public void AddEnemy(EnemyBase enemy)
        {
            _enemies.Add(enemy);
        }
    }
}