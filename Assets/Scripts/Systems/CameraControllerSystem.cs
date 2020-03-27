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

		public RoomCollectionInfo userRooms;
		public bool automaticLimitBounds = true;
		public Vector2 automaticBoundsPadding = Vector2.zero;

		protected void Awake() {
			if (automaticLimitBounds)
				userRooms.OnValueChanged += (sender, args) => {
					RedefineBounds();
					CenterCamera();
				};

			ZoomCamera(cameraMaxSize);
			touchInput.GetInterface<IUserInput<EventArgs<List<Touch>>>>().UserInput +=
				(sender, args) => CameraController(args.Data);
		}

		private void CameraController(List<Touch> touches) {
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

		private void CameraMovementController(Touch touch) {
			//TODO implement camera movement based on the percentual the delta vector traveled on screen
			Vector2 cameraMoveVector =
				touch.deltaPosition * Time.deltaTime * cameraSpeedModifier * -1 * (Camera.main.orthographicSize / 5f);
			Camera.main.transform.position += (Vector3)cameraMoveVector;
			ClampToLimitBox();
		}

		private void CameraZoomController(Touch touchZero, Touch touchOne) {
			float prevTouchDistance = (
				(touchZero.position - touchZero.deltaPosition) -
				(touchOne.position - touchOne.deltaPosition)).magnitude;
			float touchDistance = (touchZero.position - touchOne.position).magnitude;

			float deltaDistance = prevTouchDistance - touchDistance;
			ZoomCamera(deltaDistance * Time.deltaTime);
		}

		private void ZoomCamera(float size) {
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

		private void ClampToLimitBox() {
			Transform camTransform = Camera.main.transform;
			camTransform.position = new Vector3(
				Mathf.Clamp(camTransform.position.x, limitBounds.xMin + Camera.main.orthographicSize * Camera.main.aspect, limitBounds.xMax - Camera.main.orthographicSize * Camera.main.aspect),
				Mathf.Clamp(camTransform.position.y, limitBounds.yMin + Camera.main.orthographicSize, limitBounds.yMax - Camera.main.orthographicSize),
				camTransform.position.z);
		}

		private void RedefineBounds() {
			if (userRooms.Count <= 0) return;
			float xMin, yMin, xMax, yMax, comparative;
			xMin = xMax = userRooms[0].Position.x;
			yMin = yMax = userRooms[0].Position.y;
			foreach (RoomInfo room in userRooms) {
				if (room.RoomObject == null) continue;

				comparative = room.Position.x - room.Size.x / 2;
				if (comparative < xMin) xMin = comparative;
				
				comparative = room.Position.y - room.Size.y / 2;
				if (comparative < yMin) yMin = comparative;
				
				comparative = room.Position.x + room.Size.x / 2;
				if (comparative > xMax) xMax = comparative;
				
				comparative = room.Position.y + room.Size.y / 2;
				if (comparative > yMax) yMax = comparative;
			}

			limitBounds = new Rect(
				xMin - automaticBoundsPadding.x,
				yMin - automaticBoundsPadding.y,
				xMax - xMin + automaticBoundsPadding.x * 2,
				yMax - yMin + automaticBoundsPadding.y * 2);
		}

		public void CenterCamera() {
			Camera.main.transform.position = new Vector3(limitBounds.center.x, limitBounds.center.y, Camera.main.transform.position.z);
			ZoomCamera(0);
		}
	}
}