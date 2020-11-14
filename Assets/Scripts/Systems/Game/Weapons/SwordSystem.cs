using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class SwordSystem : MonoBehaviour
	{
		public UserView directionInputView;
		public UserView cooldownOutputView;

		public PlayerComponent player;

		public float swordDistance = 0.1f;
		public float swordCooldown = 0.8f;

		public GameObject swordCastPreview;
		public GameObject swordSlashPrefab;

		IDirectionInput directionInput;
		IProgressHandler cooldownOutput;
		Animator playerAnimator;

		private float lastSlashTime = 0f;

		protected virtual void Awake()
		{
			ReferencesSetup();
			DisableSlashPreview();
		}

		protected virtual void OnDestroy()
		{
			directionInput.InputStart -= EnableSlashPreview;
			directionInput.Input -= SlashPreview;
			directionInput.InputRelease -= DisableSlashPreview;
			directionInput.InputRelease -= Slash;
		}

		public void Slash(Vector2 dir)
		{
			//TODO add logging and exception 
			if (Time.time < lastSlashTime + swordCooldown) return;
			playerAnimator.SetTrigger("attack");
			float angle = DataUtility.GetAngle(dir);
			Instantiate(
				swordSlashPrefab,
				DataUtility.GetOrbitalPosition(player.@object.transform.position, angle, swordDistance),
				Quaternion.AngleAxis(angle, Vector3.forward));
			lastSlashTime = Time.time;
			cooldownOutput.Output(swordCooldown);
		}

		public void SlashPreview(Vector2 dir)
		{
			float angle = DataUtility.GetAngle(dir);
			swordCastPreview.transform.position =
				DataUtility.GetOrbitalPosition(player.@object.transform.position, angle, swordDistance);
			swordCastPreview.transform.eulerAngles = Vector3.forward * angle;
		}

		//Overload for event subscription
		private void EnableSlashPreview(Vector2 dir) => EnableSlashPreview();
		private void DisableSlashPreview(Vector2 dir) => DisableSlashPreview();

		private void EnableSlashPreview() => swordCastPreview.GetComponent<SpriteRenderer>().enabled = true;
		private void DisableSlashPreview() => swordCastPreview.GetComponent<SpriteRenderer>().enabled = false;

		private void ReferencesSetup()
		{
			cooldownOutput = cooldownOutputView.GetInterface<IProgressHandler>();
			directionInput = directionInputView.GetInterface<IDirectionInput>();
			directionInput.InputStart += EnableSlashPreview;
			directionInput.Input += SlashPreview;
			directionInput.InputRelease += DisableSlashPreview;
			directionInput.InputRelease += Slash;
			playerAnimator = player.GetComponent<Animator>();
		}
	}
}
