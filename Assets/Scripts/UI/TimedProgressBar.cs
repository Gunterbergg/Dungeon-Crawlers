using System;

namespace DungeonCrawlers.UI
{
	public class TimedProgressBar : UserView, IProgressHandler
	{
		public UserView progressView;

		public bool resetTimer = true;
		public bool inverted = false;

		private IProgressHandler progressHandler;

		public bool Enabled { get => progressHandler.Enabled; set => progressHandler.Enabled = value; }
		public float CurrentOutput { get => progressHandler.CurrentOutput; }
		public float LerpTime { get; set; } = 0f;

		public event Action<float> ValueChanged { 
			add => progressHandler.ValueChanged += value;
			remove => progressHandler.ValueChanged -= value; 
		}

		public event Action Completed;

		protected override void Awake() {
			base.Awake();
			progressHandler = progressView?.GetInterface<IProgressHandler>();
		}

		public void Output(float output, Action callback = null) {
			progressHandler.LerpTime = output;
			float timerValue = inverted ? 0f : 1f;

			progressHandler.Output(timerValue, () => {
				callback?.Invoke();
				Completed?.Invoke();
				if (resetTimer) OutputDefault();
			});
		}

		public void OutputDefault() {
			progressHandler.LerpTime = LerpTime;
			progressHandler.Output(inverted ? 1f : 0f);
		}

		public void Clear() => OutputDefault();
	}
}
