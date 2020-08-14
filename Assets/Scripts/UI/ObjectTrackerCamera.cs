using DungeonCrawlers.Data;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public class ObjectTrackerCamera : CameraView
	{
		public bool forceCamera = false;
		public ComponentCollection focusObjects;

		private Rect focusBounds;
		private bool resetBounds = true;

		protected override void Awake() {
			base.Awake();
			ObjectTrackerSetup();
		}

		private void LateUpdate() {
			EncapsulateFocusObjects();
		}

		protected virtual void EncapsulateFocusObjects() {
			bool hasRenderer = false;
			resetBounds = true;
			foreach (MonoBehaviour obj in focusObjects) {
				Renderer objRenderer = obj.GetComponent<Renderer>();
				if (objRenderer == null) continue;
				EncapsulateRenderer(objRenderer);
				hasRenderer = true;
			}

			if (!hasRenderer) return; 

			bool isLandscape = focusBounds.width / targetCam.aspect > focusBounds.height;
			if (isLandscape) focusBounds.height = focusBounds.width / targetCam.aspect;
			else focusBounds.width = focusBounds.height * targetCam.aspect;
			LimitBounds = focusBounds;
			LimitBoundsActive = true;

			if (forceCamera) MaxSize();
		}

		protected virtual void EncapsulateRenderer(Renderer renderer) {
			if (resetBounds) {
				focusBounds = AddPadding(DataUtility.GetObjectRectBounds(renderer.gameObject));
				resetBounds = false;
			} else {
				focusBounds = AddPadding(
					focusBounds.EncapsulateRect(DataUtility.GetObjectRectBounds(renderer.gameObject)));
			}
		}

		private void ObjectTrackerSetup() {
			if (focusObjects != null)
				focusObjects.CollectionChanged += (objs) => EncapsulateFocusObjects();
		}
	}
}
