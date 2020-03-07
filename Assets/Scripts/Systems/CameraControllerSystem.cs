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
		public RectInt limitBounds;
		public bool automaticLimitBounds = true;

		protected void Awake() {
			if (automaticLimitBounds) RedefineBounds();

			ClampToLimitBox();
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
			Vector2 cameraMoveVector = touch.deltaPosition * Time.deltaTime * cameraSpeedModifier * -1;
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
			Camera.main.orthographicSize =
				Mathf.Clamp(
					Camera.main.orthographicSize + size, cameraMinSize,
					Mathf.Min(
						cameraMaxSize,
						limitBounds.height / 2f,
						(limitBounds.width / 2f) / Camera.main.aspect));
			ClampToLimitBox();
		}

		private void ClampToLimitBox() {
			Transform camTransform = Camera.main.transform;
			camTransform.position = new Vector3(
				Mathf.Clamp(camTransform.position.x, limitBounds.xMin + Camera.main.orthographicSize * Camera.main.aspect, limitBounds.xMax - Camera.main.orthographicSize * Camera.main.aspect),
				Mathf.Clamp(camTransform.position.y, limitBounds.yMin + Camera.main.orthographicSize, limitBounds.yMax - Camera.main.orthographicSize),
				camTransform.position.z);
		}

		private void RedefineBounds() {
			limitBounds.x = 0;
			limitBounds.y = -(int)RoomsInfo.roomSize.y;
			limitBounds.width = (RoomsInfo.maxRoomCount * (int)RoomsInfo.roomSize.x) + ((RoomsInfo.maxRoomCount-1) * RoomLoaderSystem.roomSpacing);
			limitBounds.height = (int)RoomsInfo.roomSize.y * 2;
		}
	}
}