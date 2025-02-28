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
        
        public Ability Ability;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => Ability.TryActivate());

            this.ValidateSerializedFields();
        }

        public void SetAbility(Ability ability)
        {
            Ability = ability;
            _abilityName.text = ability.GetData().AbilityName;
            _icon.sprite = ability.GetData().Icon;
            _cooldownTime = Ability.GetCooldown();

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
            if (!_cooldownText)
                return;
            
            _cooldownText.gameObject.SetActive(true);
            _cooldownText.text = Mathf.CeilToInt(remainingTime).ToString();
        }

        private void UpdateFade(float remainingTime, float cooldownTime)
        {
            if (!_cooldownOverlay)
                return;

            float fillAmount = Mathf.Clamp01(remainingTime / cooldownTime);

            _cooldownOverlay.enabled = true;
            _cooldownOverlay.fillAmount = fillAmount;
        }

        private void ResetCooldownUI()
        {
            _cooldownOverlay.enabled = false;
            _cooldownText.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            Ability.CooldownTimer.TimerValueChanged -= UpdateCooldownUI;
        }
    }
}