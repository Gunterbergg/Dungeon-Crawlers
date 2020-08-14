using System;
using System.Collections;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public abstract class CameraView : UserView, ICameraHandler
	{
		public Camera targetCam;
		public Vector4 cameraPadding;
		public bool zLock = true;
		public bool forceAspect = true;

		[SerializeField] private Rect limitBounds;
		[SerializeField] private float cameraMaxSize;
		[SerializeField] private float cameraMinSize;
		[SerializeField] private bool limitBoundsActive;

		private Coroutine movingCoroutine;
		private Coroutine zoomCoroutine;

		private float cacheMaxSize = 0f;
		private Rect cacheLimitPosRect = new Rect(0, 0, 0, 0);

		public virtual bool Enabled { get; set; }

		public virtual Rect LimitBounds {
			get => limitBounds;
			set { 
				limitBounds = AddPadding(value);
				ClampView();
			}
		}
		public virtual float CameraMaxSize {
			get => cameraMaxSize;
			set { 
				cameraMaxSize = value;
				ClampView();
			}
		}
		public virtual float CameraMinSize {
			get => cameraMinSize;
			set { 
				cameraMinSize = value;
				ClampView();
			}
		}

		public bool LimitBoundsActive { 
			get => limitBoundsActive;
			set {
				limitBoundsActive = value;
				ClampView();
			}
		}

		protected override void Awake() {
			base.Awake();
			Cache();
		}

		public virtual void MoveTo(Vector3 vec) {
			if (LimitBoundsActive) { 
				Vector3 clampedPos = DataUtility.ClampPositionToRect(vec, cacheLimitPosRect);
				clampedPos.z = zLock ? targetCam.transform.position.z : vec.z;
				targetCam.transform.position = clampedPos;
			} else { 
				targetCam.transform.position = new Vector3(vec.x, vec.y, zLock ? targetCam.transform.position.z : vec.z);
			}
		}

		public virtual void MoveBy(Vector3 vec) => MoveTo(targetCam.transform.position + vec);

		public virtual void LerpMoveTo(Vector3 vec, float time) {
			Vector3 initialPos = targetCam.transform.position;
			if (movingCoroutine != null) StopCoroutine(movingCoroutine);
			movingCoroutine = StartCoroutine(TimedRoutine(
				(normalizedTime) => MoveTo(Vector3.Lerp(initialPos, vec,normalizedTime)), time));	
		}

		public virtual void LerpMoveBy(Vector3 vec, float time) {
			float deltaTime = 0f;
			if (movingCoroutine != null) StopCoroutine(movingCoroutine);
			movingCoroutine = StartCoroutine(TimedRoutine(
				(normalizedTime) => {
					MoveBy(vec * (normalizedTime - deltaTime));
					deltaTime = normalizedTime;
				}, time));
		}

		public virtual void SetSize(float size) {
			if (size <= 0 || CameraMaxSize <= 0 || CameraMaxSize < cameraMinSize) return;
			targetCam.orthographicSize = Mathf.Clamp(size, CameraMinSize, cacheMaxSize);
			CachePosLimit();
			ClampPos();
		}

		public virtual void ChangeSize(float size) =>  SetSize(targetCam.orthographicSize + size);
		
		public virtual void LerpSetSize(float size, float time) {
			float initialSize = targetCam.orthographicSize;
			if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
			zoomCoroutine = StartCoroutine(TimedRoutine(
				(normalizedTime) => SetSize(Mathf.Lerp(initialSize, size, normalizedTime)), time));
		}

		public virtual void LerpChangeSize(float size, float time) {
			float deltaTime = 0f;
			if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
			zoomCoroutine = StartCoroutine(TimedRoutine(
				(normalizedTime) => {
					ChangeSize(size * (normalizedTime - deltaTime));
					deltaTime = normalizedTime;
				}, time));
		}

		public virtual void SetView(Rect view) {
			SetSize(Mathf.Min(view.height, view.width * targetCam.aspect));
			MoveTo(view.center);
		}

		public virtual void LerpSetView(Rect view, float time) {
			LerpChangeSize(Mathf.Min(view.height, view.width * targetCam.aspect), time);
			LerpMoveTo(view.center, time);
		}

		public virtual void MaxSize() {
			if (LimitBoundsActive)
				CameraMaxSize = Mathf.Min(LimitBounds.height / 2f, ((LimitBounds.width / 2f) / targetCam.aspect));
			SetSize(CameraMaxSize);
		}

		public virtual void MinSize() {
			if (LimitBoundsActive)
				CameraMinSize = Mathf.Min(CameraMinSize, LimitBounds.height / 2f, ((LimitBounds.width / 2f) / targetCam.aspect));
			SetSize(CameraMinSize);
		}

		protected virtual void ClampView() {
			Cache();
			ClampFov();
			ClampPos();
		}

		protected virtual Rect AddPadding(Rect bounds) {
			bounds.xMin -= cameraPadding.x;
			bounds.yMax += cameraPadding.y;
			bounds.xMax += cameraPadding.z;
			bounds.yMin -= cameraPadding.w;

			if (!forceAspect) return bounds;

			Vector2 center = bounds.center;
			bool isLandscape = bounds.width / targetCam.aspect > bounds.height;
			if (isLandscape) bounds.height = bounds.width / targetCam.aspect;
			else bounds.width = bounds.height * targetCam.aspect;
			bounds.center = center;

			return bounds;

		}

		private void Cache() {
			CacheSizeLimit();
			CachePosLimit();
		}

		private void CacheSizeLimit() {
			cacheMaxSize = LimitBoundsActive ? 
				LimitBounds.width > 0 && LimitBounds.height > 0 ?
					Mathf.Min(CameraMaxSize, LimitBounds.height / 2f, (LimitBounds.width / 2f) / targetCam.aspect) :
					CameraMaxSize :
				CameraMaxSize;
			Debug.DrawLine(targetCam.transform.position, targetCam.transform.position + (Vector3.up * cacheMaxSize), Color.cyan, 1f);
		}

		private void CachePosLimit() {
			Vector2 newSize = new Vector2(
				Mathf.Max(LimitBounds.width - ((targetCam.orthographicSize * 2) * targetCam.aspect), 0),
				Mathf.Max(LimitBounds.height - targetCam.orthographicSize * 2, 0));
			cacheLimitPosRect = new Rect() { size = newSize, center = LimitBounds.center };
			DataUtility.DrawRect(LimitBounds, Color.red, 1f);
			DataUtility.DrawRect(cacheLimitPosRect, Color.blue, 1f);
		}

		private void ClampFov() {
			if (!LimitBoundsActive) return;
			if (CameraMaxSize <= 0 || CameraMaxSize < cameraMinSize) return;
			targetCam.orthographicSize = Mathf.Clamp(targetCam.orthographicSize, CameraMinSize, cacheMaxSize);
		}

		private void ClampPos() {
			if (!LimitBoundsActive) return;
			targetCam.transform.position = 
				DataUtility.ClampPositionToRect(targetCam.transform.position, cacheLimitPosRect);
		}

		private IEnumerator TimedRoutine(Action<float> function, float time) {
			float timer = 0f;
			while (timer <= 1f) {
				function.Invoke(timer);
				timer += Time.deltaTime / time;
				yield return null;
			}
			function.Invoke(1f);
		}
	}
}
