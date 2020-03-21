using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public struct RoomTiles
	{
		public List<GameObject> upperLeftTiles;
		public List<GameObject> upperRightTiles;
		public List<GameObject> lowerRightTiles;
		public List<GameObject> lowerLeftTiles;
		public List<GameObject> leftTiles;
		public List<GameObject> upperTiles;
		public List<GameObject> rightTiles;
		public List<GameObject> lowerTiles;
	}

	[CreateAssetMenu(fileName = "RoomTileInfo", menuName = "InfoContainer/RoomTileInfo")]
	public class RoomTileInfo : ScriptableObject
	{
		public RoomTiles walls;
		public RoomTiles wallSidedFloors;
		public List<GameObject> floors;
	}
}