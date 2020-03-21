using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;
using Leguar.TotalJSON;

namespace DungeonCrawlers.Systems
{
	public class RoomLoaderSystem : MonoBehaviour
	{
		public int roomGap = 3;
		public Vector3 roomDirection = Vector3.right;

		public RoomCollectionInfo roomPrefabs;
		public RoomCollectionInfo userRooms;
		public WebRequestInfo roomRequestInfo;
		public UserInfo userInfo;
		
		private void Awake() {
			ReloadUserRooms();
		}

		public void ReloadUserRooms() {
			HTTPClient.GetRequest(
				roomRequestInfo.RequestURL,
				new Dictionary<string, string> { { userInfo.id.ToString(), "0" } }, null,
				(sender, args) => {
					JArray loadedRooms = JSON.ParseString(args.Data.downloadHandler.text).GetJArray("rooms");
					RoomCollectionInfo deserializedRooms = ScriptableObject.CreateInstance<RoomCollectionInfo>();
					foreach (JSON loadedRoom in loadedRooms.Values) {
						RoomInfo deserializedRoom = (RoomInfo)ScriptableObject.CreateInstance(typeof(RoomInfo));
						deserializedRoom.type = loadedRoom.GetString("type");
						BuildRoom(deserializedRoom);
						deserializedRooms[loadedRoom.GetInt("index")] = deserializedRoom;
					}
					SetRooms(deserializedRooms);
				});
		}

		private void BuildRoom(RoomInfo roomData) {
			roomData.roomSize = roomPrefabs[roomData.type].roomSize;
			roomData.roomObject = 
				Instantiate(roomPrefabs[roomData.type].roomObject, Vector3.zero, Quaternion.identity, transform);
		}

		private void SetRooms(RoomCollectionInfo roomList) {
			Vector3 roomPos = transform.position - Vector3.left * roomGap;
			for (int index = 0; index < roomList.Count; index++) {
				if (roomList[index] == null) continue;
				roomPos += Vector3.right * (roomGap + roomList[index].roomSize.x);
				roomList[index].roomObject.transform.position = roomPos - (roomList[index].roomSize.x / 2f) * Vector3.right;
				userRooms[index] = roomList[index];
			}
		}
	}
}