using System;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonCrawlers.UI 
{ 
    public class AnalogDirectionInput : UserView, IDirectionInput, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public float handleAreaRadius;
        public bool clampInput = true;
        public RectTransform handle;

        private Vector3 defaultPosition;

        public event EventHandler<EventArgs<Vector2>> UserInput;

        protected override void Awake(){
            base.Awake();
            handleAreaRadius = Mathf.Min(
                ((RectTransform)transform).rect.width /2f,
                ((RectTransform)transform).rect.height /2f);
    }

        public void OnPointerDown(PointerEventData pointerData) {
            defaultPosition = handle.localPosition;
            OnDrag(pointerData);
        }

        public void OnDrag(PointerEventData pointerData) {
            handle.position = clampInput ? GetHandlePosClamped(pointerData.position) : pointerData.position;
            UserInput?.Invoke(this, new EventArgs<Vector2>(GetInput()));
        }

        public void OnPointerUp(PointerEventData pointerData) {
            handle.localPosition = defaultPosition;
            UserInput?.Invoke(this, new EventArgs<Vector2>(GetInput()));
        }

        public Vector2 GetInput() {
            return Vector2.ClampMagnitude(GetInputUnclamped(), 1);
        }

        public Vector2 GetInputUnclamped() {
            if (handle == null) return Vector2.zero;
            return (handle.position - ((RectTransform)transform).position) / handleAreaRadius;
        }

        public Vector2 GetInputRaw() {
            if (handle == null) return Vector2.zero;
            return (handle.position - ((RectTransform)transform).position);
        }

        private Vector2 GetHandlePosClamped(Vector2 pointerPos) {
            Vector2 analogCenter = ((RectTransform)transform).position;
            return analogCenter + Vector2.ClampMagnitude(pointerPos - analogCenter, handleAreaRadius);
        }
    }
}