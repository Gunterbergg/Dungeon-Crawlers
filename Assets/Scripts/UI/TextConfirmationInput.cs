using System;
using System.Collections.Generic;
using DungeonCrawlers.Data;

namespace DungeonCrawlers.UI
{
	public class TextConfirmationInput : UserView, IDataEntry, IInputHandler<string> 
	{
		public string entryName = string.Empty;
		public bool constantExpression;
		public string stringExpression;
		public UserView inputExpression;
		public List<UserView> textConfirmation = new List<UserView>();

		public string EntryName { get => entryName; private set => entryName = value; }
		public object EntryData {
			get {
				if (!constantExpression)
					return inputExpression.GetInterface<IDataEntry>().EntryData;
				return textConfirmation.Count > 0 ? textConfirmation[0].GetInterface<IDataEntry>().EntryData : string.Empty;
			}
		}
		public bool Enabled {
			get {
				if (textConfirmation.Count > 0)
					return textConfirmation[0].GetInterface<IDataEntry>().Enabled;
				return false;
			}
			set {
				if (!constantExpression)
					inputExpression.GetInterface<IDataEntry>().Enabled = false;
				textConfirmation.ForEach((textInput) => textInput.GetInterface<IDataEntry>().Enabled = false);
			}
		}

		public bool InputEnabled { get; set; } = true;

		public event EventHandler<EventArgs<string>> Input;

		protected override void Awake() {
			base.Awake();
			Action<string> raiseEvent = (text) => { if (IsValid()) Input?.Invoke(this, new EventArgs<string>(text)); };
			if (!constantExpression)
				inputExpression.GetInterface<IInputHandler<string>>().Input +=
					(sender, eventArgs) => raiseEvent.Invoke(eventArgs.Data);
			textConfirmation.ForEach(
				(textInput) => textInput.GetInterface<IInputHandler<string>>().Input +=
					(sender, eventArgs) => raiseEvent.Invoke(eventArgs.Data));
		}

		public bool IsEmpty() {
			return string.IsNullOrEmpty(EntryData as string);
		}

		public bool IsValid() {
			Func<string, bool> check = (expression) => {
				foreach (UserView textInput in textConfirmation)
					if ((textInput.GetInterface<IDataEntry>().EntryData as string) != expression)
						return false;
				return true;
			};

			if (constantExpression)
				return check.Invoke(stringExpression);

			return check.Invoke((inputExpression.GetInterface<IDataEntry>().EntryData as string)) &&
					inputExpression.GetInterface<IDataEntry>().IsValid();
		}

		public IEnumerable<string> GetStatusMessages() {
			if (IsEmpty()) {
				yield return LanguagePack.GetString("empty_field");
				yield break;
			}
			if (!IsValid()) yield return LanguagePack.GetString("non_matching_text");
		}
	}
}