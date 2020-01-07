using UnityEngine.SceneManagement;
using UnityEngine;

namespace DungeonCrawlers
{
	public static class SceneLoader
	{
		public static int CurrentSceneIndex { get => SceneManager.GetActiveScene().buildIndex; }

		public static AsyncOperation LoadSceneAsync(int buildIndex) {
			Logger.Register("Started async scene loading of buildIndex " + buildIndex, "SCENE_LOADER");
			AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(buildIndex);
			loadingOperation.allowSceneActivation = false;

			loadingOperation.completed += (sceneLoader) => 
				Logger.Register("Completed scene of buildIndex " + buildIndex + " loading", "SCENE_LOADER");

			return loadingOperation;
		}
	}
}