using System;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public enum EntityType
	{
		slime,
		skeleton,
		golem
	}

	[Serializable]
	public class Entity
	{
		[SerializeField] private EntityType type;
		[SerializeField] private int xPos;
		[SerializeField] private int yPos;

		public EntityType Type {
			get => type;
			set {
				type = value;
				RaiseChangedEvent("type");
			}
		}
		public int XPos {
			get => xPos;
			set {
				xPos = value;
				RaiseChangedEvent("xPos");
			}
		}
		public int YPos {
			get => yPos;
			set {
				yPos = value;
				RaiseChangedEvent("yPos");
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
