using System;
using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawlers.UI
{
	public class DialogBoxDisplay : UserView, IClosable, IOutputHandler<TextMessage>
	{
		public UserView dialogPrefab;
		public Transform dialogsContainer;
		public Button closeButton;
		public List<UserView> dialogs = new List<UserView>();

		public event EventHandler Closed;

		protected override void Awake() {
			base.Awake();
			closeButton.onClick.AddListener(Close);
			foreach (UserView dialog in dialogsContainer.GetComponentsInChildren<UserView>())
				AddDialog(dialog);
		}

		public void Clear() {
			List<UserView> copyList = new List<UserView>(dialogs);
			foreach (UserView dialogReference in copyList)
				dialogReference.GetInterface<IClosable>().Close();
		}

		public void Close() {
			Clear();	
			DeActivate();
			Closed?.Invoke(this, EventArgs.Empty);
		}

		public void Output(TextMessage output) {
			UserView newDialogBox = Instantiate(dialogPrefab, dialogsContainer) as UserView;
			newDialogBox.GetInterface<IOutputHandler<TextMessage>>()?.Output(output);
			AddDialog(newDialogBox);
			Activate();
		}

		public void OutputDefault() {
			Output(
				new TextMessage(
					LanguagePack.GetString("alert"),
					LanguagePack.GetString("error")
				));
		}

		public void AddDialog(UserView newDialog) {
			IClosable castDialog = newDialog.GetInterface<IClosable>();
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