using Project.Components.Scripts.Timing;
using UnityEngine;

namespace Project.Components.Scripts.Data
{
    [RequireComponent(typeof(PauseHandler))]
    public class ProgressSaverOnExit : MonoBehaviour
    {
        // private void OnApplicationQuit()
        // {
        //     if (Application.isEditor == false)
        //     {
        //         SaveData();
        //     }
        // }
    }
}