using UnityEngine;

namespace DungeonCrawlers.Data
{
	public class EntityComponent : MonoBehaviour
	{
		public Entity data;
		public float baseHealth = 100f;
		public float currentHealth = 100f;
		public ComponentCollection hurtboxes;
		public ComponentCollection skills;
		public ComponentCollection activeEffects;

		public SkillComponent FindSkill(string name) => 
			skills.Find<SkillComponent>((skill) => skill.name == name);
	}
}
