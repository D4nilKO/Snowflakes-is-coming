using System.Collections.Generic;
using System.Linq;
using Project.Components.Scripts.Character_s;
using Project.Components.Scripts.Data;
using Project.Components.Scripts.Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class EntityMover : MonoBehaviour
    {
        [HideInInspector] public List<EnemyBase> enemies; // Переделать в private
        [FormerlySerializedAs("characterPrefab")] [SerializeField] private GameObject _characterPrefab;
        private GameObject currentCharacter;
        private Character character;

        private void Awake()
        {
            InitializeCharacter();
        }

        private void Start()
        {
            enemies = FindObjectsOfType<EnemyBase>().ToList();
        }

        private void FixedUpdate()
        {
            foreach (EnemyBase enemy in enemies.Where(enemy => enemy.isActiveAndEnabled))
            {
                enemy.Move();
                enemy.Rotate();
            }

            character.Move();
        }

        private void InitializeCharacter()
        {
            if (GameData.characterIsSpawned == false)
            {
                GameData.characterIsSpawned = true;
                currentCharacter = Instantiate(_characterPrefab);
            }
            else
            {
                character = FindObjectOfType<Character>();
            }

            currentCharacter = FindObjectOfType<Character>().gameObject;
            character = currentCharacter.GetComponent<Character>();
            currentCharacter.GetComponent<CharacterCollisionHandler>().Awake();
            character.Awake();
        }
    }
}