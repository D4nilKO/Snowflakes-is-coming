using System.Collections.Generic;
using UnityEngine;

namespace Project.Abilities
{
    public class AbilityInputManager : MonoBehaviour
    {
        private readonly List<AbilityKeyboardInput> _inputs = new();

        private void Update()
        {
            foreach (var input in _inputs)
            {
                input.Tick();
            }
        }

        public void Register(AbilityKeyboardInput input)
        {
            if (_inputs.Contains(input) == false)
                _inputs.Add(input);
        }

        public void Unregister(AbilityKeyboardInput input)
        {
            _inputs.Remove(input);
        }
    }
}