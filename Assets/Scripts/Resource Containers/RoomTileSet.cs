using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public struct TileSet
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

	[Serializable]
	[CreateAssetMenu(fileName="RoomTileSet", menuName="DungeonCrawlers/RoomTileSet")]
	public class RoomTileSet : ScriptableObject
	{
		//TODO add property drawer
		public TileSet walls;
		public TileSet wallSidedFloors;
		public List<GameObject> floors;
	}
}
