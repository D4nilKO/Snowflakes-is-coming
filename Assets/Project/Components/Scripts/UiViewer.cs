using TMPro;
using UnityEngine;
using static Project.Components.Scripts.Data.GameData;

namespace Project.Components.Scripts
{
    public class UiViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelTMPText;
        [SerializeField] private string levelText;

        private void Awake()
        {
            UpdateUIText(levelTMPText,levelText + currentLevelNumber);
        }

        private void UpdateUIText(TMP_Text tmpText, string value)
        {
            tmpText.text = value;
        }
    }
}