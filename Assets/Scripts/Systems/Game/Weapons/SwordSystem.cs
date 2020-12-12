using System;
using System.Collections;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class SwordSystem : MonoBehaviour
	{
		public UserView directionInputView;
		public UserView cooldownOutputView;

		public PlayerComponent player;

		public float swordSlashDistance = 0.1f;
		public float swordCooldown = 0.8f;
		public float swordAngle = 130f;
		public float swordDistance = .4f;
		public float previewAngleOffset = 0f;

		public GameObject sword;
		public GameObject swordCastPreview;
		public GameObject swordSlashPrefab;

		IDirectionInput directionInput;
		IProgressHandler cooldownOutput;
		SpriteRenderer playerRenderer;
		Animator playerAnimator;

		private float lastSlashTime = 0f;
		private bool invertSwordSide = false;

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
			if (!player.isAlive) return;
			if (Time.time < lastSlashTime + swordCooldown) return;
			playerAnimator.SetTrigger("attack");
			float angle = DataUtility.GetAngle(dir);
			Instantiate(
				swordSlashPrefab,
				DataUtility.GetOrbitalPosition(player.@object.transform.position, angle, swordSlashDistance),
				Quaternion.AngleAxis(angle, Vector3.forward));
			lastSlashTime = Time.time;
			cooldownOutput.Output(swordCooldown);
			invertSwordSide = !invertSwordSide;
			SlashPreview(dir);
		}

		public void SlashPreview(Vector2 dir)
		{
			if (!player.isAlive) return;
			if (dir.x > 0) playerRenderer.flipX = false;
			if (dir.x < 0) playerRenderer.flipX = true;
			float angle = DataUtility.GetAngle(dir);
			//swordCastPreview.transform.position =
			//	DataUtility.GetOrbitalPosition(player.@object.transform.position, angle, swordDistance);
			swordCastPreview.transform.eulerAngles = Vector3.forward * (angle + previewAngleOffset);
			float calculatedAngle = invertSwordSide ? angle + swordAngle : angle - swordAngle;
			sword.transform.eulerAngles = Vector3.forward * (calculatedAngle);
			sword.transform.position =
				DataUtility.GetOrbitalPosition(player.@object.transform.position, calculatedAngle, swordDistance);
		}

		//Overload for event subscription
		private void EnableSlashPreview(Vector2 dir) => EnableSlashPreview();
		private void DisableSlashPreview(Vector2 dir) => DisableSlashPreview();

		private void EnableSlashPreview()
		{
			if (!player.isAlive) return;
			swordCastPreview.GetComponent<SpriteRenderer>().enabled = true;
			sword.GetComponent<SpriteRenderer>().enabled = true;
		}
		private void DisableSlashPreview() 
		{
			StartCoroutine(DelayedAction(.2f, () => { 
				swordCastPreview.GetComponent<SpriteRenderer>().enabled = false;
				sword.GetComponent<SpriteRenderer>().enabled = false;
			}));
		}

		private IEnumerator DelayedAction(float time, Action action)
		{
			yield return new WaitForSeconds(time);
			action.Invoke();
		}

		private void ReferencesSetup()
		{
			cooldownOutput = cooldownOutputView.GetInterface<IProgressHandler>();
			directionInput = directionInputView.GetInterface<IDirectionInput>();
			directionInput.InputStart += EnableSlashPreview;
			directionInput.Input += SlashPreview;
			directionInput.InputRelease += DisableSlashPreview;
			directionInput.InputRelease += Slash;
			playerAnimator = player.GetComponent<Animator>();
			playerRenderer = player.GetComponent<SpriteRenderer>();
		}
	}
}
