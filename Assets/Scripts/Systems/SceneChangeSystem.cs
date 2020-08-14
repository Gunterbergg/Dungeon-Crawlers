using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class SceneChangeSystem : MonoBehaviour
	{
		private static AsyncOperation currentOperation;

		public SceneLoader.SceneIndex scene = SceneLoader.SceneIndex.MainScene;
		
		public void LoadScene() {
			if (currentOperation != null) return;
			StartCoroutine(SceneLoader.LoadSceneAsync(scene,
				(operation) => currentOperation = operation));
		}

		public void ActivateScene() {
			if (currentOperation == null) return;
			currentOperation.allowSceneActivation = true;
		}
	}
}
