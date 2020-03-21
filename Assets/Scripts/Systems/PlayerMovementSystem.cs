using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class PlayerMovementSystem : MonoBehaviour
	{
		public float speed = 4f;
		public UserView directionInput;

		private void Awake() => directionInput.GetInterface<IDirectionInput>().UserInput += (sneder, args) => Move(args.Data);

		private void Move(Vector2 dir) {
			if (dir.x > 0) GetComponent<SpriteRenderer>().flipX = false; else
			if (dir.x < 0) GetComponent<SpriteRenderer>().flipX = true;
			GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + (dir * Time.deltaTime * speed));
		}
	}
}