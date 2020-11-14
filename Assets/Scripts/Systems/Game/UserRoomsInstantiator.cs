using DungeonCrawlers.Data;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class UserRoomsInstantiator : MonoBehaviour
	{
		public RoomMetaCollection roomsMeta;
		private GameObject room;

		protected virtual void Awake() {
			UserRoomsInstantiatorSetup();
		}

		private void OnDestroy() {
			Session.Instance.user.OnPropertyChanged -= OnPropertyChanged;
		}

		public void InstantiateRoom() {
			if (room != null) Destroy(room.gameObject);
			room = Instantiate(roomsMeta[Session.Instance.user.RoomType].prefab, transform);
		}

		public void UserRoomsInstantiatorSetup() {
			Session.Instance.user.OnPropertyChanged += OnPropertyChanged;
		}

		public void OnPropertyChanged(string property) {
			if (property != "RoomType") return;
			InstantiateRoom();
		}
	}
}
