using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public enum DamageType
	{
		All, Heat, Cold, Electric, Acid
	}

	[Serializable]
	public struct Damage
	{
		public DamageType damageType;
		public float damage;
		//public float critChance;

		public Damage(DamageType damageType, float damage)
		{
			this.damageType = damageType;
			this.damage = damage;
		}

		public static Damage operator +(Damage damageInfoA, Damage damageInfoB)
		{
			if (damageInfoA.damageType != damageInfoB.damageType) return damageInfoA;
			damageInfoA.damage += damageInfoB.damage;
			return damageInfoA;
		}

		public static float operator -(Damage damageInfo, DamageResistanceInfo resistanceInfo)
		{
			if (damageInfo.damageType != resistanceInfo.damageType) return damageInfo.damage;
			return (damageInfo.damage - resistanceInfo.baseResistance) * (1f - resistanceInfo.resistance);
		}
	}

	[Serializable]
	public struct HitboxDamage
	{
		public List<Damage> damages;
		public ComponentCollection effects;

		public static HitboxDamage operator +(HitboxDamage boxCollisionA, HitboxDamage boxCollisionB)
		{
			boxCollisionA.AddDamages(boxCollisionB.damages);
			boxCollisionA.effects.Add((IEnumerable<MonoBehaviour>)boxCollisionB.effects);
			return boxCollisionA;
		}

		public void AddDamage(Damage damage) => damages.Add(damage);
		public void AddDamages(params Damage[] damages) => AddDamages(damages);
		public void AddDamages(IEnumerable<Damage> damages)
		{
			foreach (Damage damageInfo in damages)
				AddDamage(damageInfo);
		}

		public IEnumerable<Damage> GetDamages()
		{
			List<DamageType> calculated = new List<DamageType>();
			foreach (Damage damage in damages) {
				if (calculated.Contains(damage.damageType)) continue;
				calculated.Add(damage.damageType);
				yield return this[damage.damageType];
			}
		}

		public float GetTotalDamage()
		{
			float total = 0f;
			foreach (Damage damage in GetDamages()) total += damage.damage;
			return total;
		}

		public Damage this[DamageType type] {
			get {
				Damage resultDamageInfo = new Damage(type, 0f);
				foreach (Damage damage in damages.FindAll((damageInfo) => damageInfo.damageType == type))
					resultDamageInfo += damage;
				return resultDamageInfo;
			}
			set => damages.Add(value);
		}
	}

	public class HitboxComponent : MonoBehaviour
	{
		public Team team;
		public HitboxDamage hitboxDamage;

		public List<HurtboxComponent> collided = new List<HurtboxComponent>();

		public event Action<HitboxComponent> OnHit;
		public event Action<HitboxComponent, object> OnHitObject;
		public event Action<HitboxComponent, HurtboxComponent> OnHitHurtbox;

		private void OnDestroy() {
			OnHit = null;
			OnHitObject = null;
			OnHitHurtbox = null;
		}

		public void RaiseHitEvent(object hitObject) {
			OnHit?.Invoke(this);
			OnHitObject?.Invoke(this, hitObject);
		}

		public void RaiseHitEvent(HurtboxComponent hurtBox) {
			OnHit?.Invoke(this);
			OnHitHurtbox?.Invoke(this, hurtBox);
		}
	}
}
