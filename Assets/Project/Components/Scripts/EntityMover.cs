using System;
using System.Collections.Generic;
using System.Linq;
using Project.Components.Scripts.Character_s;
using Project.Components.Scripts.Data;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class EntityMover : MonoBehaviour
    {
        [HideInInspector] public List<EnemyBase> enemies;
        public GameObject characterPrefab;
        private GameObject currentCharacter;
        private Character character;

        private void Awake()
        {
            InitializeCharacter();
        }

        private void InitializeCharacter()
        {
            if (GameData.characterIsSpawned == false)
            {
                GameData.characterIsSpawned = true;
                currentCharacter = Instantiate(characterPrefab);
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

        private void Start()
        {
            enemies = FindObjectsOfType<EnemyBase>().ToList();
        }

        private void FixedUpdate()
        {
            foreach (var enemy in enemies.Where(enemy => enemy.isActiveAndEnabled))
            {
                enemy.Move();
                enemy.Rotate();
            }

            character.Move();
        }
    }
}