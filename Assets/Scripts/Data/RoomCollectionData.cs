using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Leguar.TotalJSON;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "RoomCollectionData", menuName = "InfoContainer/RoomCollectionData")]
	public class RoomCollectionData : ScriptableObject, IEnumerable 
	{
		protected List<RoomData> collection = new List<RoomData>();
		protected JSON loadedRooms;

		public event Action<RoomData> OnValueChanged;
		public event Action<JSON> OnLoadedRoomSet;

		public JSON LoadedRooms {
			get => loadedRooms;
			set {
				loadedRooms = value;
				OnLoadedRoomSet?.Invoke(value);
				collection.Clear();
			}
		}

		public RoomData this [int roomIndex] {
			get => collection[roomIndex];
			set {
				if (collection.Count <= roomIndex) collection.Add(value);
				else collection.Insert(roomIndex, value);
				OnValueChanged?.Invoke(value);
			}
		}

		public RoomData this[string roomType] {
			get => collection.Find((room) => room.type == roomType);
		}

		public int Count { get => collection.Count; }

		protected void OnEnable() {
			SceneManager.activeSceneChanged += (init, target) => {
				List<RoomData> newCollection = new List<RoomData>();
				foreach (RoomData data in collection)
					if (data.RoomObject != null) newCollection.Add(data);
				collection = newCollection;
			};
		}

		public IEnumerator GetEnumerator() {
			return ((IEnumerable<RoomData>)collection).GetEnumerator();
		}

		public void Add(RoomData room) {
			collection.Add(room);
			OnValueChanged?.Invoke(room);
		}

		public void SwapOrder(RoomData target1, RoomData target2) { 
			//TODO implement swap and swap event
		}
	}
}
