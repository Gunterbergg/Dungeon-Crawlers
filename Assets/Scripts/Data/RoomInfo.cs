﻿using DungeonCrawlers.Data.Struct;
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
				rect.center = Position;
				if (hasChanged) onPositionChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public Vector2 Size { get => size; }

		public void CalculateSize() {
			if (RoomObject == null) return;

			RectBuilder rectBuilder = new RectBuilder();
			SpriteRenderer[] rendererCollection = RoomObject.GetComponentsInChildren<SpriteRenderer>();

			foreach (SpriteRenderer sprite in rendererCollection) {
				rectBuilder.AddHorizontal(sprite.bounds.min.x, sprite.bounds.max.x);
				rectBuilder.AddVertical(sprite.bounds.min.y, sprite.bounds.max.y);
			}

			size.x = rectBuilder.Width;
			size.y = rectBuilder.Height;
		}
	}
}