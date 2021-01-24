using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public struct EntityInfo
	{
		public EntityType type;
		public int level;
	}

	[Serializable]
	public class User
	{
		[SerializeField] private int userId;
		[SerializeField] private string name;
		[SerializeField] private string chosenGod;
		[SerializeField] private int souls;
		[SerializeField] private int gold;
		[SerializeField] private int gems;
		[SerializeField] private int currentExp;
		[SerializeField] private int level;
		[SerializeField] private int nextLevelExp;
		[SerializeField] private RoomType roomType;
		[SerializeField] private List<Entity> entities;
		[SerializeField] private List<EntityInfo> entitiesInfo = new List<EntityInfo>();
		
		public Dictionary<string, WebRequest> links = new Dictionary<string, WebRequest>();

		public int UserId {
			get => userId; 
			set {
				userId = value;
				RaiseChangedEvent("UserId");
			}
		}
		public string Name { 
			get => name;
			set {
				name = value;
				RaiseChangedEvent("Name");
			}
		}
		public int Souls {
			get => souls;
			set {
				souls = value;
				RaiseChangedEvent("Souls");
			}
		}
		public int Gold {
			get => gold;
			set {
				gold = value;
				RaiseChangedEvent("Gold");
			}
		}
		public int Gems {
			get => gems;
			set {
				gems = value;
				RaiseChangedEvent("Gems");
			}
		}
		public int Level {
			get => level;
			set {
				level = value;
				RaiseChangedEvent("Level");
			}
		}
		public int CurrentExp {
			get => currentExp;
			set {
				currentExp = value;
				RaiseChangedEvent("CurrentExp");
			}
		}
		public int NextLevelExp {
			get => nextLevelExp;
			set {
				nextLevelExp = value;
				RaiseChangedEvent("NextLevelExp");
			}
		}
		public RoomType RoomType {
			get => roomType;
			set {
				roomType = value;
				RaiseChangedEvent("RoomType");
			}
		}
		public List<Entity> Entities {
			get => entities;
			set {
				entities = value;
				RaiseChangedEvent("Entities");
			}
		}
		public List<EntityInfo> EntitiesInfo {
			get => entitiesInfo;
			set {
				entitiesInfo = value;
				RaiseChangedEvent("EntitiesInfo");
			}
		}

		public Action OnChanged;
		public Action<string> OnPropertyChanged;

		private void RaiseChangedEvent() {
			OnChanged?.Invoke();
		}

		private void RaiseChangedEvent(string propertyName) {
			OnPropertyChanged?.Invoke(propertyName);
			RaiseChangedEvent();
		}
	}
}
