using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "RoomCollectionInfo", menuName = "InfoContainer/RoomCollectionInfo")]
	public class RoomCollectionInfo : ScriptableObject
	{
		[SerializeField]
		private List<RoomInfo> collection = new List<RoomInfo>();

		public event EventHandler<EventArgs<RoomInfo>> OnValueChanged;
		public Dictionary<int, RoomInfo> activeCollection = new Dictionary<int, RoomInfo>();

		public RoomInfo this [int roomIndex] {
			get => activeCollection[roomIndex];
			set {
				if (activeCollection.ContainsKey(roomIndex)) {
					Destroy(activeCollection[roomIndex].roomObject);
					activeCollection[roomIndex] = value;
				} else {
					activeCollection.Add(roomIndex, value);
				}
				OnValueChanged?.Invoke(this, new EventArgs<RoomInfo>(value));
			}
		}

		public RoomInfo this[string roomType] {
			get {
				foreach (RoomInfo room in activeCollection.Values)
					if (room.type == roomType)
						return room;
				return null;
			}
		}

		public int GetRoomIndex(RoomInfo roomInfo) {
			foreach (KeyValuePair<int, RoomInfo> a in activeCollection)
				if (a.Value == roomInfo)
					return a.Key;
			throw new Exception();
		}

		public int Count { get => activeCollection.Count; }

		private void OnEnable() {
			for (int i = 0; i < collection.Count; i++)
				activeCollection[i] = collection[i];
		}
	}
}