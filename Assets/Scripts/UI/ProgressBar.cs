using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawlers.UI
{
	public class ProgressBar : UserView, IProgressBar
	{
		public Image progressImage;
		public float lerptTime = 0.5f;

		private float progress = 0;
		private Coroutine lerpCoroutine;

		public bool Enabled { get; set; } = true;
		public float Progress {
			get => progress;
			set => Output(value);
		}
		public float CurrentOutput { get; private set; }

		public event Action<float> OnValueChanged;

		protected override void Awake() {
			base.Awake();
			progressImage.type = Image.Type.Filled;
		}

		public void Clear() {
			Output(0f);
		}

		public void Output(float output) {
			if (!Enabled) return; 
			progress = Mathf.Clamp01(output);
			CurrentOutput = progress;
			if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
			lerpCoroutine = StartCoroutine(LerpOutput());
		}

		public void OutputDefault() {
			Clear();
		}

		protected IEnumerator LerpOutput() {
			float elapsedTime = 0f;
			float intialValue = progressImage.fillAmount;
			float lerpTime = this.lerptTime;
			while (elapsedTime < lerptTime) {
				if (Enabled) {
					progressImage.fillAmount = Mathf.Lerp(intialValue, progress, elapsedTime / lerptTime);
					elapsedTime += Time.deltaTime;
				}
				yield return null;
			}

			OnValueChanged?.Invoke(progress);
		}
	}
}