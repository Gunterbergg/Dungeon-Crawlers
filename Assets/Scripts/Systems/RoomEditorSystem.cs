using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class RoomEditorSystem : MonoBehaviour
	{
		public UserView roomSelector;
		public GameObject highlight;

		protected virtual void Awake() {
			highlight.SetActive(false);
			roomSelector.GetInterface<IInputHandler<RoomData>>().Input += OnRoomSelected;
		}

		protected void OnRoomSelected(RoomData selected) {
			highlight.transform.position = selected.Position;
			highlight.SetActive(true);
		}
	}
}
