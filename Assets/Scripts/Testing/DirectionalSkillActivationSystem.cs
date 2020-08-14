using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class DirectionalSkillActivationSystem : MonoBehaviour
	{
		public EntityComponent entity;
		public string skillName = "default";
		
		public UserView inputView;

		public float cooldown = 1f;
		public float angleOffset = 0f;

		private bool onCooldown = false;
		private IDirectionInput inputHandler;
		private IProgressHandler cooldownHandler;

		protected virtual void Awake() {
			HandlersReferenceSetup();
		}

		public void Activate(Vector2 dir) {
			if (onCooldown) return;
			
			SkillComponent skill = entity.FindSkill(skillName);
			if (skill == null) return;

			Vector2 direction = Quaternion.AngleAxis(angleOffset, Vector2.up) * dir;

			Dictionary<string, object> skillParams = new Dictionary<string, object>();
			//skillParams.Add("weapon", entity.FindSkill("weapon").EquippedItem);
			skillParams.Add("animation", "Activate");
			skillParams.Add("direction", direction);
			skill.Activate(skillParams);
			
			onCooldown = true;
			cooldownHandler.Output(cooldown);
		}

		private void HandlersReferenceSetup() {
			//TODO add logging and exception handling
			if (inputView == null) return; 
			inputHandler = inputView.GetInterface<IDirectionInput>();
			cooldownHandler = inputView.GetInterface<IProgressHandler>();

			inputHandler.InputRelease += Activate;
			cooldownHandler.Completed += () => onCooldown = false;
		}
	}
}
