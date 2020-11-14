using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "EntityMeta", menuName = "DungeonCrawlers/EntityMeta")]
	public class EntityMeta : ScriptableObject
	{
		[Serializable]
		public struct EntityLevelPrefab
		{
			public int level;
			public GameObject prefab;
		}

		private const string MetaPath = "EntitiesMeta/";
		private const string DefaultMetaPath = MetaPath + "Default";

		public EntityType type;
		public string displayName;
		public string description;
		public GameObject selectablePrefab;

		public List<EntityLevelPrefab> prefabs;

		public static EntityMeta FindEntity(EntityType type) {
			foreach (EntityMeta meta in Resources.LoadAll<EntityMeta>(MetaPath))
				if (meta.type == type) return meta;
			return Resources.Load<EntityMeta>(DefaultMetaPath);
		}

		public GameObject GetPrefab()
			=> prefabs[0].prefab;

		public GameObject GetPrefabByLevel(int level)
			=> prefabs.Find((prefab) => prefab.level == level).prefab ?? GetPrefab();

	}
}
