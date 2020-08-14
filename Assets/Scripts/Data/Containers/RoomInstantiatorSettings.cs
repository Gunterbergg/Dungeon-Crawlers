using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data 
{
	/*
	[CreateAssetMenu(fileName = "RoomInstantiatorSettings", menuName = "DungeonCrawlers/RoomInstantiatorSettings")]
	public class RoomInstantiatorSettings : ScriptableObject
	{
		[Serializable]
		public class RoomPrefab : Prefab
		{
			public RoomType type;
			public GameObject gameObject;

			public override GameObject GetPrefab() => gameObject;
		}

		[Serializable]
		public class RoomPrefabCollection : PrefabCollection
		{
			public GameObject defaultRoom;

			public List<RoomPrefab> prefabs;
			public override IEnumerable<Prefab> getCollection() => prefabs;
			public override GameObject getDefaultObject() => defaultRoom;

			public GameObject this[RoomType type] {
				get => findPrefabObject<RoomPrefab>((roomPrefab) => roomPrefab.type == type);
			}
		}

		public Vector3 roomGap;
		public Vector3 direction;
		public Vector3 offset;
		public RoomPrefabCollection roomPrefabs;
	}*/
}
