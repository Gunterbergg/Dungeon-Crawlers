using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonCrawlers.UI 
{ 
    public class AnalogDirectionInput : UserView, IDirectionInput, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public bool clampInput = true;
        public RectTransform handle;

        protected float handleAreaRadius;
        protected Vector3 defaultPosition;
        protected RectTransform handleParent;

        public event Action<Vector2> Input;
        public event Action<Vector2> InputRelease;

        public bool IsHandlingInput { get; private set; } = false;
        public bool InputEnabled { get; set; } = true;

        protected override void Awake(){
            base.Awake();
            handleParent = (RectTransform)handle.transform.parent;
            handleAreaRadius = Mathf.Min(
                handleParent.rect.width /2f,
                handleParent.rect.height /2f);
    }

        public void OnPointerDown(PointerEventData pointerData) {
            if (IsHandlingInput || !InputEnabled) return;
            defaultPosition = handle.localPosition;
            IsHandlingInput = true;
            StartCoroutine(OnDirectionInput());
            OnDrag(pointerData);
        }

        public void OnDrag(PointerEventData pointerData) {
            handle.position = clampInput ? GetHandlePosClamped(pointerData.position) : pointerData.position;
        }

        public void OnPointerUp(PointerEventData pointerData) {
            InputRelease?.Invoke(GetInput());
            handle.localPosition = defaultPosition;
            IsHandlingInput = false;
        }

        public Vector2 GetInput() {
            return Vector2.ClampMagnitude(GetInputUnclamped(), 1);
        }

        public Vector2 GetInputUnclamped() {
            if (handle == null) return Vector2.zero;
            return (handle.position - handleParent.position) / handleAreaRadius;
        }

        public Vector2 GetInputRaw() {
            if (handle == null) return Vector2.zero;
            return (handle.position - handleParent.position);
        }

        protected Vector2 GetHandlePosClamped(Vector2 pointerPos) {
            Vector2 analogCenter = handleParent.position;
            return analogCenter + Vector2.ClampMagnitude(pointerPos - analogCenter, handleAreaRadius);
        }

        protected IEnumerator OnDirectionInput() {
            while (IsHandlingInput) {
                Input?.Invoke(GetInput());
                yield return null;
            }
        }
    }
}
