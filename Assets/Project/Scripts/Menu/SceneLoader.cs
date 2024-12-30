using UnityEngine.SceneManagement;

namespace Project.Menu
{
    public class SceneLoader
    {
        private const int menuSceneBuildIndex = 0;
        private const int gameSceneBuildIndex = 1;

        public void LoadBaseGame()
        {
            SceneManager.LoadScene(gameSceneBuildIndex);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(menuSceneBuildIndex);
        }
    }
}