using System;
using UnityEngine;

namespace Project.Components.Scripts.Character_s
{
    [Serializable]
    public class CharacterSkin
    {
        [SerializeField] private string skinName;

        public string SkinName
        {
            get => skinName;
            set => skinName = value;
        }

        [SerializeField] private Sprite skinSprite;

        public Sprite SkinSprite
        {
            get => skinSprite;
            set => skinSprite = value;
        }

        [SerializeField] private PolygonCollider2D skinCollider;

        public PolygonCollider2D SkinCollider
        {
            get => skinCollider;
            set => skinCollider = value;
        }

        private bool isUnlocked;

        public bool IsUnlocked
        {
            get => isUnlocked;
            set => isUnlocked = value;
        }
    }
}