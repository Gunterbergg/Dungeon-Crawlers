using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "EntityMetaCollection", menuName = "DungeonCrawlers/EntityMetaCollection")]
	public class EntityMetaCollection : ScriptableObject, IEnumerable<EntityMeta>
	{
		public EntityMeta defaultEntityMeta;
		public List<EntityMeta> entitiesMeta;

		public EntityMeta this[EntityType type] 
		{ get => entitiesMeta.Find((meta) => meta.type == type); }

		public GameObject this[EntityType type, int level]
		{ get => entitiesMeta.Find((meta) => meta.type == type).GetPrefabByLevel(level); }

		public IEnumerator<EntityMeta> GetEnumerator() {
			return ((IEnumerable<EntityMeta>)entitiesMeta).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IEnumerable<EntityMeta>)entitiesMeta).GetEnumerator();
		}
	}
}
