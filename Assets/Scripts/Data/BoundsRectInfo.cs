using UnityEngine;

namespace DungeonCrawlers.Data
{
	public struct BoundsRectInfo
	{
		public float xMin, yMin, xMax, yMax;

		public BoundsRectInfo(float defaultValue = 0) {
			xMin = yMin = xMax = yMax = defaultValue;
		}

		public BoundsRectInfo(float xDefault, float yDefault) {
			xMin = xMax = xDefault;
			yMin = yMax = yDefault;
		}

		public Vector2 Max {
			get => new Vector2(xMin, yMax);
			set {
				xMax = value.x;
				yMax = value.y;
			}
		}

		public Vector2 Min {
			get => new Vector2(xMax, yMin);
			set {
				xMin = value.x;
				yMin = value.y;
			}
		}

		public float Width { get => xMax - xMin; }
		public float Height { get => yMax - yMin; }
		public Rect Rect { get => new Rect(xMin, yMin, Width, Height);}

		public void ClampHorizontalBounds(params float[] nums) {
			foreach (float num in nums) { 
				if (num < xMin) xMin = num;
				if (num > xMax) xMax = num;
			}
		}

		public void ClampVerticalBounds(params float[] nums) {
			foreach (float num in nums) { 
				if (num < yMin) yMin = num;
				if (num > yMax) yMax = num;
			}
		}
	}
}
