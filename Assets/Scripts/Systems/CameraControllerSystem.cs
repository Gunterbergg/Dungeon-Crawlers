using System.Collections.Generic;
using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class CameraControllerSystem : MonoBehaviour
	{
		public UserView touchInput;

		public float cameraMinSize = 2f;
		public float cameraMaxSize = 30f;
		public float cameraSpeedModifier = 1.05f;
		public Rect limitBounds;

		public RoomCollectionData userRooms;
		public bool automaticLimitBounds = true;
		public Vector2 automaticBoundsPadding = Vector2.zero;

		protected virtual void Awake() {
			if (automaticLimitBounds)
				userRooms.OnValueChanged += RefreshCamera;

			ZoomCamera(cameraMaxSize);
			touchInput.GetInterface<IInputHandler<List<Touch>>>().Input +=
				CameraController;
		}

		protected void OnDestroy() => userRooms.OnValueChanged -= RefreshCamera;

		public void CenterCamera() {
			Camera.main.transform.position = new Vector3(limitBounds.center.x, limitBounds.center.y, Camera.main.transform.position.z);
			ZoomCamera(0);
		}

		protected void RefreshCamera(RoomData obj) => RefreshCamera();
		protected void RefreshCamera() {
			RedefineBounds();
			CenterCamera();
		}

		protected void CameraController(List<Touch> touches) {
			if (touches.Count == 1) CameraMovementController(touches[0]); else
			if (touches.Count == 2) CameraZoomController(touches[0], touches[1]);
		}

		private void OnDrawGizmos() {
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(new Vector2(limitBounds.x, limitBounds.y), new Vector2(limitBounds.xMax, limitBounds.y));
			Gizmos.DrawLine(new Vector2(limitBounds.x, limitBounds.y), new Vector2(limitBounds.x, limitBounds.yMax));
			Gizmos.DrawLine(new Vector2(limitBounds.x, limitBounds.yMax), new Vector2(limitBounds.xMax, limitBounds.yMax));
			Gizmos.DrawLine(new Vector2(limitBounds.xMax, limitBounds.y), new Vector2(limitBounds.xMax, limitBounds.yMax));
		}

		protected void CameraMovementController(Touch touch) {
			//TODO implement camera movement based on the percentual the delta vector traveled on screen
			Vector2 cameraMoveVector =
				touch.deltaPosition * Time.deltaTime * cameraSpeedModifier * -1 * (Camera.main.orthographicSize / 5f);
			Camera.main.transform.position += (Vector3)cameraMoveVector;
			ClampToLimitBox();
		}

		protected void CameraZoomController(Touch touchZero, Touch touchOne) {
			float prevTouchDistance = (
				(touchZero.position - touchZero.deltaPosition) -
				(touchOne.position - touchOne.deltaPosition)).magnitude;
			float touchDistance = (touchZero.position - touchOne.position).magnitude;

			float deltaDistance = prevTouchDistance - touchDistance;
			ZoomCamera(deltaDistance * Time.deltaTime);
		}

		protected void ZoomCamera(float size) {
			Camera.main.orthographicSize = Camera.main.orthographicSize + size;
			ClampToLimitBox();
			Camera.main.orthographicSize =
				Mathf.Clamp(
					Camera.main.orthographicSize + size, cameraMinSize,
					Mathf.Min(
						cameraMaxSize,
						limitBounds.height / 2f,
						(limitBounds.width / 2f) / Camera.main.aspect));
		}

		protected void ClampToLimitBox() {
			Transform camTransform = Camera.main.transform;
			camTransform.position = new Vector3(
				Mathf.Clamp(camTransform.position.x, limitBounds.xMin + Camera.main.orthographicSize * Camera.main.aspect, limitBounds.xMax - Camera.main.orthographicSize * Camera.main.aspect),
				Mathf.Clamp(camTransform.position.y, limitBounds.yMin + Camera.main.orthographicSize, limitBounds.yMax - Camera.main.orthographicSize),
				camTransform.position.z);
		}

		protected void RedefineBounds() {
			if (userRooms.Count <= 0) return;
			BoundsRectInfo rectBuilder = new BoundsRectInfo(userRooms[0].Position.x, userRooms[0].Position.y);

			foreach (RoomData room in userRooms) {
				rectBuilder.ClampHorizontalBounds(room.Position.x - room.Size.x / 2, room.Position.x + room.Size.x / 2);				
				rectBuilder.ClampVerticalBounds(room.Position.y - room.Size.y / 2, room.Position.y + room.Size.y / 2);
			}

			limitBounds = rectBuilder.Rect;
			limitBounds.xMin -= automaticBoundsPadding.x;
			limitBounds.yMin -= automaticBoundsPadding.y;
			limitBounds.xMax += automaticBoundsPadding.x;
			limitBounds.yMax += automaticBoundsPadding.y;
		}
	}
}
