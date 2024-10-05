using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Components.Scripts.Menu
{
    public class Buttons : MonoBehaviour
    {
        [SerializeField]
        private SceneAsset _scene;

        public void LoadBaseGame()
        {
            if (_scene != null)
            {
                string scenePath = AssetDatabase.GetAssetPath(_scene);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("Scene is not set");
            }
        }
    }
}