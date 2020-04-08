using System;
using DungeonCrawlers.Data;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public class RoomSelector : UserView, IInputHandler<RoomData>
	{
		public event Action<RoomData> Input;
		public UserView touchInput;
		public RoomCollectionData userRooms;

		protected RoomData lastSelected = null;

		public bool InputEnabled { get; set; } = true;

		protected override void Awake() {
			base.Awake();
			touchInput.GetInterface<IInputHandler<Touch>>().Input += OnTouchInput;
		}

		protected void OnTouchInput(Touch touch) {
			if (!InputEnabled) return;
			Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
			foreach (RoomData room in userRooms) { 
				if (room.OverlapsPoint(touchPos) && room != lastSelected) { 
					lastSelected = room;
					Input?.Invoke(room);
					break;
				}
			}
		}
	}
}
