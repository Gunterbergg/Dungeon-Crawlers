using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;
using Leguar.TotalJSON;

namespace DungeonCrawlers.Systems
{
	public class UserRoomsLoaderSystem : MonoBehaviour
	{
		public WebRequestInfo roomRequest;
		public RoomCollectionData loadedRooms;
		public UserData userData;

		protected virtual void Awake() {
			loadedRooms.LoadedRooms = null;
			LoadUserRooms(userData.User_id.ToString());
			userData.OnPropertyChanged += LoadUserRooms;
		}

		protected virtual void OnDestroy() => userData.OnPropertyChanged -= LoadUserRooms;

		public void LoadUserRooms(string userId) {
			HTTPClient.GetRequest(
				roomRequest.RequestURL,
				new Dictionary<string, string> { { "user_id", userId } }, null,
				(request) => loadedRooms.LoadedRooms = JSON.ParseString(request.downloadHandler.text)
			);
		}
	}
}
