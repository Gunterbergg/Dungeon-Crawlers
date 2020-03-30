using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class RoomEditorSystem : MonoBehaviour
	{
		public UserView roomSelector;
		public GameObject highlight;

		private void Awake() {
			highlight.SetActive(false);
			roomSelector.GetInterface<IUserInput<EventArgs<RoomInfo>>>().UserInput +=
				(sender, args) => OnRoomSelected(args.Data);
		}

		private void OnRoomSelected(RoomInfo selected) {
			highlight.transform.position = selected.Position;
			highlight.SetActive(true);
		}
	}
}