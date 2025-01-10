using Project.Entities.Enemies;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Timing
{
    [RequireComponent(typeof(EnemySpawner))]
    public class SpawnerTimerView : TimerView
    {
        private EnemySpawner _spawner;

        protected override void Awake()
        {
            base.Awake();

            _spawner = GetComponent<EnemySpawner>();
            _spawner.Initialized += Initialize;
        }

        private void OnDisable()
        {
            _spawner.Timer.TimerValueChanged -= UpdateText;
        }

        private void OnDestroy()
        {
            _spawner.Initialized -= Initialize;
        }

        public override void Initialize(float timeToSurvive)
        {
            _spawner.Timer.TimerValueChanged += UpdateText;
            UpdateText(timeToSurvive, default);
        }

        protected override void UpdateText(float remainingTime, TimeChangingSource changingSource)
        {
            float border = 0.2f;

            TimerText.text = FormatTime(remainingTime);

            TimerText.color = remainingTime < border
                ? Color.red
                : Color.white;

            if (remainingTime == 0f)
                TimerText.text = string.Empty;
        }

        protected override string FormatTime(float remainingTime)
        {
            return $"{remainingTime:f2}";
        }
    }
}