using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonCrawlers.Systems
{
	public class SceneLoaderSystem : MonoBehaviour
	{
		public void LoadScene(int index) {
			SceneLoader.LoadSceneAsync(index).allowSceneActivation = true;
		}

		public void LoadPreviousScene() {
			int index =
				SceneLoader.CurrentSceneIndex - 1 < 0 ?
				SceneManager.sceneCountInBuildSettings - 1 : SceneLoader.CurrentSceneIndex - 1;
			LoadScene(index);
		}

		public void LoadNextScene() {
			int index =
				SceneLoader.CurrentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings ?
				SceneLoader.CurrentSceneIndex + 1 : 0;
			LoadScene(index);
		}

		public void Reload() {
			SceneLoader.LoadSceneAsync(SceneLoader.CurrentSceneIndex).allowSceneActivation = true;
		}
	}
}
