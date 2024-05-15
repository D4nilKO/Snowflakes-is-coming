using UnityEngine;

namespace Project.Components.Scripts.Character_s
{
    [DisallowMultipleComponent]
    public class SkinManager : MonoBehaviour
    {
        [SerializeField] private int numberOfDefaultSkin;
        [SerializeField] [Space(10)] private CharacterSkin[] skins;

        [SerializeField] private CharacterSkin currentCharacterSkin;
        private SpriteRenderer characterRenderer;
        private PolygonCollider2D characterCollider;

        public CharacterSkin GetDefaultSkin()
        {
            return skins[numberOfDefaultSkin];
        }

        public void ChangeSkin(CharacterSkin newSkin)
        {
            currentCharacterSkin = newSkin;

            characterCollider.points = newSkin.SkinCollider.points;
            characterCollider = newSkin.SkinCollider;

            characterRenderer.sprite = newSkin.SkinSprite;
        }

        private void Awake()
        {
            skins[numberOfDefaultSkin].IsUnlocked = true;
            characterRenderer = GetComponent<SpriteRenderer>();
            characterCollider = GetComponent<PolygonCollider2D>();

            if (currentCharacterSkin != GetDefaultSkin())
            {
                ChangeSkin(GetDefaultSkin());
            }
        }
    }
}