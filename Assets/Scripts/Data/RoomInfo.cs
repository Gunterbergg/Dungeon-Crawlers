using System;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "RoomInfo", menuName = "InfoContainer/RoomInfo")]
	public class RoomInfo : ScriptableObject
	{
		public string type;

		[SerializeField]
		protected GameObject roomObject;
		protected Vector2 size;
		protected Rect rect;

		public event EventHandler onTypeChanged;
		public event EventHandler onPositionChanged;

		public GameObject RoomObject {
			get { return roomObject; }
			set { 
				roomObject = value;
				CalculateSize();
				onTypeChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public Vector2 Position {
			get { return RoomObject.transform.position; }
			set {
				bool hasChanged = false;
				if (value != Position) hasChanged = true;
				RoomObject.transform.position = value;
				rect.position = Position;
				if (hasChanged) onPositionChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public Vector2 Size { get => size; }

		public void CalculateSize() {
			if (RoomObject == null) return;

			Vector3 minPos = Vector3.zero;
			Vector3 maxPos = Vector3.zero;
			SpriteRenderer[] rendererCollection = RoomObject.GetComponentsInChildren<SpriteRenderer>();

			foreach (SpriteRenderer sprite in rendererCollection) {
				if (sprite.bounds.min.x < minPos.x) minPos.x = sprite.bounds.min.x;
				if (sprite.bounds.min.y < minPos.y) minPos.y = sprite.bounds.min.y;
				if (sprite.bounds.max.x > maxPos.x) maxPos.x = sprite.bounds.max.x;
				if (sprite.bounds.max.y > maxPos.y) maxPos.y = sprite.bounds.max.y;
			}

			size.x = maxPos.x - minPos.x;
			size.y = maxPos.y - minPos.y;
		}
	}
}