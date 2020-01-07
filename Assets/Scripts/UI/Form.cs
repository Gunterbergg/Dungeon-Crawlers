using System;
using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawlers.UI
{
	public class Form : UserView, IForm, IUserInput<FormEventArgs>
	{
		public string titleString;
		public Text titleTextBox;
		public GameObject container;
		public string submitString = "@form_label";
		public Button submitButton;

		public bool Enabled { 
			get { return enabled; } 
			set 
			{
				enabled = value;
				foreach (IFormItem formItem in GetFormItems()) {
					formItem.Enabled = enabled;
				}
			}
		}

		public event EventHandler<FormEventArgs> UserInput;

		protected override void Awake() {
			base.Awake();
			submitButton.onClick.AddListener(() => Submit());
		}

		public virtual void Submit() {
			UserInput?.Invoke(this, new FormEventArgs(GetEntries(), GetStatusMessages(), IsValid()));
		}

		public void UpdateLabels() {
			if (!string.IsNullOrEmpty(titleString) && titleTextBox != null)
				titleTextBox.text = titleString.StartsWith("@") ? LanguagePack.GetString(titleString.Substring(1)) : titleString ;
			if (!string.IsNullOrEmpty(submitString) && submitButton != null)
				submitButton.GetComponentInChildren<Text>().text = submitString.StartsWith("@") ? LanguagePack.GetString(submitString.Substring(1)) : submitString;
		}

		public bool IsValid() {
			foreach (IFormItem formItem in GetFormItems()) {
				if (!formItem.IsValid())
					return false;
			}
			return true;
		}

		public bool ContainsEntries(in IEnumerable<string> entriesList) {
			Dictionary<string, object> entries = GetEntries();
			foreach (string entry in entriesList) {
				if (!entries.ContainsKey(entry)) {
					return false; 
				}
			}
			return true;
		}

		public IEnumerable<IFormItem> GetFormItems() {
			foreach (UserView view in container.GetComponentsInChildren<UserView>()) {
				IFormItem formItem = view.GetInterface<IFormItem>();
				if (formItem == null || view.transform.parent != container.transform)
					continue;
				yield return formItem;
			}
		}

		public List<string> GetStatusMessages() {
			List<string> statusMessages = new List<string>();
			foreach (IFormItem formItem in GetFormItems()) {
				if (!formItem.IsValid())
					foreach (string message in formItem.GetStatusMessages())
						statusMessages.Add(formItem.LabelName + ": " + message);
			}
			return statusMessages;
		}

		protected Dictionary<string, object> GetEntries() {
			Dictionary<string, object> entries = new Dictionary<string, object>();
			foreach (IFormItem formItem in GetFormItems()) {
				entries.Add(formItem.EntryName,formItem.EntryData);
			}
			return entries;
		}

		private void OnValidate() {
			UpdateLabels();
		}
	}
}