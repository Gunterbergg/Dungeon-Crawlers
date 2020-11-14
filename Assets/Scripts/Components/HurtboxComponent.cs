using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public enum Team
	{
		Player, Entities
	}

	[Serializable]
	public struct DamageResistanceInfo
	{
		public DamageType damageType;
		public float baseResistance;
		public float resistance;
	}

	public class HurtboxComponent : MonoBehaviour
	{
		public ObjectComponent owner;
		public List<DamageResistanceInfo> resistances;
		
		public Action<HurtboxComponent, HitboxComponent> Hit;

		public void RaiseDamagedEvent(HitboxComponent collisionInfo) {
			Hit?.Invoke(this, collisionInfo);
		}
	}
}
