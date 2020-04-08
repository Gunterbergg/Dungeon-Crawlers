using System;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "UserData", menuName ="InfoContainer/UserData")]
	public class UserData : ScriptableObject
	{
		//Fields must match JSON response key names
		[SerializeField] protected int user_id;
		[SerializeField] protected new string name;
		[SerializeField] protected string email;
		[SerializeField] protected int souls;
		[SerializeField] protected int gold;
		[SerializeField] protected int gems;
		[SerializeField] protected int level;
		[SerializeField] protected int current_exp;
		[SerializeField] protected int next_level_exp;

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

		new public string ToString() {
			return
				"user_id: " + User_id +
				"\n name: " + Name +
				"\n email: " + Email +
				"\n souls: " + Souls +
				"\n gems: " + Gems;
		}

		public void Copy(UserData other) {
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

		protected void RaiseChangedEvent() {
			OnChanged?.Invoke();
		}

		protected void RaiseChangedEvent(string propertyName) {
			OnPropertyChanged?.Invoke(propertyName);
			RaiseChangedEvent();
		}
	}
}
