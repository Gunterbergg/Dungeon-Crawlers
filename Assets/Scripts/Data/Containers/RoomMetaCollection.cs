using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName="RoomMetaCollection", menuName="DungeonCrawlers/RoomMetaCollection")]
	public class RoomMetaCollection : ScriptableObject
	{
		public RoomMeta defaultMeta;
		public List<RoomMeta> rooms;

		public RoomMeta this[RoomType type] {
			get => rooms.Find((room) => room.type == type) ?? defaultMeta;
		}
	}
}
