using DungeonCrawlers.Data;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public class EntitySelector : SelectorView<EntityComponent> 
	{
		public ComponentCollection entities;

		protected override IEnumerable<EntityComponent> GetCollection()
			=> EntitiesEnumerator();

		protected override bool IntersectsWith(EntityComponent entity, Vector2 cursorPosition)
			=> GetSpriteRect(entity.GetComponent<SpriteRenderer>()).Contains(cursorPosition);

		private Rect GetSpriteRect(SpriteRenderer sprite)
			=> new Rect() { size = sprite.bounds.size, center = sprite.bounds.center };

		private IEnumerable<EntityComponent> EntitiesEnumerator() {
			foreach (EntityComponent entity in entities) yield return entity;
		}
	}
}