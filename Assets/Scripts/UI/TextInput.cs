using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

namespace DungeonCrawlers.UI
{
	public class TextInput : UserView, IDataEntry, IInputHandler<string>
	{
		public string entryName;
		public TMP_InputField input;
		public string defaultText = string.Empty;
		public string regularExpression = string.Empty;

		public string EntryName { get => entryName; private set => entryName = value; }
		public object EntryData { get; private set; }
		public bool Enabled { get => input.interactable; set => input.interactable = value; }
		public bool InputEnabled { get; set; } = true;

		public event Action<string> Input;

		protected override void Awake() {
			base.Awake();
			EntryData = input?.text;
			input?.onEndEdit.AddListener((text) => {
				if (!InputEnabled) {
					input.text = (string)EntryData;
				} else {  
					EntryData = (object)text;
					Input?.Invoke(text);
				}
			});
		}

		private void OnValidate() => UpdateLabels();

		public bool IsEmpty() => string.IsNullOrEmpty(input.text);

		public void UpdateLabels() {
			TMP_InputField textMesh = gameObject.GetComponent<TMP_InputField>();
			if (textMesh == null) return;
			if (defaultText.StartsWith("@"))
			textMesh.text = LanguagePack.GetString(defaultText.Substring(1));
			else textMesh.text = defaultText; 
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
	}
}
