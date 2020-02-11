using UnityEngine;
using DungeonCrawlers;

public class SceneLoaderSystem : MonoBehaviour
{
	public void LoadScene(int index) {
		SceneLoader.LoadSceneAsync(index).allowSceneActivation = true;
	}

	public void Reload() {
		SceneLoader.LoadSceneAsync(SceneLoader.CurrentSceneIndex).allowSceneActivation = true;
	}
}
