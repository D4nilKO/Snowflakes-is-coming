using System;
using TMPro;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Timing
{
    public abstract class TimerView : MonoBehaviour
    {
        [SerializeField]
        protected TMP_Text TimerText;

        protected virtual void Awake()
        {
            this.ValidateSerializedFields();
        }

        public abstract void Initialize(float timeToSurvive);
        protected abstract void UpdateText(float remainingTime, TimeChangingSource changingSource);
        protected abstract string FormatTime(float remainingTime);
    }
}