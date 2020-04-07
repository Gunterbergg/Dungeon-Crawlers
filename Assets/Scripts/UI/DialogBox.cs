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
		private bool isDisplayingMessage = false;

		public TextMessage CurrentOutput { get; private set; }

		protected override void Awake() {
			base.Awake();
			if (closeButton != null) {
				closeButton.onClick.AddListener(NextAlert);
			}
		}

		public event Action Closed;

		public void Output(TextMessage output) {
			dialogQuery.Add(output);
			if (!isDisplayingMessage) { 
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
			isDisplayingMessage = false;
		}

		public void Close() {
			DeActivate();
			Clear();
			Closed?.Invoke();
		}

		public void NextAlert() {
			if (dialogQuery.Count > 0) {
				CurrentOutput = dialogQuery[0];
				titleTextBox.text = CurrentOutput.Title;
				messageTextBox.text = CurrentOutput.Content;
				dialogQuery.RemoveAt(0);
				isDisplayingMessage = true;
				Activate();
			} else {
				Close();
			}
		}
	}
}