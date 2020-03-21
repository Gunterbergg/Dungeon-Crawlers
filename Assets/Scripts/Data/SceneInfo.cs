using UnityEngine;

namespace DungeonCrawlers.Data 
{
	[CreateAssetMenu(fileName = "SceneInfo", menuName = "InfoContainer/SceneInfo")]
	public class SceneInfo : ScriptableObject
	{
		public int SceneIndex;
	}
	
}