using System;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	/*
	public class RoomComponent : MonoBehaviour
	{
		
		public Room data;
		public ComponentCollection entities;
		
		[SerializeField] private bool isActive = false;
		[SerializeField] private Vector2 size;

		public bool IsActive { 
			get => isActive;
			set {
				bool hasChanged = isActive != value;
				isActive = value;
				if (hasChanged) {
					Changed?.Invoke();
					PropertyChanged?.Invoke("IsActive");
				}
			}
		}
		public Vector2 Size { 
			get => size;
			set {
				bool hasChanged = size != value;
				size = value;
				if (hasChanged) {
					Changed?.Invoke();
					PropertyChanged?.Invoke("Size");
				}
			}
		}
		public Rect Rect { get => new Rect() { size = this.size, center = transform.position }; }

		public event Action Changed;
		public event Action<string> PropertyChanged;

		public void RaiseChangedEvent() => Changed?.Invoke();
	}	
	*/
}
