using DungeonCrawlers.Data;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class EntityInstantiatorSystem : MonoBehaviour
	{
		//Instatiation Components
		public ComponentCollection entities;
		public EntityMetaCollection entitiesMeta;

		protected virtual void Awake() {
			if (enabled) InstatiateEntities();
			EntityInstantiatorSetup();
		}

		public void OnDestroy() {
			Session.Instance.user.OnPropertyChanged -= OnDataPropertyChanged;
		}

		public void InstatiateEntities() {
			foreach (EntityComponent entity in InstantiateEntities(Session.Instance.user.Entities)) {
				entity.transform.position = 
					entities.transform.position + new Vector3(entity.data.XPos, entity.data.YPos);
				entities.Add(entity);
			}
		}

		public void DestroyEntities() {
			entities?.ForEach<EntityComponent>(
				(entity) => Destroy(entity.gameObject));
		}

		public void ReinstantiateEntities() {
			DestroyEntities();
			InstatiateEntities();
		}

		protected IEnumerable<EntityComponent> InstantiateEntities(IEnumerable<Entity> entities) {
			foreach (Entity entity in entities) 
				yield return InstantiateEntity(entity);
		}

		protected EntityComponent InstantiateEntity(Entity entity) {
			EntityInfo entityInfo = Session.Instance.user.EntitiesInfo.Find((info) => info.type == entity.Type);
			GameObject entityPrefab = entitiesMeta[entity.Type, entityInfo.level];
			DataUtility.GetData<EntityComponent>(entityPrefab).data = entity;

			return DataUtility.GetData<EntityComponent>(Instantiate(entityPrefab, transform));
		}

		private void EntityInstantiatorSetup() {
			if (entities == null) return;
			Session.Instance.user.OnPropertyChanged += OnDataPropertyChanged;
			entities.Added += EntityAdded;
			entities.Removed += EntityRemoved;
		}

		private void OnDataPropertyChanged(string propertyName) {
			if (propertyName != "Entities") return;
			ReinstantiateEntities();
		}

		private void EntityAdded(MonoBehaviour entity) {
			EntityComponent entityC = DataUtility.GetData<EntityComponent>(entity.gameObject);
			entity.transform.SetParent(transform);
			entity.transform.localPosition = new Vector2(entityC.data.XPos, entityC.data.YPos);
		}

		private void EntityRemoved(MonoBehaviour entity) {
			Destroy(entity.gameObject);
		}
	}
}