using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[Serializable]
	public struct DamageResistanceInfo
	{
		public DamageType damageType;
		public float baseResistance;
		public float resistance;
	}

	public class HurtboxComponent : MonoBehaviour
	{
		public EntityComponent owner;
		public List<DamageResistanceInfo> resistances;
		
		public Action<HurtboxComponent, BoxCollisionInfo> Hit;

		public void RaiseDamagedEvent(BoxCollisionInfo collisionInfo) {
			Hit?.Invoke(this, collisionInfo);
		}
	}
}
