using UnityEngine;

namespace Project.Entities.Character
{
    public abstract class Ability : ScriptableObject
    {
        public string AbilityName;
        public Sprite Icon;

        public abstract void Activate();
        public abstract void Deactivate();
    }
}