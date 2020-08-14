using System.Collections;
using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class EntityHealthSystem : MonoBehaviour
	{
		public EntityComponent entity;
		public UserView healthBarView;
		public UserView damageView;
		public float healthDisplayTime = 1f;

		private IProgressHandler healthBar;
		private IOutputHandler<float> damageOutput;
		private Animator entityAnimator;
		private Coroutine displayHealthCoroutine;

		protected virtual void Awake() {
			HandlersReferenceSetup();
			ComponentsSetup();
			DisplayHealth();
		}

		public void ApplyDamage(HurtboxComponent hurtbox, BoxCollisionInfo collisionInfo) {
			float totalDamage = collisionInfo.GetTotalDamage();
			entity.currentHealth -= totalDamage;
			damageOutput?.Output(totalDamage);
			entityAnimator?.SetTrigger("hit");
			DisplayHealth();
		}

		public void DisplayHealth() {
			if (displayHealthCoroutine != null) StopCoroutine(displayHealthCoroutine);
			displayHealthCoroutine = StartCoroutine(EnableHealthbar(healthDisplayTime));

			healthBar?.Output(entity.currentHealth / entity.baseHealth);			

			if (entity.currentHealth <= 0f) { 
				entityAnimator?.SetBool("dead", true);
				Destroy(entity.gameObject, 1.5f);
			}
		}

		private IEnumerator EnableHealthbar(float time) {
			healthBarView.Activate();
			yield return new WaitForSeconds(time);
			healthBarView.DeActivate();
		}

		private void SubscribeHitEvent(ComponentCollection collection) {
			collection.ForEach<HurtboxComponent>(
				(hurtbox) => hurtbox.Hit += ApplyDamage);
		}

		private void ComponentsSetup() {
			entity.hurtboxes.CollectionChanged += SubscribeHitEvent;
			SubscribeHitEvent(entity.hurtboxes);
			entityAnimator = entity.GetComponent<Animator>();
			healthBarView.DeActivate();
		}

		private void HandlersReferenceSetup() {
			//TODO add logging and exception handlng
			if (healthBarView == null) return;
			healthBar = healthBarView.GetInterface<IProgressHandler>();
			if (damageView == null) return;
			damageOutput = damageView.GetInterface<IOutputHandler<float>>();
		}
	}
}
