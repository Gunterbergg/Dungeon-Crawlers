using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;
using Leguar.TotalJSON;

namespace DungeonCrawlers.Systems
{
	public class RoomLoaderSystem : MonoBehaviour
	{
		public Vector3 roomGap = Vector3.right * 5;
		public Vector3 direction = Vector3.right;

		public RoomCollectionInfo roomPrefabs;
		public RoomCollectionInfo userRooms;
		public WebRequestInfo roomRequest;
		public UserInfo userData;
		
		private void Awake() {
			LoadUserRooms();
		}

		public void LoadUserRooms() {
			HTTPClient.GetRequest(
				roomRequest.RequestURL,
				new Dictionary<string, string> { { "user_id", userData.user_id.ToString() } }, null,
				(sender, args) => BuildRooms(JSON.ParseString(args.Data.downloadHandler.text))
			);
		}

		private void BuildRooms(JSON roomData) {
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