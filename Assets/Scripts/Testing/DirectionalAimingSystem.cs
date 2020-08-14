using System.Collections;
using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class DirectionalAimingSystem : AimSystem
	{
		public UserView inputView;
		public EntityComponent entity;
		public string equipSlotName = "default";

		public bool visibleOnInputOnly = false;

		private IDirectionInput inputHandler;
		private GameObject equip;

		protected override void Awake() {
			ComponentsReferenceSetup();
			HandlersReferenceSetup();
			base.Awake();
		}

		protected override void RefreshReferences() {
			base.RefreshReferences();
			Aim(Vector2.up);
		}

		protected override GameObject GetAnchorObject() {
			//TODO add logging and exception handling
			return entity.gameObject;
		}

		protected override GameObject GetAimObject() {
			//TODO add logging and exception handling
			return equip;
		}

		private void TurnVisible() => equip.GetComponent<Renderer>().enabled = true;
		private void TurnInvisible() => equip.GetComponent<Renderer>().enabled = false;

		private void VisibleOnInput(Vector2 dir) {
			TurnVisible();

			inputHandler.InputRelease += (vec2) => {
				inputHandler.Input += VisibleOnInput;
				TurnInvisible();
			};

			inputHandler.Input -= VisibleOnInput;
		}

		private void HandlersReferenceSetup() {
			//TODO add logging and exception handling
			if (inputView == null) return;       
			inputHandler = inputView.GetInterface<IDirectionInput>();
			inputHandler.Input += Aim;

			if (visibleOnInputOnly) {
				TurnInvisible();
				inputHandler.Input += VisibleOnInput;
			}
		}

		private void ComponentsReferenceSetup() {
			//TODO add logging and exception handling
			if (entity == null) return;

			EquipSlotComponent equipSlot = null;
			if (equipSlot.EquippedItem != null) equip = equipSlot.EquippedItem;

			equipSlot.EquipmentChanged += () => {
				equip = equipSlot.EquippedItem;
				RefreshReferences();
			};
		}
	}
}
