using UnityEngine;
using UnityEngine.UI;
using DungeonCrawlers.UI;
using System.Collections;


public class ProgressBarTestingSystem : MonoBehaviour
{
	public ProgressBar progressBar;
	public Slider lerpController;

	public void Start() {
		StartCoroutine(Reload());
		progressBar.OnCompleted += (sernder, args) => StartCoroutine(Reload());
		lerpController.onValueChanged.AddListener((lerp) => progressBar.lerptTime = lerp);
	}

	private IEnumerator Reload() {
		progressBar.Clear();
		yield return new WaitForSeconds(progressBar.lerptTime);
		progressBar.Output(1f);
	}

}
