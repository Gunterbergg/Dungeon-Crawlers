using DungeonCrawlers.Data;
using Leguar.TotalJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class UserRoomsLoaderSystem : MonoBehaviour
	{
		public RoomCollectionInfo loadedRooms;
		public WebRequestInfo roomRequest;
		public UserInfo userData;

		private void Awake() {
			loadedRooms.LoadedRooms = null;
			LoadUserRooms(userData.User_id.ToString());
			userData.OnPropertyChanged += LoadUserRooms;
		}

		private void OnDestroy() => userData.OnPropertyChanged -= LoadUserRooms;

		public void LoadUserRooms(object sender, string userId) => LoadUserRooms(userId);
		public void LoadUserRooms(string userId) {
			HTTPClient.GetRequest(
				roomRequest.RequestURL,
				new Dictionary<string, string> { { "user_id", userId } }, null,
				(sender, args) => loadedRooms.LoadedRooms = JSON.ParseString(args.Data.downloadHandler.text)
			);
		}
	}
}