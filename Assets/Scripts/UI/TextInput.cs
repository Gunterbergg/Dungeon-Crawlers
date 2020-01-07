using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DungeonCrawlers.Data;
using UnityEngine.UI;

namespace DungeonCrawlers.UI
{
	public class TextInput : UserView, IFormItem, IUserInput<EventArgs<string>>
	{
		public string entryName;
		public Text label;
		public string labelString = "label";
		public InputField input;
		public string placeholderString = "Field";
		public string regularExpression = string.Empty;

		public string EntryName { get => entryName; private set => entryName = value; }
		public string LabelName { get => label.text; }
		public object EntryData { get; private set; }
		public bool Enabled { get => input.interactable; set => input.interactable = value; }

		public event EventHandler<EventArgs<string>> UserInput;

		protected override void Awake() {
			base.Awake();
			input?.onEndEdit.AddListener((text) => UserInput?.Invoke(this, new EventArgs<string>(text)));
		}

		public void UpdateLabels() {
			label.text = labelString.StartsWith("@") ? LanguagePack.GetString(labelString.Substring(1)) : labelString;
			input.placeholder.GetComponent<Text>().text = placeholderString.StartsWith("@") ? LanguagePack.GetString(placeholderString.Substring(1)) : placeholderString;
		}

		public bool IsEmpty() {
			return string.IsNullOrEmpty(input.text);
		}

		public bool IsValid() {
			Regex regExp = new Regex(regularExpression);
			return IsEmpty() ? false : regExp.IsMatch(input.text);
		}

		public IEnumerable<string> GetStatusMessages() {
			if (IsEmpty()) {
				yield return LanguagePack.GetString("empty_field");
				yield break;
			}
			if (!IsValid()) yield return LanguagePack.GetString("invalid_text");
		}

		private void OnValidate() {
			UpdateLabels();
		}
	}
}