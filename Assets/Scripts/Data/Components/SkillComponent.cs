using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	public class SkillComponent : MonoBehaviour
	{
		new public string name = "default";
		public EntityComponent owner;
		public ComponentCollection activeHitboxes;

		public event Action<Dictionary<string, object>> Activated;
		public event Action<Dictionary<string, object>> BeforeActivate;

		public void Activate(Dictionary<string, object> skillInfo = null) {
			BeforeActivate?.Invoke(skillInfo);
			Activated?.Invoke(skillInfo);
		}
	}
}
