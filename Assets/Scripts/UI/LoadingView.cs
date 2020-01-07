using System;
using System.Collections;
using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawlers.UI 
{
	public class LoadingView : UserView, ILoadingView 
	{
		public UserView loadingBar;
		public Text statusText;

		private Coroutine currentCoroutine;
		private List<string> statusMessages;
		private int messageIndex = 0;

		public IProgressBar ProgressBar { get => loadingBar.GetInterface<IProgressBar>(); }

		public void Output(LoadingScreenEventArgs output) {
			ProgressBar.Output(output.Progress);
			statusMessages = new List<string>(output.Status);
			messageIndex = 0;
			NextStatus();
		}

		public void Clear() {
			ProgressBar.Clear();
			statusText.text = LanguagePack.GetString("loading_label");
		}

		public void NextStatus() {
			string nextMessage;
			if (statusMessages.Count > 0) {
				nextMessage = messageIndex + 1 < statusMessages.Count ? statusMessages[++messageIndex] : statusMessages[0];
			} else {
				nextMessage = LanguagePack.GetString("loading_label");
			}
			statusText.text = nextMessage;
		}

		public void ListenTo(Func<float> function) {
			if (currentCoroutine != null) 
				StopCoroutine(currentCoroutine);
			IEnumerator newProcess = ListenFunction(function);
			currentCoroutine = StartCoroutine(newProcess);
		}

		protected IEnumerator ListenFunction(Func<float> function) {
			float result;
			do {
				result = function.Invoke();
				ProgressBar.Output(result);
				yield return null;
			} while (result < 1);
		}
	}
}