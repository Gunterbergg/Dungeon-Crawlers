using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Leguar.TotalJSON;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "RoomCollectionInfo", menuName = "InfoContainer/RoomCollectionInfo")]
	public class RoomCollectionInfo : ScriptableObject, IEnumerable 
	{
		[SerializeField]
		private List<RoomInfo> initialCollection = new List<RoomInfo>();
		private List<RoomInfo> collection = new List<RoomInfo>();
		private JSON loadedRooms;

		public event Action<RoomInfo> OnValueChanged;
		public event Action<JSON> OnLoadedRoomSet;

		public JSON LoadedRooms {
			get => loadedRooms;
			set {
				loadedRooms = value;
				OnLoadedRoomSet?.Invoke(value);
				collection.Clear();
			}
		}

		public RoomInfo this [int roomIndex] {
			get => collection[roomIndex];
			set {
				if (collection.Count <= roomIndex) collection.Add(value);
				else collection.Insert(roomIndex, value);
				OnValueChanged?.Invoke(value);
			}
		}

		public RoomInfo this[string roomType] {
			get => collection.Find((room) => room.type == roomType);
		}

		public int Count { get => collection.Count; }

		private void OnEnable() {
			collection = new List<RoomInfo>(initialCollection);
			SceneManager.activeSceneChanged += (init, target) => {
				List<RoomInfo> newCollection = new List<RoomInfo>();
				foreach (RoomInfo data in collection)
					if (data.RoomObject != null) newCollection.Add(data);
				collection = newCollection;
			};
		}

		public IEnumerator GetEnumerator() {
			return ((IEnumerable<RoomInfo>)collection).GetEnumerator();
		}

		public void Add(RoomInfo room) {
			collection.Add(room);
			OnValueChanged?.Invoke(room);
		}

		public void SwapOrder(RoomInfo target1, RoomInfo target2) { 
			//TODO implement swap and swap event
		}

	}
}