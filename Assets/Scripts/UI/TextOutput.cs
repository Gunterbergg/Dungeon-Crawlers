using UnityEngine.UI;

namespace DungeonCrawlers.UI
{
	public class TextOutput : UserView, IOutputHandler<string> 
	{
		public string defaultText = string.Empty;

		public string CurrentOutput { get => GetComponent<Text>().text; set => GetComponent<Text>().text = value; }
		
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
			CurrentOutput = 
				defaultText.StartsWith("@") ?
					LanguagePack.GetString(defaultText.Substring(1)) :
					defaultText;
		}

		public void OnValidate() {
			UpdateLabels();
		}
	}
}