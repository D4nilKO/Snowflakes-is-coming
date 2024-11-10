using UnityEngine;

namespace Project.Entities.Character
{
    [DisallowMultipleComponent]
    public class SkinManager : MonoBehaviour
    {
        [SerializeField] private int _numberOfDefaultSkin;
        [SerializeField] [Space(10)] private CharacterSkin[] _skins;

        [SerializeField] private CharacterSkin _currentCharacterSkin;

        private SpriteRenderer _characterRenderer;
        private PolygonCollider2D _characterCollider;

        private void Awake()
        {
            _skins[_numberOfDefaultSkin].IsUnlocked = true;

            _characterRenderer = GetComponent<SpriteRenderer>();
            _characterCollider = GetComponent<PolygonCollider2D>();

            if (_currentCharacterSkin != GetDefaultSkin())
                ChangeSkin(GetDefaultSkin());
        }

        private CharacterSkin GetDefaultSkin()
        {
            return _skins[_numberOfDefaultSkin];
        }

        private void ChangeSkin(CharacterSkin newSkin)
        {
            _currentCharacterSkin = newSkin;

            _characterCollider.points = newSkin.SkinCollider.points;
            _characterCollider = newSkin.SkinCollider;

            _characterRenderer.sprite = newSkin.SkinSprite;
        }
    }
}