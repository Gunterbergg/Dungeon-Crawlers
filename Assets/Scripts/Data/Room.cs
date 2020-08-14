using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	/*

	[Serializable]
	public class Room
	{
		[SerializeField] private RoomType type;
		[SerializeField] private List<Entity> entities;

		public RoomType Type { 
			get => type;
			set {
				type = value;
				RaiseChangedEvent("Type");
			}
		}
		public List<Entity> Entities { 
			get => entities;
			set {
				entities = value;
				RaiseChangedEvent("Entities");
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
	}*/
}
