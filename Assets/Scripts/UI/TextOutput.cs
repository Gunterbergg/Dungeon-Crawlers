using UnityEngine.UI;
using TMPro;
using System;

namespace DungeonCrawlers.UI
{
	public class TextOutput : UserView, IOutputHandler<string> 
	{
		public string defaultText = string.Empty;

		public string CurrentOutput { get => GetComponent<TextMeshProUGUI>().text; set => Output(value); }

		private void OnValidate() => OutputDefault();

		public virtual void Clear() => gameObject.GetComponent<TextMeshProUGUI>().text = string.Empty;

		public virtual void OutputDefault() => Output(defaultText);

		public virtual void Output(string output, Action callback = null) {
			TextMeshProUGUI textMesh = gameObject.GetComponent<TextMeshProUGUI>();
			if (textMesh == null) return;

			if (output.StartsWith("@"))
			textMesh.text = LanguagePack.GetString(output.Substring(1)); else
			textMesh.text = output;

			callback?.Invoke();
		}
	}
}
