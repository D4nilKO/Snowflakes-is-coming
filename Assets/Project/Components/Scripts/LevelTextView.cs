using TMPro;
using UnityEngine;

namespace Project.Components.Scripts
{
    public class LevelTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelTMPText;
        [SerializeField] private string _levelText;

        public void Init(int currentLevelNumber)
        {
            UpdateUIText(_levelTMPText, $"{_levelText}{currentLevelNumber}");
        }

        private void UpdateUIText(TMP_Text tmpText, string value)
        {
            tmpText.text = value;
        }
    }
}