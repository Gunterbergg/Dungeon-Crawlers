﻿using System;
using System.Collections;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.EventSystems;
	
namespace DungeonCrawlers.UI
{
	public class TouchDirectionInput : UserView, IDirectionInput, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public bool useInvertedInput = true;
		private Vector2 currentDir = Vector2.zero;
		private Vector2 pointerDownPos;

		public event EventHandler<EventArgs<Vector2>> UserInput;
		public event EventHandler<EventArgs<Vector2>> InputRelease;

		public bool IsHandlingInput { get; private set; } = false;

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
			InputRelease?.Invoke(this, new EventArgs<Vector2>(CurrentDir));
			CurrentDir = Vector2.zero;
			IsHandlingInput = false;
		}

		private IEnumerator OnDirectionInput() {
			while (IsHandlingInput) {
				UserInput?.Invoke(this, new EventArgs<Vector2>(GetInput()));
				yield return null;
			}
		}
	}
}