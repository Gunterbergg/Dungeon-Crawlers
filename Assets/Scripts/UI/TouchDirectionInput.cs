using System;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.EventSystems;
	
namespace DungeonCrawlers.UI
{
	public class TouchDirectionInput : UserView, IDirectionInput, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public bool useInvertedInput = true;

		private Vector2 currentDir = Vector2.zero;
		private bool isHandlingPointer = false;
		private Vector2 pointerDownPos;

		public event EventHandler<EventArgs<Vector2>> UserInput;

		public Vector2 CurrentDir { 
			get { return useInvertedInput ? currentDir * -1 : currentDir; }
			private set { currentDir = value; }
		}

		public Vector2 GetInput() {
			return Vector2.ClampMagnitude(CurrentDir, 1);
		}

		public Vector2 GetInputRaw() {
			return CurrentDir;			
		}

		public Vector2 GetInputUnclamped() {
			return CurrentDir;
		}

		public void OnDrag(PointerEventData eventData) {
			CurrentDir = pointerDownPos - eventData.position;
			UserInput?.Invoke(this, new EventArgs<Vector2>(CurrentDir));
		}

		public void OnPointerDown(PointerEventData eventData) {
			if (isHandlingPointer) return;
			pointerDownPos = eventData.position;
			isHandlingPointer = true;
		}

		public void OnPointerUp(PointerEventData eventData) {
			CurrentDir = Vector2.zero;
			UserInput?.Invoke(this, new EventArgs<Vector2>(CurrentDir));
			isHandlingPointer = false;
		}
	}
}