using DungeonCrawlers.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class DelayedHitboxSystem : MonoBehaviour
	{
		public float delayTime;
		public float activeTime;
		public float collisionDelay = 0.15f;
		
		private HitboxComponent hitbox;
		private List<HurtboxComponent> alredyCollided = new List<HurtboxComponent>();

		protected virtual void Awake() {
			HitboxReferenceSetup();
			StartCoroutine(ActivateHitbox());
		}

		protected virtual void OnTriggerEnter2D(Collider2D collider) => HandleCollision(collider);
		protected virtual void OnTriggerStay2D(Collider2D collider) => HandleCollision(collider);

		private void HitboxReferenceSetup() {
			hitbox = GetComponent<HitboxComponent>();
			if (hitbox == null) Destroy(gameObject);
		}

		private IEnumerator ActivateHitbox() {
			SetHitboxComponentsState(false);
			if (delayTime != 0f) yield return new WaitForSeconds(delayTime);
			
			SetHitboxComponentsState(true);
			if (activeTime == 0) yield break;
			
			yield return new WaitForSeconds(activeTime);
			Destroy(gameObject);
		}

		private void HandleCollision(Collider2D collider) {
			HurtboxComponent hurtbox = collider.GetComponent<HurtboxComponent>();
			if (hurtbox == null) {
				hitbox.RaiseHitEvent(collider.gameObject);
				return;
			}
			if (alredyCollided.Contains(hurtbox)) return;
		
			hitbox.collided.Add(hurtbox);
			hitbox.RaiseHitEvent(hurtbox);
			hurtbox.RaiseDamagedEvent(hitbox);

			alredyCollided.Add(hurtbox);
			StartCoroutine(RemoveCollided(hurtbox));
		}

		private void SetHitboxComponentsState(bool state) {
			foreach (Collider2D collider in hitbox.GetComponentsInChildren<Collider2D>())
				collider.enabled = state;
			foreach (SpriteRenderer renderer in hitbox.GetComponentsInChildren<SpriteRenderer>())
				renderer.enabled = state;
		}

		private IEnumerator RemoveCollided(HurtboxComponent hurtbox) {
			yield return new WaitForSeconds(collisionDelay);
			if (hitbox == null) yield break;
			alredyCollided.Remove(hurtbox);
			hitbox.collided.Remove(hurtbox);
		}
	}
}
