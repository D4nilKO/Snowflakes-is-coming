using System;
using UnityEngine;

namespace Project.Components.Scripts.Entities.Character
{
    [Serializable]
    public class CharacterSkin
    {
        [SerializeField] private string _skinName;
        [SerializeField] private Sprite _skinSprite;
        [SerializeField] private PolygonCollider2D _skinCollider;

        private bool _isUnlocked;

        public string SkinName
        {
            get => _skinName;
            set => _skinName = value;
        }

        public Sprite SkinSprite
        {
            get => _skinSprite;
            set => _skinSprite = value;
        }

        public PolygonCollider2D SkinCollider
        {
            get => _skinCollider;
            set => _skinCollider = value;
        }

        public bool IsUnlocked
        {
            get => _isUnlocked;
            set => _isUnlocked = value;
        }
    }
}