using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class UserDataDisplaySystem : MonoBehaviour 
	{
		public UserView nameView;
		public UserView levelView;
		public UserView experienceView;
		public UserView goldView;
		public UserView gemsView;
		public UserView soulsView;

		protected virtual void Awake() {
			ReferencesSetup();
		}

		public void Display() {
			if (Session.Instance == null || Session.Instance.user == null) return;

			nameView?.GetInterface<IOutputHandler<string>>()
				.Output(Session.Instance.user.Name);
			levelView?.GetInterface<IOutputHandler<string>>()
				.Output(Session.Instance.user.Level.ToString());
			experienceView?.GetInterface<IProgressHandler>()
				.Output((float)Session.Instance.user.CurrentExp / (float)Mathf.Max(Session.Instance.user.NextLevelExp, 1));
			goldView?.GetInterface<IOutputHandler<string>>()
				.Output(Session.Instance.user.Gold.ToString());
			gemsView?.GetInterface<IOutputHandler<string>>()
				.Output(Session.Instance.user.Gems.ToString());
			soulsView?.GetInterface<IOutputHandler<string>>()
				.Output(Session.Instance.user.Souls.ToString());
		}

		private void OnDestroy() {
			Session.Instance.SessionUserChanged -= Display;
			Session.Instance.user.OnChanged -= Display;
		}

		private void ReferencesSetup() {
			//TODO add logging and exception handling
			Session.Instance.SessionUserChanged += Display;

			if (Session.Instance.user != null) Session.Instance.user.OnChanged += Display;
		}
	}
}
