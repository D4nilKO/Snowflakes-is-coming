using System.Collections.Generic;

namespace Project.LevelSystem.LevelCreatingSystem
{
    public class EnemyNumberСombinator
    {
        private string _separator = ",";

        public List<List<string>> GetAllCombinations(int countEnemyTypes, int countOfNumberCells)
        {
            var result = new List<List<string>>();

            for (int i = 1; i <= countOfNumberCells; i++)
            {
                var combinations = GetNumberCombinations(countEnemyTypes, i);
                result.Add(combinations);
            }

            return result;
        }

        public List<string> GetNumberCombinations(int countEnemyTypes, int countOfNumberCells)
        {
            var results = new List<string>();
            GenerateNumberCombinations(results, new List<int>(), countEnemyTypes, countOfNumberCells, 1);
            return results;
        }

        public void GenerateNumberCombinations(
            List<string> results,
            List<int> current,
            int countEnemyTypes,
            int countOfNumberCells,
            int start)
        {
            if (current.Count == countOfNumberCells)
            {
                results.Add(string.Join(_separator, current));
                return;
            }

            for (int i = start; i <= countEnemyTypes; i++)
            {
                current.Add(i);
                GenerateNumberCombinations(results, current, countEnemyTypes, countOfNumberCells,
                    1);
                current.RemoveAt(current.Count - 1);
            }
        }
    }
}