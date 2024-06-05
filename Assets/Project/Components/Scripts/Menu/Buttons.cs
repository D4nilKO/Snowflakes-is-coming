using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Components.Scripts.Menu
{
    public class Buttons : MonoBehaviour
    {
        [SerializeField] private Scene _scene;
        
        public void LoadBaseGame()
        {
            SceneManager.LoadScene(_scene.buildIndex);
        }
    }
}