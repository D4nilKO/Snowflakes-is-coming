using Project.GameState;
using VavilichevGD.Utils.Timing;
using VContainer;

namespace Project.Timing
{
    public class SurviveTimerView : TimerView
    {
        private GameOutcome _gameOutcome;
        
        [Inject]
        public void Construct(GameOutcome gameOutcome)
        {
            _gameOutcome = gameOutcome;
        }

        protected override void Awake()
        {
            base.Awake();

            _gameOutcome.SurviveTimer.TimerValueChanged += UpdateText;
        }

        private void OnDestroy()
        {
            _gameOutcome.SurviveTimer.TimerValueChanged -= UpdateText;
        }

        public override void Initialize(float timeToSurvive)
        {
            //
        }

        protected override void UpdateText(float remainingTime, TimeChangingSource changingSource)
        {
            TimerText.text = FormatTime(remainingTime);
        }

        protected override string FormatTime(float remainingTime)
        {
            int secondsInMinute = 60;

            int minutes = (int)(remainingTime / secondsInMinute);
            int seconds = (int)(remainingTime % secondsInMinute);

            return $"{minutes:00}:{seconds:00}";
        }
    }
}