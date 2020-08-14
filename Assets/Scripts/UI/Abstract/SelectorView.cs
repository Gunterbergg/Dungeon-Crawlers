using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DungeonCrawlers.UI
{
	public abstract class SelectorView<Type> : UserView, IInputHandler<Type> where Type : class
	{
		//Interface implementation
		public event Action<Type> Input;
		public bool InputEnabled { get; set; } = true;
		public UnityEvent OnInput;

		//Selector components
		public UserView touchInput;
		public float holdingTreshold = 1f;

		private IEnumerable<Type> collection;
		private Type touchedEntity = null;
		private float holdingTime = 0f;

		protected override void Awake() {
			base.Awake();
			touchInput.GetInterface<IInputHandler<Touch>>().Input += OnTouchInput;
			RefreshReferences();
		}

		protected abstract IEnumerable<Type> GetCollection();

		protected abstract bool IntersectsWith(Type entity, Vector2 cursorPosition);

		public void RefreshReferences() {
			collection = GetCollection();
		}

		protected void OnTouchInput(Touch touch) {
			if (!InputEnabled || Input == null) return;
			Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

			if (touch.phase == TouchPhase.Ended) {
				touchedEntity = null;
				return;
			} else if (touch.phase == TouchPhase.Began) {
				touchedEntity = GetCurrentSelectedRoom(touchPos);
				holdingTime = 0;
			}

			if (touchedEntity == null || !IntersectsWith(touchedEntity, touchPos)) {
				holdingTime = 0;
				return;
			}

			holdingTime += Time.deltaTime;
			if (holdingTime >= holdingTreshold) {
				Input?.Invoke(touchedEntity);
				OnInput.Invoke();
				holdingTime = 0;
			}
		}

		private Type GetCurrentSelectedRoom(Vector2 cursorPosition) {
			foreach (Type entity in collection)
				if (IntersectsWith(entity, cursorPosition)) return entity;
			return null;
		}
	}
}
