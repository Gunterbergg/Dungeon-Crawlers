using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "RoomCollectionInfo", menuName = "InfoContainer/RoomCollectionInfo")]
	public class RoomCollectionInfo : ScriptableObject, IEnumerable
	{
		[SerializeField]
		private List<RoomInfo> initialCollection = new List<RoomInfo>();
		private List<RoomInfo> collection = new List<RoomInfo>();

		public event EventHandler<EventArgs<RoomInfo>> OnValueChanged;

		public RoomInfo this [int roomIndex] {
			get => collection[roomIndex];
			set {
				if (collection.Count <= roomIndex) collection.Add(value);
				else collection.Insert(roomIndex, value);
				OnValueChanged?.Invoke(this, new EventArgs<RoomInfo>(value));
			}
		}

		public RoomInfo this[string roomType] {
			get => collection.Find((room) => room.type == roomType);
		}

		public int Count { get => collection.Count; }
	
		private void OnEnable() => collection = new List<RoomInfo>(initialCollection);
		
		public IEnumerator GetEnumerator() {
			return ((IEnumerable<RoomInfo>)collection).GetEnumerator();
		}

		public void Add(RoomInfo room) {
			collection.Add(room);
			OnValueChanged?.Invoke(this, new EventArgs<RoomInfo>(room));
		}

		public void SwapOrder(RoomInfo target1, RoomInfo target2) { 
			//TODO implement swap and swap event
		}

	}
}