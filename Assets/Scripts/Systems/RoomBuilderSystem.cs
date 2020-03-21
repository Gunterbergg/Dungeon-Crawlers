using System.Collections.Generic;
using UnityEngine;
using DungeonCrawlers.Data;

namespace DungeonCrawlers.Systems
{
	public class RoomBuilderSystem : MonoBehaviour
	{
		public RoomTileInfo roomTiles;
		public Vector2 roomSize;

		private Vector3 roomCenter;
		private float offset = 0.5f;

		public Vector3 UpperLeftPos { get; private set; }
		public Vector3 UpperRightPos { get; private set; }
		public Vector3 LowerRightPos { get; private set; }
		public Vector3 LowerLeftPos { get; private set; }

		private void Awake() {
			roomSize = new Vector2(Mathf.Max(4, roomSize.x), Mathf.Max(4, roomSize.y));
			roomCenter = transform.position;

			UpperLeftPos = new Vector3(roomCenter.x - roomSize.x / 2 - offset, roomCenter.y + roomSize.y / 2 + offset);
			UpperRightPos = new Vector3(roomCenter.x + roomSize.x / 2 + offset, roomCenter.y + roomSize.y / 2 + offset);
			LowerRightPos = new Vector3(roomCenter.x + roomSize.x / 2 + offset, roomCenter.y - roomSize.y / 2 - offset);
			LowerLeftPos = new Vector3(roomCenter.x - roomSize.x / 2 - offset, roomCenter.y - roomSize.y / 2 - offset);

			BuildRoom();
		}

		public void BuildRoom() {

			CreateTile(GetRandomTile(roomTiles.walls.upperLeftTiles), UpperLeftPos);
			CreateTile(GetRandomTile(roomTiles.walls.upperRightTiles), UpperRightPos);
			CreateTile(GetRandomTile(roomTiles.walls.lowerRightTiles), LowerRightPos);
			CreateTile(GetRandomTile(roomTiles.walls.lowerLeftTiles), LowerLeftPos);

			RectangleFill(
				new Rect(LowerLeftPos + Vector3.up, (UpperLeftPos + Vector3.down) - (LowerLeftPos + Vector3.up)),
				roomTiles.walls.leftTiles);
			RectangleFill(
				new Rect(UpperLeftPos + Vector3.right, (UpperRightPos + Vector3.left) - (UpperLeftPos + Vector3.right)),
				roomTiles.walls.upperTiles);
			RectangleFill(
				new Rect(LowerRightPos + Vector3.up, (UpperRightPos + Vector3.down) - (LowerRightPos + Vector3.up)),
				roomTiles.walls.rightTiles);
			RectangleFill(
				new Rect(LowerLeftPos + Vector3.right, (LowerRightPos + Vector3.left) - (LowerLeftPos + Vector3.right)),
				roomTiles.walls.lowerTiles);

			CreateTile(GetRandomTile(roomTiles.wallSidedFloors.upperLeftTiles), UpperLeftPos + Vector3.down + Vector3.right);
			CreateTile(GetRandomTile(roomTiles.wallSidedFloors.upperRightTiles), UpperRightPos + Vector3.down + Vector3.left);
			CreateTile(GetRandomTile(roomTiles.wallSidedFloors.lowerRightTiles), LowerRightPos + Vector3.up + Vector3.left);
			CreateTile(GetRandomTile(roomTiles.wallSidedFloors.lowerLeftTiles), LowerLeftPos + Vector3.up + Vector3.right);
			
			RectangleFill(
				new Rect(LowerLeftPos + Vector3.up * 2 + Vector3.right,
						(UpperLeftPos + Vector3.down * 2 + Vector3.right) - (LowerLeftPos + Vector3.up * 2 + Vector3.right)),
				roomTiles.wallSidedFloors.leftTiles);
			RectangleFill(
				new Rect(UpperLeftPos + Vector3.right * 2 + Vector3.down,
						(UpperRightPos + Vector3.left * 2 + Vector3.down) - (UpperLeftPos + Vector3.right * 2 + Vector3.down)),
				roomTiles.wallSidedFloors.upperTiles);
			RectangleFill(
				new Rect(LowerRightPos + Vector3.up * 2 + Vector3.left,
						(UpperRightPos + Vector3.down * 2 + Vector3.left) - (LowerRightPos + Vector3.up * 2 + Vector3.left)),
				roomTiles.wallSidedFloors.rightTiles);
			RectangleFill(
				new Rect(LowerLeftPos + Vector3.right * 2 + Vector3.up,
						(LowerRightPos + Vector3.left * 2 + Vector3.up) - (LowerLeftPos + Vector3.right * 2 + Vector3.up)),
				roomTiles.wallSidedFloors.lowerTiles);

			RectangleFill(new Rect(
				LowerLeftPos + (Vector3.up + Vector3.right) * 2,
				(UpperRightPos + (Vector3.down + Vector3.left) * 2) - (LowerLeftPos + (Vector3.up + Vector3.right) * 2)),
					roomTiles.floors);
		}

		private void RectangleFill(Rect rectangle, List<GameObject> tilePoolList) {
			for (float xPos = rectangle.x; xPos <= (rectangle.x + rectangle.width); xPos++)
				for (float yPos = rectangle.y; yPos <= (rectangle.y + rectangle.height); yPos++)
					CreateTile(GetRandomTile(tilePoolList), new Vector3(xPos, yPos));
		}

		private GameObject GetRandomTile(List<GameObject> tiles) {
			return tiles[Random.Range(0, tiles.Count)];
		}

		private void CreateTile(GameObject tile, Vector3 pos) {
			Instantiate(tile, pos, Quaternion.identity, transform);
		}
	}
}
