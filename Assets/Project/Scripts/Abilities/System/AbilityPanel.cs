using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Project.Abilities
{
    [RequireComponent(typeof(Canvas))]
    public class AbilityPanel : MonoBehaviour
    {
        [SerializeField]
        private AbilityView _buttonPrefab;

        [SerializeField]
        private RectTransform _buttonParent;

        private List<AbilityView> _buttons = new();
        private Canvas _canvas;

        private IAbilityFactory _factory;

        // нужно отдать размер в мировых координатах
        public float GetPanelWidth => _buttonParent.rect.width * _canvas.scaleFactor;

        [Inject]
        public void Construct(IAbilityFactory abilityFactory)
        {
            _factory = abilityFactory;
        }

        public void Initialize()
        {
            foreach (AbilityView abilityView in _buttons)
            {
                abilityView.Initialize();
            }
        }

        private void Awake()
        {
            this.ValidateSerializedFields();
            _canvas = GetComponent<Canvas>();
            CreateAbilityButtons(_factory.GetAbilities());
        }

        private void CreateAbilityButtons(IEnumerable<Ability> abilities)
        {
            ClearContainersChildren();

            foreach (Ability ability in abilities)
            {
                AbilityView abilityButton = Instantiate(_buttonPrefab, _buttonParent);
                abilityButton.SetAbility(ability);
                _buttons.Add(abilityButton);
            }
        }

        private void ClearContainersChildren()
        {
            int childCount = _buttonParent.childCount;

            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = _buttonParent.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }
}