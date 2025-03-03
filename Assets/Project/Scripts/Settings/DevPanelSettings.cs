using UnityEngine;

namespace Project.Settings
{
    [CreateAssetMenu(fileName = "DevPanelSettings", menuName = "Settings/DevPanelSettings")]
    public class DevPanelSettings : ScriptableObject
    {
        [field: SerializeField]
        public string ShowCommand = "dev39611";

        [field: SerializeField]
        public string HideCommand = "hide";

        private void OnValidate()
        {
            ShowCommand = ShowCommand.ToLower();
            HideCommand = HideCommand.ToLower();
        }
    }
}