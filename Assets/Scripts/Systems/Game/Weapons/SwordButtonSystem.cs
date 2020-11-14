using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class SwordButtonSystem : MonoBehaviour
	{
		public UserView cooldownOutputView;
		public PlayerComponent player;
		public GameObject swordSlashPrefab;
		public GameObject swordCastPreview;

		public float swordDistance = 0.1f;
		public float swordCooldown = 0.8f;

		IProgressHandler cooldownOutput;
		float lastSlashTime = 0f;

		protected virtual void Awake() => ReferencesSetup();

		protected virtual void Update() => SlashPreview();

		public void Slash()
		{
			//TODO add logging and exception handling
			if (Time.time < lastSlashTime + swordCooldown) return;
			float angle = DataUtility.GetAngle(player.movementDirection);
			Instantiate(
				swordSlashPrefab,
				DataUtility.GetOrbitalPosition(player.@object.transform.position, angle, swordDistance),
				Quaternion.AngleAxis(angle, Vector3.forward));
			lastSlashTime = Time.time;
			cooldownOutput.Output(swordCooldown);
		}

		public void SlashPreview()
		{
			if (!player.isWalking || !player.isAlive) {
				DisableSlashPreview();
				return;
			}
			EnableSlashPreview();
			float angle = DataUtility.GetAngle(player.movementDirection);
			swordCastPreview.transform.position =
				DataUtility.GetOrbitalPosition(player.@object.transform.position, angle, swordDistance);
			swordCastPreview.transform.eulerAngles = Vector3.forward * angle;
		}

		private void EnableSlashPreview() => swordCastPreview.GetComponent<SpriteRenderer>().enabled = true;
		private void DisableSlashPreview() => swordCastPreview.GetComponent<SpriteRenderer>().enabled = false;

		private void ReferencesSetup() => cooldownOutput = cooldownOutputView.GetInterface<IProgressHandler>();
	}
}
