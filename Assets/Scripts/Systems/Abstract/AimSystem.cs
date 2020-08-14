using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public abstract class AimSystem : MonoBehaviour
	{
		public float objectDistance = 1f;
		public float angleOffset = 0f;
		public bool handleAnchorFlip = true;

		private SpriteRenderer anchorRenderer;
		private SpriteRenderer aimObjectRenderer;
		private Transform anchorTransform;
		private Transform aimObjectTransform;

		protected abstract GameObject GetAnchorObject();
		protected abstract GameObject GetAimObject();

		protected virtual void Awake() {
			RefreshReferences();
		}

		protected virtual void Aim(Vector2 direction) {
			if (handleAnchorFlip)
				if (direction.normalized.x >= 0)
					anchorTransform.eulerAngles = new Vector3(anchorTransform.eulerAngles.x, 0f, anchorTransform.eulerAngles.z);
				else
					anchorTransform.eulerAngles = new Vector3(anchorTransform.eulerAngles.x, 180f, anchorTransform.eulerAngles.z);

			float orbitalAngle = DataUtility.GetAngle(direction) + angleOffset;
			aimObjectTransform.position = DataUtility.GetOrbitalPosition(anchorTransform.position, orbitalAngle, objectDistance); ;
			aimObjectTransform.eulerAngles = Vector3.forward * orbitalAngle;

			float zEuler = aimObjectTransform.eulerAngles.z;
			if (zEuler <= 90f || zEuler >= 270)
				aimObjectRenderer.sortingOrder = anchorRenderer.sortingOrder - 1;
			else
				aimObjectRenderer.sortingOrder = anchorRenderer.sortingOrder + 1;
		}

		protected virtual void RefreshReferences() {
			AnchorReferenceSetup();
			AimObjectReferenceSetup();
		}

		private void AnchorReferenceSetup() {
			GameObject anchorObject = GetAnchorObject();
			//TODO add logging and exception handling
			if (anchorObject == null) return;    
			anchorTransform = anchorObject.transform;
			anchorRenderer = anchorObject.GetComponent<SpriteRenderer>();
		}

		private void AimObjectReferenceSetup() {
			GameObject aimObject = GetAimObject();
			//TODO add logging and exception handling
			if (aimObject == null) return;   
			aimObjectTransform = aimObject.transform;
			aimObjectRenderer = aimObject.GetComponent<SpriteRenderer>();
		}
	}
}
