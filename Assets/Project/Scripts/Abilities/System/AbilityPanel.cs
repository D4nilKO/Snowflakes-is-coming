using System.Collections.Generic;
using Project.Data.Collections;
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
        
        private CollectionManager _collectionManager;

        private void Awake()
        {
            this.ValidateSerializedFields();
        }

        [Inject]
        public void Construct(CollectionManager collectionManager)
        {
            _collectionManager = collectionManager;
        }

        public void CreateAbilityButtons(List<Ability> abilities)
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