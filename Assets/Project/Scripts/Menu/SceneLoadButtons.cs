using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Menu
{
    public class SceneLoadButtons : MonoBehaviour
    {
        [SerializeField]
        private int _menuSceneBuildIndex = 0;

        [SerializeField]
        private int _gameSceneBuildIndex = 1;

        public void LoadBaseGame()
        {
            SceneManager.LoadScene(_gameSceneBuildIndex);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(_menuSceneBuildIndex);
        }
    }
}