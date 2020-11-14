using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DungeonCrawlers.UI 
{
	public struct TextMessageInfo
	{
		public TextMessageInfo(string content)
		{
			Title = LanguagePack.GetString("alert");
			Content = content;
		}

		public TextMessageInfo(string title, string message)
		{
			Title = title;
			Content = message;
		}

		public string Title { get; }
		public string Content { get; }
	}

	public class DialogBox : UserView, IClosable, IOutputHandler<TextMessageInfo>, IOutputHandler<string>
	{
		public Button closeButton;
		public UserView titleTextView;
		public UserView messageTextView;

		protected struct DialogBoxQueryItem {
			public TextMessageInfo messageInfo;
			public Action callback;

			public DialogBoxQueryItem(TextMessageInfo messageInfo, Action callback) {
				this.messageInfo = messageInfo;
				this.callback = callback;
			}
		}

		protected IOutputHandler<string> titleText;
		protected IOutputHandler<string> messageText;
		protected List<DialogBoxQueryItem> dialogQuery = new List<DialogBoxQueryItem>();
		protected bool isDisplayingMessage = false;

		public TextMessageInfo CurrentOutput { get; private set; }
		string IOutputHandler<string>.CurrentOutput { get => CurrentOutput.Content; }

		public event Action Closed;
		
		protected override void Awake() {
			base.Awake();
			HandlersReferenceSetup();
		}

		public void Output(string output, Action callback = null) => Output(new TextMessageInfo(output), callback);

		public void Output(TextMessageInfo output, Action callback = null) {
			dialogQuery.Add(new DialogBoxQueryItem(output, callback));
			if (!isDisplayingMessage) {
				Activate();
				NextAlert();
			}
		}

		public void NextAlert() {
			if (dialogQuery.Count <= 0) {
				Close();
				return;
			}

			DialogBoxQueryItem nextDialog = dialogQuery[0];
			CurrentOutput = nextDialog.messageInfo;
			
			titleText.Output(CurrentOutput.Title);
			messageText.Output(CurrentOutput.Content);
			
			dialogQuery.RemoveAt(0);
			isDisplayingMessage = true;
			Activate();
			nextDialog.callback?.Invoke();			
		}

		public void Clear() {
			titleText?.Output(string.Empty);
			messageText?.Output(string.Empty);
		}

		public void Close() {
			DeActivate();
			Closed?.Invoke();
			isDisplayingMessage = false;
			Clear();
		}

		public void OutputDefault() => Output(new TextMessageInfo(LanguagePack.GetString("alert"),LanguagePack.GetString("error")));

		private void HandlersReferenceSetup() {
			//TODO add logging and exception handling
			if (titleTextView != null) titleText = titleTextView.GetInterface<IOutputHandler<string>>();
			if (messageTextView != null) messageText = messageTextView.GetInterface<IOutputHandler<string>>();
			if (closeButton != null) closeButton.onClick.AddListener(NextAlert);
		}
	}
}
