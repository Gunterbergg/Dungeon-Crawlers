using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "UserInfo", menuName ="InfoContainer/UserInfo")]
	public class UserInfo : ScriptableObject
	{
		//Fields must match JSON response key names
		public int user_id;
		public new string name;
		public string email;
		public int souls;
		public int gold;
		public int gems;

		public string Password { get; set; }

		public void Copy(UserInfo other) {
			if (other == null) return;
			this.user_id = other.user_id;
			this.name = other.name;
			this.email = other.email;
			this.souls = other.souls;
			this.gold = other.gold;
			this.gems = other.gems;
		}

		new public string ToString() {
			return
				"user_id: " + user_id +
				"\n name: " + name +
				"\n email: " + email +
				"\n souls: " + souls +
				"\n gems: " + gems;
		}
	}
}