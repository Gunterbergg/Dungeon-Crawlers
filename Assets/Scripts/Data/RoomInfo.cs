using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "RoomInfo", menuName = "InfoContainer/RoomInfo")]
	public class RoomInfo : ScriptableObject
	{
		public string type;
		public GameObject roomObject;
		public Vector2Int roomSize;
	}
}