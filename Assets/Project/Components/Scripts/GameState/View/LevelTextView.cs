using TMPro;
using UnityEngine;

namespace Project.Components.Scripts.GameState.View
{
    public class LevelTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelTMPText;
        [SerializeField] private string _levelText;

        public void Initialize(int currentLevelNumber)
        {
            UpdateUIText(_levelTMPText, $"{_levelText}{currentLevelNumber}");
        }

        private void UpdateUIText(TMP_Text tmpText, string value)
        {
            tmpText.text = value;
        }
    }
}