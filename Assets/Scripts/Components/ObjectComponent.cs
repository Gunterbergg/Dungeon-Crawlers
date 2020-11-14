using UnityEngine;

namespace DungeonCrawlers.Data
{
	public class ObjectComponent : MonoBehaviour
	{
		public Team team;
		public GameObject @object;
		public ComponentCollection hurtboxes;
		public ComponentCollection activeEffects;
	}
}
