using System;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public enum RoomType
	{
		basic
	}

	[CreateAssetMenu(fileName="RoomMeta", menuName="DungeonCrawlers/RoomMeta")]
	public class RoomMeta : ScriptableObject
	{
		public RoomType type;
		public new string name;
		public GameObject prefab;
		public GameObject selectablePrefab;
	}
}
