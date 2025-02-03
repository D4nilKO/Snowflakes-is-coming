using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VavilichevGD.Utils.Timing;

namespace Project.Abilities
{
    public class AbilityView : MonoBehaviour, IPointerUpHandler, IDisposable
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _abilityName;

        [SerializeField]
        private Image _cooldownOverlay;

        [SerializeField]
        private TMP_Text _cooldownText;

        private Ability _ability;

        public void SetAbility(Ability ability)
        {
            _ability = ability;
            _ability.CooldownTimer.TimerValueChanged += UpdateCooldownUI;
            ResetCooldownUI();
        }

        private void Start()
        {
            this.ValidateSerializedFields();
        }

        // private void Update()
        // {
        //     UpdateCooldownUI();
        // }

        private void UpdateCooldownUI(float remainingTime, TimeChangingSource changingSource)
        {
            // float remainingTime = _ability.GetRemainingCooldown();
            float cooldownTime = _ability.GetCooldown();

            if (cooldownTime > 0 && remainingTime > 0)
            {
                UpdateFade(remainingTime, cooldownTime);
                UpdateText(remainingTime);
            }
            else
            {
                ResetCooldownUI();
            }
        }

        private void UpdateText(float remainingTime)
        {
            _cooldownText.text = Mathf.CeilToInt(remainingTime).ToString();
            _cooldownText.gameObject.SetActive(true);
        }

        private void UpdateFade(float remainingTime, float cooldownTime)
        {
            float fillAmount = Mathf.Clamp01(remainingTime / cooldownTime);
            _cooldownOverlay.fillAmount = fillAmount;
        }

        private void ResetCooldownUI()
        {
            _cooldownOverlay.fillAmount = 0f;
            _cooldownText.gameObject.SetActive(false);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ActivateAbility();
        }

        private void ActivateAbility()
        {
            if (_ability.IsReady())
            {
                _ability.TryActivate();
            }
        }

        public void Dispose()
        {
            _ability.CooldownTimer.TimerValueChanged -= UpdateCooldownUI;
        }
    }
}