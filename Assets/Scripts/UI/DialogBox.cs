using System;
using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine.UI;

namespace DungeonCrawlers.UI 
{
	public class DialogBox : UserView, IClosable, IOutputHandler<TextMessage>
	{
		public Button closeButton;
		public Text titleTextBox;
		public Text messageTextBox;

		private List<TextMessage> dialogQuery = new List<TextMessage>();
		private bool isActive = false;

		protected override void Awake() {
			base.Awake();
			if (closeButton != null) { 
				closeButton.onClick.AddListener(() => UserInput?.Invoke(this, EventArgs.Empty));
				UserInput += (sender, eventArgs) => NextAlert();
			}
		}

		public event EventHandler Closed;
		public event EventHandler<EventArgs> UserInput;

		public void Output(TextMessage output) {
			dialogQuery.Add(output);
			if (!isActive) { 
				NextAlert();
			}
		}

		public void OutputDefault() {
			Output(
				new TextMessage(
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
				TextMessage nextDialog = dialogQuery[0];
				titleTextBox.text = nextDialog.Title;
				messageTextBox.text = nextDialog.Content;
				dialogQuery.RemoveAt(0);
				isActive = true;
				Activate();
			} else {
				Close();
			}
		}
	}
}