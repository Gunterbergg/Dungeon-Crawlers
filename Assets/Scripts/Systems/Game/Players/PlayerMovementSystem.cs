using UnityEngine;
using DungeonCrawlers.UI;
using DungeonCrawlers.Data;

namespace DungeonCrawlers.Systems
{
	public class PlayerMovementSystem : MonoBehaviour
	{
		public float speed = 4f;
		public UserView analogView;
		public PlayerComponent player;

		private SpriteRenderer playerRenderer;
		private Rigidbody2D playerRigidbody;
		private Animator playerAnimator;

		protected virtual void Awake()
		{
			HandlersReferenceSetup();
			PlayerReferenceSetup();
		}

		protected void Move(Vector2 dir)
		{
			if (!player.isAlive) return;
			playerRigidbody.MovePosition((Vector2)transform.position + (dir * Time.deltaTime * speed));
			player.movementDirection = dir.normalized;
			if (dir.x > 0) playerRenderer.flipX = false;
			if (dir.x < 0) playerRenderer.flipX = true;
		}

		private void HandlersReferenceSetup()
		{
			//TODO add logging and exception handling
			if (analogView == null) return;
			IDirectionInput directionHandler = analogView.GetInterface<IDirectionInput>();
			directionHandler.Input += Move;
			directionHandler.InputStart += OnInputStart;
			directionHandler.InputRelease += OnInputRelease;
		}

		protected void OnInputStart(Vector2 dir)
		{
			playerAnimator.SetBool("walking", dir != Vector2.zero);
			player.isWalking = true;
		}

		protected void OnInputRelease(Vector2 dir)
		{
			playerAnimator.SetBool("walking", false);
			player.isWalking = false;
		}

		private void PlayerReferenceSetup()
		{
			//TODO add logging and exception handling
			playerRigidbody = player.@object.GetComponent<Rigidbody2D>();
			playerAnimator = player.@object.GetComponent<Animator>();
			playerRenderer = player.@object.GetComponent<SpriteRenderer>();
		}
	}
}