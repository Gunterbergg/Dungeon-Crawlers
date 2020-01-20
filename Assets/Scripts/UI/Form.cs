using System;
using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawlers.UI
{
	public class Form : UserView, IForm
	{
		public GameObject container;
		public Button submitButton;

		public bool Enabled { 
			get { return enabled; } 
			set 
			{
				enabled = value;
				foreach (IDataEntry formItem in GetFormItems()) {
					formItem.Enabled = enabled;
				}
			}
		}

		public event EventHandler<FormEventArgs> UserInput;

		protected override void Awake() {
			base.Awake();
			submitButton?.onClick.AddListener(() => Submit());
		}

		public virtual void Submit() {
			UserInput?.Invoke(this, new FormEventArgs(GetEntries(), GetStatusMessages(), IsValid()));
			new FormEventArgs(GetEntries(), GetStatusMessages(), IsValid());
		}

		public bool IsValid() {
			foreach (IDataEntry formItem in GetFormItems()) {
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

		public List<IDataEntry> GetFormItems() {
			List<IDataEntry> entryList = new List<IDataEntry>();
			foreach (UserView view in container.GetComponentsInChildren<UserView>()) {
				if (!(view is IDataEntry)) continue; 
				if (view.transform.parent != container.transform) continue;

				IDataEntry formItem = view.GetInterface<IDataEntry>();
				if (!entryList.Exists((entry) => formItem.EntryName == entry.EntryName))
					entryList.Add(formItem);				
			}
			return entryList;
		}

		public List<string> GetStatusMessages() {
			List<string> statusMessages = new List<string>();
			foreach (IDataEntry formItem in GetFormItems()) {
				if (!formItem.IsValid())
					foreach (string message in formItem.GetStatusMessages())
						statusMessages.Add(formItem.EntryName + ": " + message);
			}
			return statusMessages;
		}

		public Dictionary<string, object> GetEntries() {
			Dictionary<string, object> entries = new Dictionary<string, object>();
			foreach (IDataEntry formItem in GetFormItems()) {
				entries.Add(formItem.EntryName,formItem.EntryData);
			}
			return entries;
		}

	}
}