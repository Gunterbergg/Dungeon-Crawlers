using System;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "UserInfo", menuName ="InfoContainer/UserInfo")]
	public class UserInfo : ScriptableObject
	{
		//Fields must match JSON response key names
		[SerializeField] private int user_id;
		[SerializeField] private new string name;
		[SerializeField] private string email;
		[SerializeField] private int souls;
		[SerializeField] private int gold;
		[SerializeField] private int gems;
		[SerializeField] private int level;
		[SerializeField] private int current_exp;
		[SerializeField] private int next_level_exp;

		public string Password { get; set; }

		public int User_id {
			get => user_id; 
			set {
				user_id = value;
				RaiseChangedEvent("user_id");
			}
		}
		public string Name { 
			get => name;
			set {
				name = value;
				RaiseChangedEvent("name");
			}
		}
		public string Email {
			get => email;
			set {
				email = value;
				RaiseChangedEvent("email");
			}
		}
		public int Souls {
			get => souls;
			set {
				souls = value;
				RaiseChangedEvent("souls");
			}
		}
		public int Gold {
			get => gold;
			set {
				gold = value;
				RaiseChangedEvent("gold");
			}
		}
		public int Gems {
			get => gems;
			set {
				gems = value;
				RaiseChangedEvent("gems");
			}
		}
		public int Level {
			get => level;
			set {
				level = value;
				RaiseChangedEvent("level");
			}
		}
		public int Current_exp {
			get => current_exp;
			set {
				current_exp = value;
				RaiseChangedEvent("current_exp");
			}
		}
		public int Next_level_exp {
			get => next_level_exp;
			set {
				next_level_exp = value;
				RaiseChangedEvent("next_level_exp");
			}
		}

		public Action OnChanged;
		public Action<string> OnPropertyChanged;

		public void Copy(UserInfo other) {
			if (other == null) return;
			this.user_id = other.User_id;
			this.name = other.Name;
			this.email = other.Email;
			this.souls = other.Souls;
			this.gold = other.Gold;
			this.gems = other.Gems;
			this.level = other.Level;
			this.current_exp = other.Current_exp;
			this.next_level_exp = other.Next_level_exp;
			RaiseChangedEvent();
		}

		private void RaiseChangedEvent() {
			OnChanged?.Invoke();
		}

		private void RaiseChangedEvent(string propertyName) {
			OnPropertyChanged?.Invoke(propertyName);
			RaiseChangedEvent();
		}

		new public string ToString() {
			return
				"user_id: " + User_id +
				"\n name: " + Name +
				"\n email: " + Email +
				"\n souls: " + Souls +
				"\n gems: " + Gems;
		}
	}
}