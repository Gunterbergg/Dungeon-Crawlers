using System;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public class RoomData
	{
		public string type = "none";

		[SerializeField]
		protected GameObject roomObject;
		protected Vector2 size;
		protected Rect rect;

		public RoomData(GameObject roomObject, string type = "none") {
			this.roomObject = roomObject;
			this.type = type;
			CalculateSize();
		}

		public event Action onTypeChanged;
		public event Action onPositionChanged;

		public GameObject RoomObject {
			get { return roomObject; }
			set { 
				roomObject = value;
				CalculateSize();
				onTypeChanged?.Invoke();
			}
		}

		public Vector2 Position {
			get { return RoomObject.transform.position; }
			set {
				bool hasChanged = false;
				if (value != Position) hasChanged = true;
				RoomObject.transform.position = value;
				rect.center = Position;
				if (hasChanged) onPositionChanged?.Invoke();
			}
		}

		public Vector2 Size { get => size; }

		public bool OverlapsPoint(Vector2 point) {
			return rect.Contains(point);
		}

		protected void CalculateSize() {
			if (RoomObject == null) return;

			BoundsRectInfo rectBuilder = new BoundsRectInfo();
			SpriteRenderer[] rendererCollection = RoomObject.GetComponentsInChildren<SpriteRenderer>();

			foreach (SpriteRenderer sprite in rendererCollection) {
				rectBuilder.ClampHorizontalBounds(sprite.bounds.min.x, sprite.bounds.max.x);
				rectBuilder.ClampVerticalBounds(sprite.bounds.min.y, sprite.bounds.max.y);
			}

			size.x = rectBuilder.Width;
			size.y = rectBuilder.Height;
			rect.size = Size;
		}
	}
}
