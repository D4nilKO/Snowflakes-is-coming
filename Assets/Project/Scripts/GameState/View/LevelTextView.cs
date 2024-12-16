using UnityEngine;
using YG;

namespace Project.GameState.View
{
    public class LevelTextView : MonoBehaviour
    {
        [SerializeField]
        private LangYGAdditionalText _additionalTextComponent;

        public void Initialize(int currentLevelNumber)
        {
            UpdateUIText(_additionalTextComponent, $" {currentLevelNumber}");
        }

        private void UpdateUIText(LangYGAdditionalText tmpText, string value)
        {
            tmpText.additionalText = value;
        }
    }
}