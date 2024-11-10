// using System.Linq;
// using NUnit.Framework;
// using Project.Components.Scripts.Level_System.LevelCreatingSystem;
// using UnityEngine;
// using UnityEngine.Assertions;
// using UnityEngine.TestTools;
//
// // [UnityTest]
// public class CombinatorTests
// {
//     public EnemyNumberСombinator _сombinator = new();
//
//     private int GetTrueCount(int countEnemyTypes, int countOfNumberCells)
//     {
//         return (int)(Mathf.Pow(countEnemyTypes, countOfNumberCells + 1) - countEnemyTypes);
//     }
//
//     [Test]
//     public void TestCount_2_4()
//     {
//         int countEnemyTypes = 2;
//         int countOfNumberCells = 4;
//
//         var combinations = _сombinator.GetAllCombinations(countEnemyTypes, countOfNumberCells);
//
//         int totalCount = combinations.Sum(c => c.Count);
//
//         Assert.That(totalCount, Is.EqualTo(GetTrueCount(countEnemyTypes, countOfNumberCells)));
//     }
//     
//     [Test]
//     public void TestCount_3_4()
//     {
//         int countEnemyTypes = 3;
//         int countOfNumberCells = 4;
//
//         var combinations = _сombinator.GetAllCombinations(countEnemyTypes, countOfNumberCells);
//
//         int totalCount = combinations.Sum(c => c.Count);
//
//         Assert.That(totalCount, Is.EqualTo(GetTrueCount(countEnemyTypes, countOfNumberCells)));
//     }
// }