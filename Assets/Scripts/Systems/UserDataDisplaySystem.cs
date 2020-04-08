using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using System;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class UserDataDisplaySystem : MonoBehaviour 
	{
		public UserData userData;
		public UserView nameView;
		public UserView levelView;
		public UserView experienceView;
		public UserView goldView;
		public UserView gemsView;
		public UserView soulsView;

		protected virtual void Awake() {
			userData.OnChanged += Refresh;
			Refresh();
		}

		protected virtual void OnDestroy() => userData.OnChanged -= Refresh;

		protected void Refresh() {
			nameView.GetInterface<IOutputHandler<string>>()
				.Output(userData.Name);
			levelView.GetInterface<IOutputHandler<string>>()
				.Output(userData.Level.ToString());
			experienceView.GetInterface<IProgressBar>()
				.Output((float)userData.Current_exp / (float)Mathf.Max(userData.Next_level_exp, 1));
			goldView.GetInterface<IOutputHandler<string>>()
				.Output(userData.Gold.ToString());
			gemsView.GetInterface<IOutputHandler<string>>()
				.Output(userData.Gems.ToString());
			soulsView.GetInterface<IOutputHandler<string>>()
				.Output(userData.Souls.ToString());
		}
	}
}
