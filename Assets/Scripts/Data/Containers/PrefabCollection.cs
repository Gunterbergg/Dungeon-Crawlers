using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	/*
	[Serializable]
	public abstract class Prefab
	{
		public abstract GameObject GetPrefab();
	}

	[Serializable]
	public abstract class PrefabCollection : IEnumerable
	{

		public IEnumerator GetEnumerator() => getCollection().GetEnumerator();

		public abstract IEnumerable<Prefab> getCollection();

		public abstract GameObject getDefaultObject();

		public virtual GameObject findPrefabObject<PrefabType>(Predicate<PrefabType> predicate) where PrefabType : Prefab
			=> findPrefab<PrefabType>(predicate).GetPrefab();

		public virtual PrefabType findPrefab<PrefabType>(Predicate<PrefabType> predicate) where PrefabType : Prefab {
			foreach (PrefabType prefab in getCollection())
				if (predicate.Invoke(prefab)) return prefab;
			return null;
		}
	}*/
}
