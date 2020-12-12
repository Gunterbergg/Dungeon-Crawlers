using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers
{
	[Serializable]
	public enum Team
	{
		Player, Entities
	}

	/*
	[Serializable]
	public struct DamageResistanceInfo
	{
		public DamageType damageType;
		public float baseResistance;
		public float resistance;
	}
	*/

	public class ObjectComponent : MonoBehaviour
	{
		public Team team;
		public GameObject @object;
		public List<Status> activeStatus = new List<Status>();

		//public List<DamageResistanceInfo> resistances;

		public Action<HitboxComponent> Hit;

		public T GetStatus<T>() where T : Status
		{
			T resultStatus = null;
			foreach (Status status in activeStatus) {
				if (!(status is T)) continue;
				if (resultStatus == null) {
					resultStatus = (T)status;
					continue;
				}
				resultStatus = (T)resultStatus.Accumulate(status);
			}
			return resultStatus;
		}

		public void RaiseDamagedEvent(HitboxComponent collisionInfo)
		{
			Hit?.Invoke(collisionInfo);
		}
	}
}
