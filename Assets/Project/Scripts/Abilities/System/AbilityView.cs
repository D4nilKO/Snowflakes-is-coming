using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Utils.Timing;

namespace Project.Abilities
{
    [RequireComponent(typeof(Button))]
    public class AbilityView : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _abilityName;

        [SerializeField]
        private Image _cooldownOverlay;

        [SerializeField]
        private TMP_Text _cooldownText;

        private Button _button;
        private float _cooldownTime;

        public Ability Ability { get; private set; }

        private void Awake()
        {
            this.ValidateSerializedFields();
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(() => Ability?.TryActivate());
        }

        public void SetAbility(Ability ability)
        {
            Ability = ability;

            _abilityName.text = ability.Data.AbilityName;
            _icon.sprite = ability.Data.Icon;
            _cooldownTime = ability.Data.Cooldown;

            Ability.CooldownTimer.TimerValueChanged += UpdateCooldownUI;
            ResetCooldownUI();
        }

        public void Initialize()
        {
            ResetCooldownUI();
        }

        private void UpdateCooldownUI(float remainingTime, TimeChangingSource changingSource)
        {
            if (_cooldownTime > 0 && remainingTime > 0)
            {
                UpdateFade(remainingTime, _cooldownTime);
                UpdateText(remainingTime);
            }
            else
            {
                ResetCooldownUI();
            }
        }

        private void UpdateText(float remainingTime)
        {
            if (_cooldownText == null)
                return;

            _cooldownText.gameObject.SetActive(true);
            _cooldownText.text = Mathf.CeilToInt(remainingTime).ToString();
        }

        private void UpdateFade(float remainingTime, float cooldownTime)
        {
            if (_cooldownOverlay == null)
                return;

            _cooldownOverlay.enabled = true;
            _cooldownOverlay.fillAmount = Mathf.Clamp01(remainingTime / cooldownTime);
        }

        private void ResetCooldownUI()
        {
            if (_cooldownOverlay != null)
                _cooldownOverlay.enabled = false;

            if (_cooldownText != null)
                _cooldownText.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            if (Ability != null)
                Ability.CooldownTimer.TimerValueChanged -= UpdateCooldownUI;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}