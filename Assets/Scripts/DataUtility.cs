using System;
using UnityEngine;
using Leguar.TotalJSON;

namespace DungeonCrawlers
{
	public enum Direction
	{
		Left, Up, Right, Down,
		UpperLeft, UpperRight, LowerRight, LowerLeft,
		Neutral
	}

	public static class DataUtility
	{
		public static DeserializeSettings deserializationSettings =
			new DeserializeSettings() {
				RequireAllFieldsArePopulated = false,
				RequireAllJSONValuesAreUsed = false,
				AllowNonStringDictionaryKeys = true
			};

		public static DataType GetData<DataType>(GameObject gameObject) where DataType : MonoBehaviour {
			DataType dataComponent = gameObject.GetComponent<DataType>();
			if (dataComponent == null)
				dataComponent = gameObject.AddComponent<DataType>();
			return dataComponent;
		}

		public static Vector3 GetOrbitalPosition(Vector3 origin, float angle, float distance) {
			return origin + (distance * (Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up));
		}

		public static float GetAngle(Vector3 direction) {
			direction.Normalize();
			return Vector3.Angle(Vector3.up, direction) * (direction.x > 0f ? -1f : 1f);
		}

		public static Direction GetFourDirection(Vector2 dir) {
			if (dir.x == dir.y) {
				return Direction.Neutral;
			} else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
				if (dir.x > 0) return Direction.Right;
				return Direction.Left;
			} else {
				if (dir.y > 0) return Direction.Up;
				return Direction.Down;
			}
		}

		public static Direction GetEightDirection(Vector2 dir) {
			float inputAngle = Mathf.Atan2(Mathf.Abs(dir.x), Mathf.Abs(dir.y)) * Mathf.Rad2Deg;
			if (inputAngle <= 23 || inputAngle >= 67) return GetFourDirection(dir);
			if (dir.x > 0) {
				if (dir.y > 0) return Direction.UpperRight;
				return Direction.LowerRight;
			} else {
				if (dir.y > 0) return Direction.UpperLeft;
				return Direction.LowerLeft;
			}
		}

		public static Rect GetObjectRectBounds(GameObject gameObject) {
			Rect objectBounds = new Rect(gameObject.transform.position, Vector2.zero);
			SpriteRenderer[] rendererCollection = gameObject.GetComponentsInChildren<SpriteRenderer>();

			foreach (SpriteRenderer sprite in rendererCollection) {
				Bounds bounds = sprite.bounds;
				if (objectBounds.xMin > bounds.min.x) objectBounds.xMin = bounds.min.x;
				if (objectBounds.xMax < bounds.max.x) objectBounds.xMax = bounds.max.x;
				if (objectBounds.yMin > bounds.min.y) objectBounds.yMin = bounds.min.y;
				if (objectBounds.yMax < bounds.max.y) objectBounds.yMax = bounds.max.y;
			}

			return objectBounds;
		}

		public static T GetAttribute<T>(this Enum enumVal) where T : Attribute {
			var type = enumVal.GetType();
			var memInfo = type.GetMember(enumVal.ToString());
			var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
			return (attributes.Length > 0) ? (T)attributes[0] : null;
		}

		public static Rect BoundsToRect(Bounds bounds) {
			return new Rect() {size = bounds.size, center = bounds.center };
		}

		public static Rect EncapsulateRect(this Rect rect, Rect other) {
			rect.xMax = Mathf.Max(rect.xMax, other.xMax);
			rect.yMax = Mathf.Max(rect.yMax, other.yMax);
			rect.xMin = Mathf.Min(rect.xMin, other.xMin);
			rect.yMin = Mathf.Min(rect.yMin, other.yMin);
			return rect;
		}

		public static void DrawRect(Rect rect, Color color, float duration) {
			Debug.DrawLine(rect.min, new Vector3(rect.xMax, rect.yMin), color, duration);
			Debug.DrawLine(rect.min, new Vector3(rect.xMin, rect.yMax), color, duration);
			Debug.DrawLine(rect.max, new Vector3(rect.xMin, rect.yMax), color, duration);
			Debug.DrawLine(rect.max, new Vector3(rect.xMax, rect.yMin), color, duration);
		}

		public static Vector3 ClampPositionToRect(Vector3 pos, Rect rect) {
			return new Vector3(
				Mathf.Clamp(pos.x, rect.xMin, rect.xMax),
				Mathf.Clamp(pos.y, rect.yMin, rect.yMax),
				pos.z);
		}

	}
}
