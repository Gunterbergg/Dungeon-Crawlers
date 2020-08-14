using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	class TouchCamera : CameraView
	{
		public UserView touchView;
		
		[SerializeField] private bool touchEnabled = false;
		
		private IInputHandler<List<Touch>> touchInput;

		public override bool Enabled { get => touchEnabled; set => touchEnabled = value; }

		protected override void Awake() {
			base.Awake();
			TouchCameraSetup();
		}

		private void TouchController(List<Touch> touches) {
			if (!Enabled) return;
			if (touches.Count == 1) CameraMovementController(touches[0]); else
			if (touches.Count == 2) CameraZoomController(touches[0], touches[1]);
		}

		protected void CameraMovementController(Touch touch) {
			Vector3 mov = targetCam.ScreenToWorldPoint(
				targetCam.WorldToScreenPoint(targetCam.transform.position) - (Vector3)touch.deltaPosition);
			MoveTo(mov);
		}

		protected void CameraZoomController(Touch touchZero, Touch touchOne) {
			float prevTouchDistance = (
				(touchZero.position - touchZero.deltaPosition) -
				(touchOne.position - touchOne.deltaPosition)).magnitude;
			float touchDistance = (touchZero.position - touchOne.position).magnitude;

			float deltaDistance = prevTouchDistance - touchDistance;
			ChangeSize(deltaDistance * Time.deltaTime);
		}

		private void TouchCameraSetup() {
			if (touchView == null) return;
			touchInput = touchView.GetInterface<IInputHandler<List<Touch>>>();
			touchInput.Input += TouchController;
		}
	}
}
