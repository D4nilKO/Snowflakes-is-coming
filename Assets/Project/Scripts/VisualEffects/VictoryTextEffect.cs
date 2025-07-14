using TMPro;
using UnityEngine;

namespace Project.VisualEffects
{
    public class VictoryTextEffect : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _tmpComponent;

        [SerializeField]
        private float _delayBetweenLetters = 0.1f;

        [SerializeField]
        private float _jumpHeight = 30f;

        [SerializeField]
        private float _duration = 0.5f;

        private void Start()
        {
            this.ValidateSerializedFields();

            PlayVictoryEffect();
        }

        private void PlayVictoryEffect()
        {
            string text = _tmpComponent.text;
            _tmpComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = _tmpComponent.textInfo;
            RectTransform parentRect = _tmpComponent.rectTransform;
            _tmpComponent.enabled = false;

            // Рассчитываем базовые параметры шрифта
            float fontSize = _tmpComponent.fontSize;
            float fontScale = fontSize / _tmpComponent.font.faceInfo.pointSize;
            float baseLineOffset = _tmpComponent.font.faceInfo.baseline * fontScale;

            for (int i = 0; i < text.Length; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                // Пропускаем невидимые символы и пробелы
                if (!charInfo.isVisible || char.IsWhiteSpace(text[i]))
                    continue;

                char letter = text[i];
                Vector3 charBottomLeft = charInfo.bottomLeft;
                Vector3 charTopRight = charInfo.topRight;

                float positionX = (charBottomLeft.x + charTopRight.x) / 2f;
                float positionY = charInfo.baseLine + baseLineOffset; // Ключевое исправление!

                Vector2 charPosition = new(positionX, positionY);
                float delay = i * _delayBetweenLetters;

                CreateLetterObject(letter, parentRect, charPosition, delay);
            }
        }

        private void CreateLetterObject(char letter, RectTransform parent, Vector2 position, float delay)
        {
            GameObject letterObj = new($"{letter}_Letter");
            RectTransform letterRect = letterObj.AddComponent<RectTransform>();
            letterRect.SetParent(parent, false);

            // Устанавливаем pivot в верхний центр
            letterRect.pivot = new Vector2(0.5f, 1f);
            letterRect.localScale = Vector3.one;

            TextMeshProUGUI letterTMP = letterObj.AddComponent<TextMeshProUGUI>();
            letterTMP.text = letter.ToString();
            letterTMP.font = _tmpComponent.font;
            letterTMP.fontSize = _tmpComponent.fontSize;
            letterTMP.color = _tmpComponent.color;
            letterTMP.alignment = TextAlignmentOptions.Top; // Выравнивание по верхнему краю
            letterRect.anchoredPosition = position;

            FloatingObject floatingObject = letterObj.AddComponent<FloatingObject>();
            floatingObject.Initialize(_jumpHeight, _duration, delay);
        }
    }
}