using UnityEngine;

namespace DungeonCrawlers.Systems
{
	[ExecuteInEditMode]
	public class RoomGridSystem : MonoBehaviour
	{
		public Grid grid;
		public Vector3 offset;

		private void Awake() {
			if (!Application.isEditor || Application.isPlaying) enabled = false;
		}

		private void Update() {
			foreach (Transform child in transform) 
				child.localPosition = 
					grid.CellToLocal(grid.LocalToCell(child.localPosition)) + offset;
		}
	}
}