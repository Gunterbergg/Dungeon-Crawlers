using System;
using System.Collections.Generic;
using DungeonCrawlers.Data;

namespace DungeonCrawlers.UI
{
	public class PasswordInput : UserView, IFormItem, IUserInput<EventArgs<string>>
	{
		public string entryName = "passw";
		public string regularExpression = string.Empty;
		public TextInput input;
		public TextInput confirmationInput;

		public string EntryName { get => entryName; private set => entryName = value; }
		public string LabelName { get => input.LabelName; }
		public object EntryData { get => input.EntryData; }
		public bool Enabled { 
			get => confirmationInput.Enabled && input.Enabled; 
			set { input.Enabled = value; confirmationInput.Enabled = value; }
		}

		public event EventHandler<EventArgs<string>> UserInput;

		protected override void Awake() {
			base.Awake();
			Action<string> raiseEvent = (text) => { if (IsValid()) UserInput?.Invoke(this, new EventArgs<string>(text)); };
			input.UserInput += (sender, eventArgs) => raiseEvent.Invoke(eventArgs.Data);
			confirmationInput.UserInput += (sender, eventArgs) => raiseEvent.Invoke(eventArgs.Data);
		}

		public bool IsEmpty() {
			return input.IsEmpty() && confirmationInput.IsEmpty();
		}

		public bool IsValid() {
			return input.IsValid() && confirmationInput.IsValid() && HasMatchingText();
		}

		public IEnumerable<string> GetStatusMessages() {
			if (!HasMatchingText()) yield return LanguagePack.GetString("non_matching_passwords");
			foreach (string message in input.GetStatusMessages())
				yield return message;
		}

		public void UpdateLabels() {
			input.UpdateLabels();
			confirmationInput.UpdateLabels();
		}

		private bool HasMatchingText() {
			return input.EntryData == confirmationInput.EntryData;
		}

		private void OnValidate() {
			input.regularExpression = regularExpression;
			UpdateLabels();
		}
	}
}