using TMPro;
using UnityEngine;
using static Project.Components.Scripts.Data.ProgressData;

namespace Project.Components.Scripts
{
    public class LevelTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelTMPText;
        [SerializeField] private string _levelText;

        private void Awake()
        {
            UpdateUIText(_levelTMPText, $"{_levelText}{CurrentLevelNumber}");
        }

        private void UpdateUIText(TMP_Text tmpText, string value)
        {
            tmpText.text = value;
        }
    }
}