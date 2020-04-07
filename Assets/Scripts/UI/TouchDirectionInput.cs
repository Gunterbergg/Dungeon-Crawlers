using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
	
namespace DungeonCrawlers.UI
{
	public class TouchDirectionInput : UserView, IDirectionInput, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public bool useInvertedInput = true;
		private Vector2 currentDir = Vector2.zero;
		private Vector2 pointerDownPos;

		public event Action<Vector2> Input;
		public event Action<Vector2> InputRelease;

		public bool IsHandlingInput { get; private set; } = false;
		public bool InputEnabled { get; set; } = true;

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
		}

		public void OnPointerDown(PointerEventData eventData) {
			if (IsHandlingInput) return;
			pointerDownPos = eventData.position;
			IsHandlingInput = true;
			StartCoroutine(OnDirectionInput());
		}

		public void OnPointerUp(PointerEventData eventData) {
			InputRelease?.Invoke(CurrentDir);
			CurrentDir = Vector2.zero;
			IsHandlingInput = false;
		}

		private IEnumerator OnDirectionInput() {
			while (IsHandlingInput) {
				Input?.Invoke(GetInput());
				yield return null;
			}
		}
	}
}