using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawlers.UI
{
	public class ProgressBar : UserView, IProgressHandler
	{
		public Image progressImage;
		public float lerpAmount = 0.5f;
		new public bool enabled = true;

		private Coroutine lerpCoroutine;

		public bool Enabled { 
			get => enabled;
			set {
				enabled = value;
				Output(CurrentOutput);
			}
		}
		public float CurrentOutput { get; set; } = 0f;
		public float LerpTime { get => lerpAmount; set => lerpAmount = value; }

		public event Action<float> ValueChanged;
		public event Action Completed;

		protected override void Awake() {
			base.Awake();
			progressImage.type = Image.Type.Filled;
			CurrentOutput = progressImage.fillAmount;
		}

		public virtual void Output(float output, Action callback = null) {
			CurrentOutput = Mathf.Clamp01(output);
			if (!Enabled) return;

			if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
			lerpCoroutine = StartCoroutine(LerpOutput(CurrentOutput, LerpTime, callback));
		}

		public virtual void OutputDefault() => Output(0f);

		public virtual void Clear() => OutputDefault();

		protected void RaiseCompletedEvent() => Completed?.Invoke();
		protected void RaiseValueChangedEvent() => ValueChanged?.Invoke(CurrentOutput);

		protected IEnumerator LerpOutput(float targetValue, float lerpValue, Action callback = null) {
			float elapsedTime = 0f;
			float intialValue = progressImage.fillAmount;
			while (elapsedTime < lerpValue) {
				if (Enabled) {
					progressImage.fillAmount = Mathf.Lerp(intialValue, targetValue, elapsedTime / lerpValue);
					elapsedTime += Time.deltaTime;
				}
				yield return null;
			}
			progressImage.fillAmount = targetValue;

			callback?.Invoke();
			if (targetValue == 1) RaiseCompletedEvent();
			RaiseValueChangedEvent();
		}
	}
}
