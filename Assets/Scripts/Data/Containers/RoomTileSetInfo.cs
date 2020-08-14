using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public struct DirectionalTileSetInfo
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
	public class RoomTileSetInfo : ScriptableObject
	{
		//TODO add property drawer
		public DirectionalTileSetInfo walls;
		public DirectionalTileSetInfo wallSidedFloors;
		public List<GameObject> floors;
	}
}
