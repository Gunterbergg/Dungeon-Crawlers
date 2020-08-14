using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	public class HitboxComponent : MonoBehaviour
	{
		public BoxCollisionInfo hitInfo;

		public List<HurtboxComponent> collided = new List<HurtboxComponent>();

		public event Action<HitboxComponent> OnHit;
		public event Action<HitboxComponent, object> OnHitObject;
		public event Action<HitboxComponent, HurtboxComponent> OnHitHurtbox;

		private void OnDestroy() {
			OnHit = null;
			OnHitObject = null;
			OnHitHurtbox = null;
		}

		public void RaiseHitEvent(object hitObject) {
			OnHit?.Invoke(this);
			OnHitObject?.Invoke(this, hitObject);
		}

		public void RaiseHitEvent(HurtboxComponent hurtBox) {
			OnHit?.Invoke(this);
			OnHitHurtbox?.Invoke(this, hurtBox);
		}
	}
}
