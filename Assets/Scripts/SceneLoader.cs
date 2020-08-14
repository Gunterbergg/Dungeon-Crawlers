using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

namespace DungeonCrawlers
{
	public static class SceneLoader
	{
		[Serializable]
		public enum SceneIndex
		{
			AuthScene = 0, MainScene = 1, CombatScene = 2
		}

		public static int CurrentSceneIndex { get => SceneManager.GetActiveScene().buildIndex; }

		public static IEnumerator LoadSceneAsync(SceneIndex sceneIndex, Action<AsyncOperation> callback = null) =>
			LoadSceneAsync((int)sceneIndex, callback);

		public static IEnumerator LoadSceneAsync(int buildIndex, Action<AsyncOperation> callback = null) {
			yield return null;
			Logger.Register("Started async scene loading of buildIndex " + buildIndex, "SCENE_LOADER");
			AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
			loadingOperation.allowSceneActivation = false;

			loadingOperation.completed += (sceneLoader) => 
				Logger.Register("Completed scene of buildIndex " + buildIndex + " loading", "SCENE_LOADER");

			callback?.Invoke(loadingOperation);
		}
	}
}
