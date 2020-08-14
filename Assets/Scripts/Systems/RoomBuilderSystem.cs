using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class RoomBuilderSystem : MonoBehaviour
	{
		//TODO make child colliders composite
		new public bool enabled = true;
		public RoomTileSetInfo roomTiles;
		public Vector2Int size;

		public Vector3 UpperLeftPos { get; private set; }
		public Vector3 UpperRightPos { get; private set; }
		public Vector3 LowerRightPos { get; private set; }
		public Vector3 LowerLeftPos { get; private set; }
		public Vector3 RoomCenter { get; private set; }

		private Vector2 currentRoomSize;

		protected virtual void Awake() {
			BuildRoom();
		}

		public void BuildRoom() {
			if (!this.enabled) return;

			FindRoomBounds();

			CreateWallSides();
			CreateWallCorners();
			
			CreateFloorSides();
			CreateFloorCorners();

			CreateFloor();
		}

		public void DestroyRoom() {
			currentRoomSize = Vector2.zero;
			Transform tilesParent = gameObject.GetComponent<Transform>();
			foreach (Transform tile in tilesParent) {
				Destroy(tile.gameObject);
			}
		}

		public void RebuildRoom() {
			DestroyRoom();
			BuildRoom();
		}

		private void FindRoomBounds() {
			RoomCenter = transform.position;
			size.x = Mathf.Max(4, this.size.x);
			size.y = Mathf.Max(4, this.size.y);

			UpperLeftPos = new Vector3(RoomCenter.x - size.x / 2, RoomCenter.y + size.y / 2);
			UpperRightPos = new Vector3(RoomCenter.x + size.x / 2, RoomCenter.y + size.y / 2);
			LowerRightPos = new Vector3(RoomCenter.x + size.x / 2, RoomCenter.y - size.y / 2);
			LowerLeftPos = new Vector3(RoomCenter.x - size.x / 2, RoomCenter.y - size.y / 2);
		}

		private void CreateWallSides() {
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
		}

		private void CreateWallCorners() {
			CreateTile(GetRandomTile(roomTiles.walls.upperLeftTiles), UpperLeftPos);
			CreateTile(GetRandomTile(roomTiles.walls.upperRightTiles), UpperRightPos);
			CreateTile(GetRandomTile(roomTiles.walls.lowerRightTiles), LowerRightPos);
			CreateTile(GetRandomTile(roomTiles.walls.lowerLeftTiles), LowerLeftPos);
		}

		private void CreateFloorSides() {
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
		}

		private void CreateFloorCorners() {
			CreateTile(GetRandomTile(roomTiles.wallSidedFloors.upperLeftTiles), UpperLeftPos + Vector3.down + Vector3.right);
			CreateTile(GetRandomTile(roomTiles.wallSidedFloors.upperRightTiles), UpperRightPos + Vector3.down + Vector3.left);
			CreateTile(GetRandomTile(roomTiles.wallSidedFloors.lowerRightTiles), LowerRightPos + Vector3.up + Vector3.left);
			CreateTile(GetRandomTile(roomTiles.wallSidedFloors.lowerLeftTiles), LowerLeftPos + Vector3.up + Vector3.right);
		}

		private void CreateFloor() {
			RectangleFill(new Rect(
				LowerLeftPos + (Vector3.up + Vector3.right) * 2,
				(UpperRightPos + (Vector3.down + Vector3.left) * 2) - (LowerLeftPos + (Vector3.up + Vector3.right) * 2)),
					roomTiles.floors);
		}

		private void RectangleFill(Rect rectangle, List<GameObject> tilePoolList) {
			for (float xPos = rectangle.x; xPos <= rectangle.xMax; xPos++)
				for (float yPos = rectangle.y; yPos <= rectangle.yMax; yPos++)
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
