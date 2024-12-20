using UnityEngine;
using DG.Tweening;
using TMPro;

public class VictoryTextEffect : MonoBehaviour
{
    public TMP_Text tmpComponent; // Ссылка на компонент TMP_Text
    public float jumpHeight = 20f; // Высота прыжка
    public float duration = 0.5f; // Длительность прыжка
    public float delayBetweenLetters = 0.1f; // Задержка между буквами

    private void Start()
    {
        PlayVictoryEffect();
    }

    private void PlayVictoryEffect()
    {
        if (tmpComponent == null)
        {
            Debug.LogError("TMP_Text компонент не назначен!");
            return;
        }

        string text = tmpComponent.text;
        tmpComponent.ForceMeshUpdate(); // Обновляем текстовую информацию
        TMP_TextInfo textInfo = tmpComponent.textInfo;

        // Создаём родительский объект для букв
        RectTransform parentRect = tmpComponent.rectTransform;
        tmpComponent.enabled = false; // выключаем исходный текст, чтобы буквы отображались только через анимацию

        for (int i = 0; i < text.Length; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue; // Пропускаем невидимые символы

            char letter = text[i];
            int index = i;

            // Рассчитываем позицию символа
            Vector3 charBottomLeft = charInfo.bottomLeft;
            Vector3 charTopRight = charInfo.topRight;

            Vector2 charPosition = new Vector2(
                (charBottomLeft.x + charTopRight.x) / 2f,
                (charBottomLeft.y + charTopRight.y) / 2f
            );

            GameObject letterObj = CreateLetterObject(letter, parentRect, charPosition);
            if (letterObj == null) continue;

            RectTransform letterRect = letterObj.GetComponent<RectTransform>();

            // Анимация прыжка
            letterRect.DOAnchorPosY(jumpHeight, duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
                .SetDelay(index * delayBetweenLetters)
                .SetUpdate(true);
        }
    }

    private GameObject CreateLetterObject(char letter, RectTransform parent, Vector2 position)
    {
        GameObject letterObj = new GameObject($"Letter_{letter}");
        RectTransform letterRect = letterObj.AddComponent<RectTransform>();

        letterRect.SetParent(parent, false);
        letterRect.localScale = Vector3.one;

        // Добавляем компонент TextMeshProUGUI для отображения буквы
        TextMeshProUGUI letterTMP = letterObj.AddComponent<TextMeshProUGUI>();
        letterTMP.text = letter.ToString();
        letterTMP.font = tmpComponent.font;
        letterTMP.fontSize = tmpComponent.fontSize;
        letterTMP.color = tmpComponent.color;
        letterTMP.alignment = TextAlignmentOptions.Top;

        // Устанавливаем начальную позицию буквы
        letterRect.anchoredPosition = position;

        return letterObj;
    }
}