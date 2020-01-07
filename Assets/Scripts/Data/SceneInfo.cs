using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonCrawlers.Data 
{
	[CreateAssetMenu(fileName = "SceneInfo", menuName = "InfoContainer/SceneInfo")]
	public class SceneInfo : ScriptableObject
	{
		public int SceneIndex;
	}
	
}