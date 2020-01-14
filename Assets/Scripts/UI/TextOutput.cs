using UnityEngine.UI;

namespace DungeonCrawlers.UI
{
	public class TextOutput : UserView, IUserOutput<string> {
		public string defaultText = string.Empty;
		public string DisplayText {get => GetComponent<Text>().text; set => GetComponent<Text>().text = value; }

		public void Clear() {
			gameObject.GetComponent<Text>().text = string.Empty;
		}

		public void Output(string output) {
			gameObject.GetComponent<Text>().text = LanguagePack.GetString(output);
		}

		public void OutputDefault() {
			Output(defaultText);
		}

		public void UpdateLabels() {
			DisplayText = 
				defaultText.StartsWith("@") ?
					LanguagePack.GetString(defaultText.Substring(1)) :
					defaultText;
		}

		public void OnValidate() {
			UpdateLabels();
		}

	}
}