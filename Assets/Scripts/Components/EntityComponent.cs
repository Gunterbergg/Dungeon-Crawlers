using DungeonCrawlers.Data;

namespace DungeonCrawlers
{
	public class EntityComponent : ObjectComponent
	{
		public Entity data;
		public float baseHealth = 100f;
		public float currentHealth = 100f;
		public bool behaviourEnabled = true;
	}
}
