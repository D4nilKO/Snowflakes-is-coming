using System;
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
        private Camera _cameraCache;

        private IAbilityFactory _factory;
        private CollectionManager _collectionManager;
        private AbilityInputRegistry _abilityInputRegistry;

        public event Action OnPanelResized;

        [Inject]
        public void Construct(IAbilityFactory abilityFactory, CollectionManager collectionManager,
            AbilityInputRegistry abilityInputRegistry)
        {
            _factory = abilityFactory;
            _collectionManager = collectionManager;
            _abilityInputRegistry = abilityInputRegistry;
        }

        private void Awake()
        {
            this.ValidateSerializedFields();

            _canvas = GetComponent<Canvas>();
            _cameraCache = _canvas.worldCamera;

            List<Ability> unlockedAbilities = _factory.GetAllUnlocked();
            CreateAbilityButtons(unlockedAbilities);

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

        public float GetPanelWidth()
        {
            if (_canvas == null)
                _canvas = GetComponent<Canvas>();

            if (_buttonParent.gameObject.activeInHierarchy == false)
                return 0f;

            return _buttonParent.rect.width * _canvas.scaleFactor;
        }

        private void CreateAbilityButtons(List<Ability> abilities)
        {
            ClearContainersChildren();

            if (abilities.Any() == false)
            {
                HidePanel();
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
                Debug.LogError("При создании кнопки способности возникла ошибка: Ability is null");
                return;
            }

            if (_buttons.Any(view => view.Ability == ability))
            {
                return;
            }

            ShowPanel();

            AbilityView abilityButton = Instantiate(_buttonPrefab, _buttonParent);
            KeyCode assignedKey = _abilityInputRegistry.GetAbilityKey(ability);
            abilityButton.SetAbility(ability, assignedKey);
            _buttons.Add(abilityButton);
        }

        private void ShowPanel()
        {
            _buttonParent.gameObject.SetActive(true);
            OnPanelResized?.Invoke();
        }

        private void HidePanel()
        {
            _buttonParent.gameObject.SetActive(false);
            OnPanelResized?.Invoke();
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

        public bool IsTouchOnPanel(Vector2 screenPosition)
        {
            if (!_buttonParent.gameObject.activeInHierarchy)
                return false;

            return RectTransformUtility.RectangleContainsScreenPoint(
                _buttonParent,
                screenPosition,
                _cameraCache
            );
        }
    }
}