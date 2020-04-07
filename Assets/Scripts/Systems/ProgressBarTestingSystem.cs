using UnityEngine;
using UnityEngine.UI;
using DungeonCrawlers.UI;

namespace DungeonCrawlers.Systems
{
	public class ProgressBarTestingSystem : MonoBehaviour
	{
		public ProgressBar progressBar;
		public Slider lerpController;
		public Toggle toggleController;

		public void Start() {
			progressBar.OnValueChanged += (progress) => {
				IProgressBar progressHandler = progressBar.GetInterface<IProgressBar>();
				if (progress == 0) progressHandler.Output(1f);
				if (progress == 1) progressHandler.Output(0f);
			};

			lerpController.onValueChanged.AddListener((newValue) => progressBar.lerptTime = newValue);
			toggleController.onValueChanged.AddListener((newValue) => progressBar.Enabled = newValue);
			progressBar.Clear();
		}
	}
}