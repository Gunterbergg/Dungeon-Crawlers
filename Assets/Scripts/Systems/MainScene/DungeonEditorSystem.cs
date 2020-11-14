using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using UnityEngine;
using UnityEngine.Events;

namespace DungeonCrawlers.Systems
{
	public class DungeonEditorSystem : MonoBehaviour
	{
		public UserView cameraView;

		public UserView GameObjectSelector;

		public UserView entityInput;

		public CanvasGroup entityEditorOptions;

		public UserView roomSelectorView;
		public UserView entitySelectorView;
		public UserView entityUpgraderView;

		public UnityEvent OnEnable;
		public UnityEvent OnDisable;

		[SerializeField] private bool editorEnabled = false;

		private ComponentCollection entities;
		private EntityComponent selectedEntity;

		protected virtual void Awake() {
			DungeonEditorSetup();
		}

		public void EnableEditor() {
			if (editorEnabled) return;
			editorEnabled = true;
			OnEnable.Invoke();
		}

		public void DisableEditor() {
			if (!editorEnabled) return;
			editorEnabled = false;
			OnDisable.Invoke();
		}

		public void OnObjectSelected(GameObject selectedObject) {
			if (!editorEnabled) EnableEditor();
			entities = DataUtility.GetData<ComponentCollection>(selectedObject);
			cameraView.GetInterface<ICameraHandler>().LimitBounds = DataUtility.GetObjectRectBounds(selectedObject);
		}

		public void OnEntityInput(EntityComponent entity) {
			selectedEntity = entity;
			entityEditorOptions.interactable = true;
		}

		public void UnselectEntity() {
			entityEditorOptions.interactable = false;
		}

		public void ChangeRoomType() {

		}

		public void ChangeEntityType(EntityMeta otherMeta) {
			if (otherMeta.type == selectedEntity.data.Type) return;
			selectedEntity.data.Type = otherMeta.type;
			Entity dataReference = selectedEntity.data;

			entities.Remove(selectedEntity);
			GameObject newEntityObject = Instantiate(
				otherMeta.GetPrefabByLevel(
					Session.Instance.user.EntitiesInfo.Find((meta) => meta.type == otherMeta.type).level));
			EntityComponent entity = DataUtility.GetData<EntityComponent>(newEntityObject);
			entity.data = dataReference;

			entities.Add(entity);
			selectedEntity = entity;
		}

		public void UpgradeEntity() {

		}

		private void DungeonEditorSetup() {
			GameObjectSelector.GetInterface<IInputHandler<GameObject>>().Input += OnObjectSelected;
			entityInput.GetInterface<IInputHandler<EntityComponent>>().Input += OnEntityInput;
			entitySelectorView.GetInterface<IInputHandler<EntityMeta>>().Input += ChangeEntityType;
		}
	}
}
