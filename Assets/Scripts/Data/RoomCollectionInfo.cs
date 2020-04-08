using System;
using System.Collections;
using System.Collections.Generic;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public struct RoomCollectionInfo : IEnumerable
	{
		//TODO add property drawer
		public List<RoomData> collection;

		public RoomData this[string roomType] {
			get => collection.Find((room) => room.type == roomType);
		}

		public int Count { get => collection.Count; }

		public IEnumerator GetEnumerator() {
			return collection.GetEnumerator();
		}
	}
}
