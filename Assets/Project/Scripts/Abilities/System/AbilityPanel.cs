using System.Collections.Generic;
using System.Linq;
using Project.Data.Collections;
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
        private CollectionManager _collectionManager;

        public float PanelWidth
        {
            get
            {
                if (_buttonParent.gameObject.activeInHierarchy)
                    return 0f;

                return _buttonParent.rect.width * _canvas.scaleFactor;
            }
        }

        [Inject]
        public void Construct(IAbilityFactory abilityFactory, CollectionManager collectionManager)
        {
            _factory = abilityFactory;
            _collectionManager = collectionManager;
        }

        private void Start()
        {
            this.ValidateSerializedFields();
            _canvas = GetComponent<Canvas>();

            CreateAbilityButtons(_factory.GetUnlockedAbilities());

            _collectionManager.OnCollectionCompleted += CreateAbilityButton;
        }

        private void OnDestroy()
        {
            _collectionManager.OnCollectionCompleted -= CreateAbilityButton;
        }

        public void Initialize()
        {
            foreach (AbilityView abilityView in _buttons)
            {
                abilityView.Initialize();
            }
        }

        private void CreateAbilityButtons(List<Ability> abilities)
        {
            ClearContainersChildren();

            if (abilities.Any() == false)
            {
                _buttonParent.gameObject.SetActive(false);
                return;
            }

            foreach (Ability ability in abilities)
            {
                CreateAbilityButton(ability);
            }
        }

        private void CreateAbilityButton(Ability ability)
        {
            if (ability == null)
            {
                Debug.LogError("При создании кнопки способности вылезла ошибка: Ability is null");
                return;
            }

            _buttonParent.gameObject.SetActive(true);

            AbilityView abilityButton = Instantiate(_buttonPrefab, _buttonParent);
            abilityButton.SetAbility(ability);
            _buttons.Add(abilityButton);
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