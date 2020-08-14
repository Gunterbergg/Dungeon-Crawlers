using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class DirectionalMovementSystem : MonoBehaviour
	{
		public float speed = 4f;
		public EntityComponent entity;
		public UserView inputView;
		public bool movementEnabled = true;

		private Rigidbody2D playerRigidbody;

		protected virtual void Awake() {
			HandlersReferenceSetup();
			PlayerReferenceSetup();
		}

		protected void Move(Vector2 dir) {
			if (!movementEnabled) return;
			playerRigidbody.MovePosition((Vector2)transform.position + (dir * Time.deltaTime * speed));
		}

		private void HandlersReferenceSetup() {
			//TODO add logging and exception handling
			if (inputView == null) return;
			inputView.GetInterface<IDirectionInput>().Input += Move;
		}

		private void PlayerReferenceSetup() {
			//TODO add logging and exception handling
			if (entity == null) return;
			playerRigidbody = entity.GetComponent<Rigidbody2D>();
		}
	}
}
