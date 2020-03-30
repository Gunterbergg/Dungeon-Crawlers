using System;
using System.Collections;
using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonCrawlers.UI
{
	public class TouchInput : UserView, IUserInput<EventArgs<Touch>>, IUserInput<EventArgs<List<Touch>>>, IPointerDownHandler
	{
		public event EventHandler<EventArgs<Touch>> OnTouchInput;
		public event EventHandler<EventArgs<List<Touch>>> OnTouchesInput;

		event EventHandler<EventArgs<List<Touch>>> IUserInput<EventArgs<List<Touch>>>.UserInput {
			add => OnTouchesInput += value;
			remove => OnTouchesInput -= value;
		}

		event EventHandler<EventArgs<Touch>> IUserInput<EventArgs<Touch>>.UserInput { 
			add => OnTouchInput += value;
			remove => OnTouchInput -= value;
		}

		public bool IsHandlingInput { get; private set; }

		public void OnPointerDown(PointerEventData eventData) {
			if (IsHandlingInput) return;
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

				OnTouchInput?.Invoke(this, new EventArgs<Touch>(touchesInBounds[0]));
				OnTouchesInput?.Invoke(this, new EventArgs<List<Touch>>(touchesInBounds));
				yield return null;
			}
		}
	}
}