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
	public struct DamageInfo
	{
		public DamageType damageType;
		public float damage;
		//public float critChance;

		public DamageInfo(DamageType damageType, float damage) {
			this.damageType = damageType;
			this.damage = damage;
		}

		public static DamageInfo operator +(DamageInfo damageInfoA, DamageInfo damageInfoB) {
			if (damageInfoA.damageType != damageInfoB.damageType) return damageInfoA;
			damageInfoA.damage += damageInfoB.damage;
			return damageInfoA;
		}
		
		public static float operator -(DamageInfo damageInfo, DamageResistanceInfo resistanceInfo) {
			if (damageInfo.damageType != resistanceInfo.damageType) return damageInfo.damage;
			return (damageInfo.damage - resistanceInfo.baseResistance) * (1f - resistanceInfo.resistance);
		}
	}

	[Serializable]
	public struct BoxCollisionInfo
	{
		public List<DamageInfo> damages;
		public ComponentCollection effects;
		public Vector2 pullForceOffset;
		public Vector2 pullForce;

		public static BoxCollisionInfo operator +(BoxCollisionInfo boxCollisionA, BoxCollisionInfo boxCollisionB) {
			boxCollisionA.AddDamages(boxCollisionB.damages);
			boxCollisionA.effects.Add((IEnumerable<MonoBehaviour>)boxCollisionB.effects);
			boxCollisionA.pullForce += boxCollisionB.pullForce;
			boxCollisionA.pullForceOffset += boxCollisionB.pullForceOffset;
			return boxCollisionA;
		}

		public void AddDamage(DamageInfo damage) => damages.Add(damage);
		public void AddDamages(params DamageInfo[] damages) => AddDamages(damages);
		public void AddDamages(IEnumerable<DamageInfo> damages) {
			foreach (DamageInfo damageInfo in damages)
				AddDamage(damageInfo);
		}

		public IEnumerable<DamageInfo> GetDamages() {
			List<DamageType> calculated = new List<DamageType>();
			foreach (DamageInfo damage in damages) {
				if (calculated.Contains(damage.damageType)) continue;
				calculated.Add(damage.damageType);
				yield return this[damage.damageType];
			}
		}

		public float GetTotalDamage() {
			float total = 0f;
			foreach (DamageInfo damage in GetDamages()) total += damage.damage;
			return total;
		}

		public DamageInfo this[DamageType type] {
			get {
				DamageInfo resultDamageInfo = new DamageInfo(type, 0f);
				foreach (DamageInfo damage in damages.FindAll((damageInfo) => damageInfo.damageType == type))
					resultDamageInfo += damage;
				return resultDamageInfo;
			}
			set => damages.Add(value);
		}
	}
}
