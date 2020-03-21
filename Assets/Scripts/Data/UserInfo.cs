using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "UserInfo", menuName ="InfoContainer/UserInfo")]
	public class UserInfo : ScriptableObject
	{
		public uint id;
		public new string name;
		public string email;
		public int souls;
		public int gold;
		public int gems;
	}
}