using System;
using UnityEngine;

namespace Project.Components.Scripts.Character_s
{
    [Serializable]
    public class CharacterSkin
    {
        [SerializeField] private string skinName;
        [SerializeField] private Sprite skinSprite;
        [SerializeField] private PolygonCollider2D skinCollider;

        private bool isUnlocked;

        public string SkinName
        {
            get => skinName;
            set => skinName = value;
        }

        public Sprite SkinSprite
        {
            get => skinSprite;
            set => skinSprite = value;
        }

        public PolygonCollider2D SkinCollider
        {
            get => skinCollider;
            set => skinCollider = value;
        }

        public bool IsUnlocked
        {
            get => isUnlocked;
            set => isUnlocked = value;
        }
    }
}