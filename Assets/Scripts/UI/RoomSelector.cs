using DungeonCrawlers.Data;
using System;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public class RoomSelector : UserView, IInputHandler<RoomInfo>
	{
		public event EventHandler<EventArgs<RoomInfo>> Input;
		public UserView touchInput;
		public RoomCollectionInfo userRooms;

		private RoomInfo lastSelected = null;

		public bool InputEnabled { get; set; } = true;

		protected override void Awake() {
			base.Awake();
			touchInput.GetInterface<IInputHandler<Touch>>().Input +=
				(sender, args) => OnTouchInput(args.Data);
		}

		protected void OnTouchInput(Touch touch) {
			if (!InputEnabled) return;
			Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
			foreach (RoomInfo room in userRooms) { 
				if (room.OverlapsPoint(touchPos) && room != lastSelected) { 
					lastSelected = room;
					Input?.Invoke(this, new EventArgs<RoomInfo>(room));
					break;
				}
			}
		}
	}
}