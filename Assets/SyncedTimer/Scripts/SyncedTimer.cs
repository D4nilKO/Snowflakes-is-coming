using System;
using UnityEngine;

namespace VavilichevGD.Utils.Timing
{
	public delegate void TimerValueChangedHandler(float remainingSeconds, TimeChangingSource changingSource);

	public class SyncedTimer
	{
		public event TimerValueChangedHandler TimerValueChanged;
		public event Action TimerFinished;

		public TimerType Type { get; }
		public bool IsActive { get; private set; }
		public bool IsPaused { get; private set; }
		public float RemainingSeconds { get; private set; }

		public SyncedTimer(TimerType type)
		{
			this.Type = type;
		}

		public SyncedTimer(TimerType type, float seconds)
		{
			this.Type = type;

			SetTime(seconds);
		}

		public void SetTime(float seconds)
		{
			RemainingSeconds = seconds;
			TimerValueChanged?.Invoke(RemainingSeconds, TimeChangingSource.TimeForceChanged);
		}

		public void Start()
		{
			if (IsActive)
				return;

			if (Math.Abs(RemainingSeconds) < Mathf.Epsilon)
			{
#if DEBUG
				Debug.LogError("TIMER: You are trying start timer with remaining seconds equal 0.");
#endif
				//TimerFinished?.Invoke();
				return;
			}

			IsActive = true;
			IsPaused = false;
			SubscribeOnTimeInvokerEvents();

			TimerValueChanged?.Invoke(RemainingSeconds, TimeChangingSource.TimerStarted);
		}

		public void Start(float seconds)
		{
			if (IsActive)
				return;

			SetTime(seconds);
			Start();
		}

		public void Pause()
		{
			if (IsPaused || !IsActive)
				return;

			IsPaused = true;
			UnsubscribeFromTimeInvokerEvents();

			TimerValueChanged?.Invoke(RemainingSeconds, TimeChangingSource.TimerPaused);
		}

		public void Unpause()
		{
			if (!IsPaused || !IsActive)
				return;

			IsPaused = false;
			SubscribeOnTimeInvokerEvents();

			TimerValueChanged?.Invoke(RemainingSeconds, TimeChangingSource.TimerUnpaused);
		}

		public void Stop()
		{
			if (IsActive)
			{
				UnsubscribeFromTimeInvokerEvents();
				
				RemainingSeconds = 0f;
				IsActive = false;
				IsPaused = false;

				TimerValueChanged?.Invoke(RemainingSeconds, TimeChangingSource.TimerFinished);
				TimerFinished?.Invoke();
			}
		}

		public void ForceStop()
		{
			if (IsActive)
			{
				UnsubscribeFromTimeInvokerEvents();
				
				RemainingSeconds = 0f;
				IsActive = false;
				IsPaused = false;

				TimerValueChanged?.Invoke(RemainingSeconds, TimeChangingSource.TimerFinished);
			}
		}

		private void SubscribeOnTimeInvokerEvents()
		{
			switch (Type)
			{
				case TimerType.UpdateTick:
					TimeInvoker.instance.OnUpdateTimeTickedEvent += OnTicked;
					break;
				case TimerType.UpdateTickUnscaled:
					TimeInvoker.instance.OnUpdateTimeUnscaledTickedEvent += OnTicked;
					break;
				case TimerType.OneSecTick:
					TimeInvoker.instance.OnOneSyncedSecondTickedEvent += OnSyncedSecondTicked;
					break;
				case TimerType.OneSecTickUnscaled:
					TimeInvoker.instance.OnOneSyncedSecondUnscaledTickedEvent += OnSyncedSecondTicked;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void UnsubscribeFromTimeInvokerEvents()
		{
			switch (Type)
			{
				case TimerType.UpdateTick:
					TimeInvoker.instance.OnUpdateTimeTickedEvent -= OnTicked;
					break;
				case TimerType.UpdateTickUnscaled:
					TimeInvoker.instance.OnUpdateTimeUnscaledTickedEvent -= OnTicked;
					break;
				case TimerType.OneSecTick:
					TimeInvoker.instance.OnOneSyncedSecondTickedEvent -= OnSyncedSecondTicked;
					break;
				case TimerType.OneSecTickUnscaled:
					TimeInvoker.instance.OnOneSyncedSecondUnscaledTickedEvent -= OnSyncedSecondTicked;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void CheckFinish()
		{
			if (RemainingSeconds <= 0f)
			{
				Stop();
			}
		}

		private void NotifyAboutTimePassed()
		{
			if (RemainingSeconds >= 0f)
			{
				TimerValueChanged?.Invoke(RemainingSeconds, TimeChangingSource.TimePassed);
			}
		}

		private void OnTicked(float deltaTime)
		{
			RemainingSeconds -= deltaTime;
			
			NotifyAboutTimePassed();
			CheckFinish();
		}

		private void OnSyncedSecondTicked()
		{
			RemainingSeconds -= 1;
			
			NotifyAboutTimePassed();
			CheckFinish();
		}
	}
}