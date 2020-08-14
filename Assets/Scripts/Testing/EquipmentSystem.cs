using DungeonCrawlers.Data;
	using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class EquipmentSystem : MonoBehaviour
	{
		public EntityComponent entity;

		protected virtual void Awake() {
			ComponentsReferenceSetup();
		}

		protected void UpdateSkillCollection(ComponentCollection equips) {
			entity.skills.ForEach<EquipSlotComponent>((slot) => {
				entity.skills.AddNodeComponents<SkillComponent>(slot.EquippedItem);
			});
		}

		private void ComponentsReferenceSetup() {
			//TODO add logging and handling exception
			if (entity == null || entity.skills == null) return;

			entity.skills.ForEach<EquipSlotComponent>((slot) => {
				if (slot.Owner != entity) slot.Owner = entity;
			});

			entity.skills.CollectionChanged += UpdateSkillCollection;
			UpdateSkillCollection(entity.skills);
		}
	}
}
