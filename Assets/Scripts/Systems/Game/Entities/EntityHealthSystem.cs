using System.Collections;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers
{
	public class EntityHealthSystem : MonoBehaviour
	{
		public EntityComponent entity;
		public UserView healthBarView;
		public float healthDisplayTime = 1f;

		private IProgressHandler healthBar;
		private Animator entityAnimator;
		private Coroutine displayHealthCoroutine;

		protected virtual void Awake() {
			HandlersReferenceSetup();
			ComponentsSetup();
			DisplayHealth();
		}

		public void OnHit(HitboxComponent hitbox) {
			if (hitbox.team == entity.team) return;
			float totalDamage = hitbox.GetTotalDamage();
			entity.currentHealth -= totalDamage;
			entityAnimator?.SetTrigger("hit");
			DisplayHealth();
		}

		public void DisplayHealth() {

			if (entity.currentHealth <= 0f) {
				entityAnimator?.SetBool("alive", false);
				return;
			}

			if (displayHealthCoroutine != null) StopCoroutine(displayHealthCoroutine);
			displayHealthCoroutine = StartCoroutine(EnableHealthbar(healthDisplayTime));

			healthBar?.Output(entity.currentHealth / entity.baseHealth);			
		}

		private IEnumerator EnableHealthbar(float time) {
			healthBarView.Activate();
			yield return new WaitForSeconds(time);
			healthBarView.DeActivate();
		}

		private void ComponentsSetup() {
			entity.Hit += OnHit;
			entityAnimator = entity.GetComponent<Animator>();
			healthBarView.DeActivate();
		}

		private void HandlersReferenceSetup() {
			if (healthBarView == null) return;
			healthBar = healthBarView.GetInterface<IProgressHandler>();
		}
	}
}
