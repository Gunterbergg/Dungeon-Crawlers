using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonCrawlers.UI
{
	public class TouchInput : UserView, IInputHandler<Touch>, IInputHandler<List<Touch>>, IPointerDownHandler
	{
		private bool inputEnabled = true;

		public event Action<Touch> OnTouchInput;
		public event Action<List<Touch>> OnTouchesInput;

		event Action<List<Touch>> IInputHandler<List<Touch>>.Input {
			add => OnTouchesInput += value;
			remove => OnTouchesInput -= value;
		}

		event Action<Touch> IInputHandler<Touch>.Input { 
			add => OnTouchInput += value;
			remove => OnTouchInput -= value;
		}

		public bool IsHandlingInput { get; private set; }
		bool IInputHandler<Touch>.InputEnabled { get => inputEnabled; set => inputEnabled = value; }
		bool IInputHandler<List<Touch>>.InputEnabled { get => inputEnabled; set => inputEnabled = value; }

		public void OnPointerDown(PointerEventData eventData) {
			if (IsHandlingInput || !inputEnabled) return;
			IsHandlingInput = true;
			StartCoroutine(TouchHandler());
		}

		private IEnumerator TouchHandler() {
			List<Touch> touchesInBounds = new List<Touch>();
			while (IsHandlingInput) {
				touchesInBounds.Clear();
				foreach (Touch touch in Input.touches)
					if (RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, touch.position))
						touchesInBounds.Add(touch);

				if (touchesInBounds.Count == 0) {
					IsHandlingInput = false;
					continue;
				}

				OnTouchInput?.Invoke(touchesInBounds[0]);
				OnTouchesInput?.Invoke(touchesInBounds);
				yield return null;
			}
		}
	}
}