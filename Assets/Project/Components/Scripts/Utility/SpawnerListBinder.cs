// using System.Collections.Generic;
// using UnityEngine;
//
// #if UNITY_EDITOR
//
// namespace Project.Components.Scripts.Utility
// {
//     [ExecuteAlways]
//     [DisallowMultipleComponent]
//     [RequireComponent(typeof(EnemySpawner))]
//     public class SpawnerListBinder : MonoBehaviour
//     {
//         private EnemySpawner _enemySpawner;
//         private List<GameObject> attachedList1;
//         private List<int> attachedList2;
//
//         private void Awake()
//         {
//             _enemySpawner = GetComponent<EnemySpawner>();
//         }
//         
//         private void Update()
//         {
//             if (Application.IsPlaying(this))
//             {
//                 var go = GetComponent<SpawnerListBinder>();
//                 go.enabled = false;
//             }
//
//             if (attachedList1 != _enemySpawner.enemyList && attachedList2 != _enemySpawner.enemyCount)
//             {
//                 attachedList1 = _enemySpawner.enemyList; // Откуда берем размер
//                 attachedList2 = _enemySpawner.enemyCount; // Кому присваиваем размер
//             }
//             
//             if (attachedList1.Count == attachedList2.Count)
//                 return;
//
//             attachedList2.Resize(attachedList1.Count);
//         }
//     }
// }
//
// #endif