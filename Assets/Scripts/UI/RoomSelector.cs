using DungeonCrawlers.Data;
using System;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public class RoomSelector : UserView, IUserInput<EventArgs<RoomInfo>>
	{
		public event EventHandler<EventArgs<RoomInfo>> UserInput;
		public UserView touchInput;
		public RoomCollectionInfo userRooms;

		private RoomInfo lastSelected = null;

		protected override void Awake() {
			base.Awake();
			touchInput.GetInterface<IUserInput<EventArgs<Touch>>>().UserInput +=
				(sender, args) => OnTouchInput(args.Data);
		}

		protected void OnTouchInput(Touch touch) {
			Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
			foreach (RoomInfo room in userRooms) { 
				if (room.OverlapsPoint(touchPos) && room != lastSelected) { 
					lastSelected = room;
					UserInput?.Invoke(this, new EventArgs<RoomInfo>(room));
					break;
				}
			}
		}
	}
}