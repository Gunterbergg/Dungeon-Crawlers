using UnityEngine;
using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using System.Collections;

namespace DungeonCrawlers.Systems
{
    public class PlayerHealthSystem : MonoBehaviour
    {
        public PlayerComponent player;
        public UserView healthOutputView;
        public float healthDisplayTime = 1.5f;

        private IOutputHandler<int> healthOutput;
        private Animator playerAnimator;

        private Coroutine displayHealthCoroutine;

        protected virtual void Awake()
        {
            HealthSystemSetup();
            DisplayHealth();
        }

        private void OnHit(HurtboxComponent hurtbox, HitboxComponent hitbox)
        {
            if (hitbox.team == player.team) return;
            if (hitbox.hitboxDamage.GetTotalDamage() <= 0) return;
            playerAnimator.SetTrigger("hit");
            player.healthPoints = Mathf.Max(0, player.healthPoints - 1);
            playerAnimator.SetBool("alive", player.healthPoints > 0);
            DisplayHealth();
        }

        private void HealthSystemSetup()
        {
            healthOutput = healthOutputView.GetInterface<IOutputHandler<int>>();
            playerAnimator = player.GetComponent<Animator>();
            player.hurtboxes.ForEach<HurtboxComponent>((hurtbox) => hurtbox.Hit += OnHit);
        }

        public void DisplayHealth()
        {
            if (displayHealthCoroutine != null) StopCoroutine(displayHealthCoroutine);
            displayHealthCoroutine = StartCoroutine(EnableHealthbar(healthDisplayTime));
            healthOutput.Output(player.healthPoints);
        }

        private IEnumerator EnableHealthbar(float time)
        {
            healthOutputView.Activate();
            yield return new WaitForSeconds(time);
            healthOutputView.DeActivate();
        }
    }
}
