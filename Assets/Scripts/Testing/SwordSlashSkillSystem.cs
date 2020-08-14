using System;
using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class SwordSlashSkillSystem : MonoBehaviour
	{
		public SkillComponent swordSkill;
		public GameObject hitboxPrefab;
		public float defaultDistance = 0.1f;

		private Dictionary<string, object> currentSkillParams = new Dictionary<string, object>();

		protected virtual void Awake() {
			ComponentsReferenceSetup();
		}

		public void Slash(Dictionary<string, object> skillParams) {
			//TODO add logging and exception handling
			if (swordSkill == null || swordSkill.owner == null) return;
			currentSkillParams = skillParams;
			HandleParam<GameObject>("weapon", (weapon) => {
				HandleParam<Vector2>("direction", (direction) => {
					string animation = GetParam<string>("animation") ?? "Activate";
					float distance = ParamIsValid<float>("distance") ? GetParam<float>("distance") : defaultDistance;

					weapon.GetComponent<Animator>()?.SetTrigger(animation);
					SetWeaponPosition(weapon, direction, distance);

				});
			});

			swordSkill.activeHitboxes.AddNodeComponents<HitboxComponent>(
				Instantiate(hitboxPrefab, swordSkill.activeHitboxes.transform));
			swordSkill.activeHitboxes.ForEach((component) => {
				HitboxComponent hitboxData = component as HitboxComponent;
				hitboxData.OnHitHurtbox -= DealDamage;
				hitboxData.OnHitHurtbox += DealDamage;
			});
		}

		protected void DealDamage(HitboxComponent hitbox, HurtboxComponent hurtbox) {
			hurtbox.RaiseDamagedEvent(hitbox.hitInfo);
		}

		protected void SetWeaponPosition(GameObject weapon, Vector3 direction, float distance) {
			float angle = DataUtility.GetAngle(direction);
			weapon.transform.position = DataUtility.GetOrbitalPosition(swordSkill.owner.transform.position, angle, distance);
			weapon.transform.eulerAngles = Vector3.forward * angle;
		}

		private void ComponentsReferenceSetup() {
			//TODO add logging and exception handling
			if (swordSkill == null) return;
			swordSkill.Activated += Slash;
		}

		//TODO add inheritance to AttackSystem with these methods
		private void HandleParam<Type>(string param, Action<Type> action) {
			//TODO add logging and exception handling
			if (ParamIsValid<Type>(param))
				action.Invoke((Type)currentSkillParams[param]);
		}

		private Type GetParam<Type>(string param) {
			if (ParamIsValid<Type>(param))
				return (Type)currentSkillParams[param];
			return default;
		}

		private bool ParamIsValid<Type>(string param) {
			return currentSkillParams.ContainsKey(param) && (currentSkillParams[param] is Type);
		}
	}
}
