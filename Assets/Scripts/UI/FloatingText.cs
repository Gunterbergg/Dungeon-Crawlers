using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace DungeonCrawlers.UI
{
	public class FloatingText : UserView, IOutputHandler<string>, IOutputHandler<float>
	{
		public string CurrentOutput { get; protected set; }
		float IOutputHandler<float>.CurrentOutput { get; }

		public UserView floatingTextPrefab;
		public float floatingTime = 1f;
		public Vector2 floatingSpeed = Vector2.up;
		public float countResetTime = 1f;

		protected List<UserView> floatingTexts = new List<UserView>();
		protected float currentCount;

		private float lastReset;

		protected override void Awake() {
			base.Awake();
			PrefabSetup();
		}

		public void Clear() {
			foreach (UserView floatingText in floatingTexts)
				Destroy(floatingText);
		}

		public void Output(string output, Action callback = null) {
			CurrentOutput = output;
			StartCoroutine(CreateFloatingText(output));
			callback?.Invoke();
		}

		public void Output(float output, Action callback = null) {
			currentCount = Time.time <= lastReset + countResetTime ? currentCount + output : output;
			lastReset = Time.time;
			Output(currentCount.ToString(), callback);
		}

		public void OutputDefault() => Output("Floating text");
	
		private IEnumerator CreateFloatingText(string text) {
			GameObject floatingText = Instantiate(floatingTextPrefab.gameObject, transform);
				
			floatingText.GetComponent<UserView>()
				.GetInterface<IOutputHandler<string>>().Output(text);

			float elapsedTime = 0f;
			while (elapsedTime < floatingTime) {
				floatingText.transform.Translate(floatingSpeed * Time.deltaTime);
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			Destroy(floatingText);
		}

		private void PrefabSetup() {
			//TODO add logging and exception handling
			if (floatingTextPrefab == null) return;
			if (!(floatingTextPrefab is IOutputHandler<string>)) return;
		}
	}
}
