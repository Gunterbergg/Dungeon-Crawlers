using System;

namespace DungeonCrawlers.UI
{
	public class HealthBar : UserView, IProgressHandler
	{
		public UserView mainBarView;
		public UserView secondaryBarView;

		private IProgressHandler mainHandler;
		private IProgressHandler secondaryHandler;

		public float lerpOffset = 1f;

		public bool Enabled {
			get => mainHandler.Enabled;
			set {
				mainHandler.Enabled = value;
				secondaryHandler.Enabled = value;
			}
		}

		public float LerpTime { 
			get => mainHandler.LerpTime;
			set {
				mainHandler.LerpTime = value;
				secondaryHandler.LerpTime = value + lerpOffset;
			}
		}

		public float CurrentOutput { get => mainHandler.CurrentOutput; }

		public event Action<float> ValueChanged { 
			add { secondaryHandler.ValueChanged += value; }
			remove { secondaryHandler.ValueChanged -= value; }
		}
		public event Action Completed {
			add { mainHandler.Completed += value; }
			remove { mainHandler.Completed -= value; }
		}

		protected override void Awake() {
			base.Awake();
			HandlersReferenceSetup();
		}

		public void Output(float output, Action callback = null) {
			mainHandler.Output(output);
			secondaryHandler.Output(output, callback);
		}

		public void OutputDefault() => Output(1f);
		public void Clear() => OutputDefault();

		private void HandlersReferenceSetup() {
			mainHandler = mainBarView?.GetInterface<IProgressHandler>();
			secondaryHandler = secondaryBarView?.GetInterface<IProgressHandler>();
		}
	}
}
