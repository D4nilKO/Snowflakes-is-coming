using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Abilities
{
    [CreateAssetMenu(fileName = "AbilityKeyCodesBank", menuName = "Settings/AbilityKeyCodesBank", order = 0)]
    public class AbilityKeyCodesBank : ScriptableObject
    {
        [SerializeField]
        private List<KeyCode> _keycodes = new();

        public KeyCode GetKeycode(int index)
        {
            return index < _keycodes.Count
                ? _keycodes[index]
                : KeyCode.None;
        }
    }
}