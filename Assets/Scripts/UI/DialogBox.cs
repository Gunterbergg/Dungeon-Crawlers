	using System;
using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine.UI;

namespace DungeonCrawlers.UI 
{
	public class DialogBox : UserView, IDialogBox<EventArgs>
	{
		public Button closeButton;
		public Text titleTextBox;
		public Text messageTextBox;

		private List<DialogBoxOutput> dialogQuery = new List<DialogBoxOutput>();
		private bool isActive = false;

		protected override void Awake() {
			base.Awake();
			if (closeButton != null) { 
				closeButton.onClick.AddListener(() => UserInput?.Invoke(this, EventArgs.Empty));
				UserInput += (sender, eventArgs) => NextAlert();
			}
		}

		public event EventHandler<EventArgs> Closed;
		public event EventHandler<EventArgs> UserInput;

		public void Output(DialogBoxOutput output) {
			dialogQuery.Add(output);
			if (!isActive) { 
				NextAlert();
			}
		}

		public void OutputDefault() {
			Output(
				new DialogBoxOutput(
					LanguagePack.GetString("alert"),
					LanguagePack.GetString("error")
				));
		}

		public void Clear() {
			titleTextBox.text = string.Empty;
			messageTextBox.text = string.Empty;
			isActive = false;
		}

		public void Close() {
			DeActivate();
			Clear();
			Closed?.Invoke(this, EventArgs.Empty);
		}

		public void NextAlert() {
			if (dialogQuery.Count > 0) {
				DialogBoxOutput nextDialog = dialogQuery[0];
				titleTextBox.text = nextDialog.Title;
				messageTextBox.text = nextDialog.Message;
				dialogQuery.RemoveAt(0);
				isActive = true;
				Activate();
			} else {
				Close();
			}
		}
	}
}