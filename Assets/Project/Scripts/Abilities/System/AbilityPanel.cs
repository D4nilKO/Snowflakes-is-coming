using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Project.Abilities
{
    public class AbilityPanel : MonoBehaviour
    {
        [SerializeField]
        private AbilityView _abilityButtonPrefab;

        [SerializeField]
        private Transform _abilityButtonParent;

        private List<AbilityView> _abilityButtons = new();

        private IAbilityFactory _abilityFactory;

        [Inject]
        public void Construct(IAbilityFactory abilityFactory)
        {
            _abilityFactory = abilityFactory;
        }

        private void Awake()
        {
            this.ValidateSerializedFields();
            CreateAbilityButtons(_abilityFactory.GetAbilities());
        }

        private void CreateAbilityButtons(IEnumerable<Ability> abilities)
        {
            ClearContainersChildren();

            foreach (Ability ability in abilities)
            {
                AbilityView abilityButton = Instantiate(_abilityButtonPrefab, _abilityButtonParent);
                abilityButton.SetAbility(ability);
                _abilityButtons.Add(abilityButton);
            }
        }

        private void ClearContainersChildren()
        {
            int childCount = _abilityButtonParent.childCount;

            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = _abilityButtonParent.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }
}