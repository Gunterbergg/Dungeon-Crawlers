using System;
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
		public RoomCollectionInfo userRooms;
		
		private void Awake() {
			userRooms.OnLoadedRoomSet += BuildRooms;
		}

		private void Start() {
			if (userRooms.LoadedRooms != null)
				BuildRooms(userRooms.LoadedRooms);
		}

		private void OnDestroy() => userRooms.OnLoadedRoomSet -= BuildRooms;

		private void BuildRooms(object sender, EventArgs args) => BuildRooms(userRooms.LoadedRooms);
		private void BuildRooms(JSON roomData) {
			if (roomData == null) return;
			Debug.Log("Building...");
			JArray loadedRoomsInfo = roomData.GetJArray("rooms");
			Vector3 roomPos = transform.position;

			foreach (JSON roomInfo in loadedRoomsInfo.Values) {
				RoomInfo newRoom = BuildRoom(roomInfo);
				roomPos += new Vector3(
					(newRoom.Size.x * direction.x),
					(newRoom.Size.y * direction.y)
				) + roomGap;
				newRoom.Position = roomPos;
				userRooms.Add(newRoom);
			}
		}

		private RoomInfo BuildRoom(JSON roomInfo) {
			RoomInfo newRoom = (RoomInfo)ScriptableObject.CreateInstance(typeof(RoomInfo));
			newRoom.type = roomInfo.GetString("type");
			newRoom.RoomObject =
				Instantiate(roomPrefabs[newRoom.type].RoomObject, Vector3.zero, Quaternion.identity, transform);
			return newRoom;
		}
	}
}