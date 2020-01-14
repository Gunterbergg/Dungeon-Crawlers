using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawlers.UI
{
	public class ProgressBar : UserView, IProgressBar
	{
		public Image fillImage;
		public float lerptTime = 0.3f;

		private float progress = 0;

		public bool Enabled { get; set; } = true;
		public float Progress {
			get => progress;
			set {
				progress = value;
				Output(progress);
			}
		}

		public event EventHandler OnCompleted;

		protected override void Awake() {
			base.Awake();
			fillImage.type = Image.Type.Filled;
		}

		public void Clear() {
			Output(0f);
		}

		public void Output(float output) {
			if (Enabled) { 
				progress = Mathf.Clamp(output, 0f, 1f);
				StopAllCoroutines();
				StartCoroutine("LerpOutput");
			}
		}

		public void OutputDefault() {
			Output(0f);
		}

		protected IEnumerator LerpOutput() {
			float elapsedTime = 0f;
			float intialValue = fillImage.fillAmount;
			while (elapsedTime < lerptTime) {
				fillImage.fillAmount = Mathf.Lerp(intialValue, progress, elapsedTime/lerptTime);
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			if (progress >= 1)
				OnCompleted?.Invoke(this, EventArgs.Empty);
		}
	}
}