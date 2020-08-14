using UnityEngine;

namespace DungeonCrawlers.Data
{
	public abstract class EffectComponent<Effect> : MonoBehaviour where Effect : EffectComponent<Effect>
	{
		public abstract void Add(Effect effect);
	}
}
