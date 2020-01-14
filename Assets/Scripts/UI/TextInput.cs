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
		public InputField input;
		public string placeholderDefault = "Field";
		public string regularExpression = string.Empty;

		public string EntryName { get => entryName; private set => entryName = value; }
		public object EntryData { get; private set; }
		public bool Enabled { get => input.interactable; set => input.interactable = value; }

		public event EventHandler<EventArgs<string>> UserInput;

		protected override void Awake() {
			base.Awake();
			input?.onEndEdit.AddListener((text) => UserInput?.Invoke(this, new EventArgs<string>(text)));
		}

		public void UpdateLabels() {
			input.placeholder.GetComponent<Text>().text = placeholderDefault.StartsWith("@") ? LanguagePack.GetString(placeholderDefault.Substring(1)) : placeholderDefault;
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