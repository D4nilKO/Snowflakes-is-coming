using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Menu
{
    public class Buttons : MonoBehaviour
    {
        [SerializeField]
        private int _sceneBuildIndex = 1;

        public void LoadBaseGame()
        {
            SceneManager.LoadScene(_sceneBuildIndex);
        }
    }
}