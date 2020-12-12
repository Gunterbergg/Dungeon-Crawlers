using System.Collections;
using UnityEngine;

namespace DungeonCrawlers
{
	public class SlimeBehaviourSystem : MonoBehaviour
	{
		public EntityComponent entity;
		public float jumpDelay = 0.25f;
		public float jumpSpeed = 0.5f;
		public float jumpCooldown = 2f;
		public float maxJumpDistance = 2f;
		public float randomDelayGap = 0.4f;
		public int gooJumpsGap = 2;

		public GameObject jumpHitboxPrefab;
		public GameObject jumpGooPrefab;

		private Rigidbody2D entityRigidbody;
		private Animator entityAnimator;
		private SpriteRenderer entityRenderer;
		private float lastJumpTime = 0;
		private int gooJumpCount = 0;

		private bool isJumping = false;

		private GameObject enemyTarget;

		protected virtual void Awake() => ReferencesSetup();

		protected virtual void Start() => StartCoroutine(JumpBehaviour());

		protected virtual IEnumerator JumpBehaviour()
		{
			while (true) {
				if (!entity.behaviourEnabled) yield break;
				
				if (enemyTarget == null)
					while (!FindEnemy()) yield return null;
				
				if (Time.time >= lastJumpTime + jumpCooldown) {
					do { yield return Jump(); } while (isJumping);
				}
				yield return null;
			}
		}

		private IEnumerator Jump()
		{
			isJumping = true;
			entityAnimator.SetBool("jumping", true);
			Vector2 originalPos = transform.position;
			Vector2 deltaPos = ((Vector2)enemyTarget.transform.position - originalPos).normalized * maxJumpDistance;
			entityRenderer.flipX = deltaPos.x >= 0 ? false : true;
			Instantiate(jumpHitboxPrefab, transform);
			yield return new WaitForSeconds(jumpDelay);

			Vector2 targetPos = originalPos + deltaPos;
			for (float currJumpTime = 0f; currJumpTime <= 1f; currJumpTime += Time.deltaTime / jumpSpeed)
			{
				entityRigidbody.MovePosition(Vector2.Lerp(originalPos, targetPos, currJumpTime));
				yield return null;
			}
			if (++gooJumpCount >= gooJumpsGap) {
				Instantiate(jumpGooPrefab, targetPos, Quaternion.identity);
				gooJumpCount = 0;
			}
			entityAnimator.SetBool("jumping", false);
			lastJumpTime = Time.time - Random.Range(0, randomDelayGap);
			isJumping = false;
		}

		private bool FindEnemy()
		{
			enemyTarget = GameObject.FindGameObjectWithTag("Player");
			return enemyTarget != null;
		}

		private void ReferencesSetup()
		{
			//TODO add logging and exception handling
			entityRigidbody = entity.GetComponent<Rigidbody2D>();
			entityAnimator = entity.GetComponent<Animator>();
			entityRenderer = entity.GetComponent<SpriteRenderer>();
		}
	}
}
