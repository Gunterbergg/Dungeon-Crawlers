using System;
using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawlers.UI
{
	public class DialogBoxDisplay : UserView, IDialogBox<EventArgs>
	{
		public UserView dialogPrefab;
		public Transform dialogsContainer;
		public Button closeButton;
		public List<UserView> dialogs = new List<UserView>();

		public event EventHandler<EventArgs> Closed;
		public event EventHandler<EventArgs> UserInput;

		protected override void Awake() {
			base.Awake();
			closeButton.onClick.AddListener(() => UserInput?.Invoke(this, EventArgs.Empty));
			UserInput += (sender, eventArgs) => Close();
			foreach (UserView dialog in dialogsContainer.GetComponentsInChildren<UserView>())
				AddDialog(dialog);
		}

		public void Clear() {
			List<UserView> copyList = new List<UserView>(dialogs);
			foreach (UserView dialogReference in copyList)
				dialogReference.GetInterface<IDialogBox<EventArgs>>().Close();
		}

		public void Close() {
			Clear();	
			DeActivate();
			Closed?.Invoke(this, EventArgs.Empty);
		}

		public void Output(DialogBoxOutput output) {
			UserView newDialogBox = Instantiate(dialogPrefab, dialogsContainer) as UserView;
			newDialogBox.GetInterface<IDialogBox<EventArgs>>()?.Output(output);
			AddDialog(newDialogBox);
			Activate();
		}

		public void OutputDefault() {
			Output(
				new DialogBoxOutput(
					LanguagePack.GetString("alert"),
					LanguagePack.GetString("error")
				));
		}

		public void AddDialog(UserView newDialog) {
			IDialogBox<EventArgs> castDialog = newDialog.GetInterface<IDialogBox<EventArgs>>();
			if (castDialog == null)
				return;
			dialogs.Add(newDialog);
			castDialog.Closed += (sender, eventArgs) => OnDialogClose(sender as UserView);
		}

		public void RemoveDialog(UserView dialog) {
			if (!dialogs.Contains(dialog))
				return;
			dialog.Destroy();
			dialogs.Remove(dialog);
		}

		protected void OnDialogClose(UserView closedDialog) {
			RemoveDialog(closedDialog);
			if (dialogs.Count == 0)
				Close();
		}

	}
}