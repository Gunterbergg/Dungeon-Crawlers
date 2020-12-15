using System.Collections;
using UnityEngine;

namespace DungeonCrawlers
{
    public class SkeletonBehaviourSystem : MonoBehaviour
    {
        public EntityComponent entity;

        public GameObject attackHitboxPrefab;
        public float hitboxDistance = 0.1f;
        public float attackRange = 0.3f;
        public float attackDelay = 0.3f;
        public float attackRecoveryTime = 1.2f;
        public float attackCooldown = 0.5f;
        public float moveSpeed = 4.8f;
        public float hitStunTime = 0.2f;

        private GameObject enemyTarget;
        private float lastAttackTime = 0f;
        private bool isAttacking = false;
        private bool isStunned = false;

        private Rigidbody2D entityRigidbody;
        private Animator entityAnimator;
        private SpriteRenderer entityRenderer;

        protected virtual void Awake() => ReferencesSetup();

        protected virtual void Start() => StartCoroutine(HostileBehaviour());

        private IEnumerator HostileBehaviour()
        {
            while (true) {
                if (!entity.behaviourEnabled) { yield return null; continue; }
                
                if (enemyTarget == null) {
                    entityAnimator.SetBool("moving", false);
                    while (!FindEnemy()) { yield return null; continue; }                
                }
                if (isStunned) { yield return null; continue; }
                if (Vector3.Distance(transform.position, enemyTarget.transform.position) > attackRange) {
                    entityAnimator.SetBool("moving", true);
                    Move();
                    yield return null;
                    continue;
                }
                entityAnimator.SetBool("moving", false);
                if (Time.time >= lastAttackTime + attackCooldown) {
                    do { yield return Attack(); } while (isAttacking);
                    continue;
                }                
                yield return null;
            }
        }

        private IEnumerator Attack()
        {
            isAttacking = true;
            entityAnimator.SetTrigger("attack");
            Vector3 targetDirection = (enemyTarget.transform.position - transform.position).normalized;
            Vector3 hitboxPosition = transform.position + targetDirection * hitboxDistance;
            yield return new WaitForSeconds(attackDelay);
            lastAttackTime = Time.time;
            if (isStunned) {
                isAttacking = false;
                yield break;
            }
            Instantiate(attackHitboxPrefab, hitboxPosition, Quaternion.LookRotation(targetDirection, Vector3.up));
            yield return new WaitForSeconds(attackRecoveryTime);
            isAttacking = false;
        }

        private void Move()
        {
            Vector3 targetDir = (enemyTarget.transform.position - transform.position).normalized;
            Vector3 moveVector = targetDir * moveSpeed * Time.deltaTime;
            entityRigidbody.MovePosition(transform.position + moveVector);
            entityRenderer.flipX = targetDir.x < 0;
        }

        private bool FindEnemy()
        {
            enemyTarget = GameObject.FindGameObjectWithTag("Player");
            return enemyTarget != null;
        }

        private IEnumerator Stun()
        {
            isStunned = true;
            yield return new WaitForSeconds(hitStunTime);
            isStunned = false;
        }

        private void ReferencesSetup()
        {
            //TODO add logging and exception handling
            entityRigidbody = entity.GetComponent<Rigidbody2D>();
            entityAnimator = entity.GetComponent<Animator>();
            entityRenderer = entity.GetComponent<SpriteRenderer>();
            entity.Hit += (hit) => StartCoroutine(Stun());
        }
    }
}