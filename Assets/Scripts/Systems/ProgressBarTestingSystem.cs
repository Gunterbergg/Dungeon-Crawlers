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
			progressBar.OnValueChanged += (sender, args) => {
				if (args.Data == 0) ((IProgressBar)sender).Output(1f);
				if (args.Data == 1) ((IProgressBar)sender).Output(0f);
			};

			lerpController.onValueChanged.AddListener((newValue) => progressBar.lerptTime = newValue);
			toggleController.onValueChanged.AddListener((newValue) => progressBar.Enabled = newValue);
			progressBar.Clear();
		}
	}
}