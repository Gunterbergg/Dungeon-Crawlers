using DungeonCrawlers.Data;
using UnityEngine;
using Leguar.TotalJSON;

namespace DungeonCrawlers.Systems
{
	public class UserRoomsBuilderSystem : MonoBehaviour
	{
		public Vector3 roomGap = Vector3.right * 5;
		public Vector3 direction = Vector3.right;

		public RoomCollectionInfo roomPrefabs;
		public RoomCollectionData userRooms;
		
		protected virtual void Awake() {
			userRooms.OnLoadedRoomSet += BuildRooms;
		}

		protected virtual void Start() {
			if (userRooms.LoadedRooms != null)
				BuildRooms(userRooms.LoadedRooms);
		}

		protected virtual void OnDestroy() => userRooms.OnLoadedRoomSet -= BuildRooms;

		protected void BuildRooms(JSON roomData) {
			if (roomData == null) return;
			JArray loadedRoomsInfo = roomData.GetJArray("rooms");
			Vector3 roomPos = transform.position;

			foreach (JSON roomInfo in loadedRoomsInfo.Values) {
				RoomData newRoom = BuildRoom(roomInfo);
				roomPos += new Vector3(
					(newRoom.Size.x * direction.x),
					(newRoom.Size.y * direction.y)
				) + roomGap;
				newRoom.Position = roomPos;
				userRooms.Add(newRoom);
			}
		}

		protected RoomData BuildRoom(JSON roomInfo) {
			string newType = roomInfo.GetString("type");
			GameObject newRoomObject =
				Instantiate(roomPrefabs[newType].RoomObject, Vector3.zero, Quaternion.identity, transform);
			return new RoomData(newRoomObject, newType);
		}
	}
}