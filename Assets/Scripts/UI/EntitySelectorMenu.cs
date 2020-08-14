using System;
using DungeonCrawlers.Data;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public class EntitySelectorMenu : UserView, IInputHandler<EntityMeta>
	{
		public EntityMetaCollection entities;
		public Transform selectableContainer;
		public bool filterUserEntities = true;

		private EntityMeta selected;

		public bool InputEnabled { get; set; }

		public event Action<EntityMeta> Input;
		
		protected override void Awake() {
			base.Awake();
			SetupSelectables();
		}

		public void SetupSelectables() {
			foreach(EntityMeta meta in entities) {
				if (filterUserEntities && Session.Instance.user.Entities.Exists((entity) => entity.Type == meta.type)) continue;
				UserView newSelectableView = Instantiate(meta.selectablePrefab, selectableContainer).GetComponent<UserView>();
				ISelectable selectable = newSelectableView.GetInterface<ISelectable>();
				selectable.Value = meta;
				selectable.Input += OnSelected;
			}
		}

		private void OnSelected(UnityEngine.Object selected) {
			this.selected = (EntityMeta)selected;
			Input?.Invoke(this.selected);
		}
	}
}
