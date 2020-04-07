using Leguar.TotalJSON;
using System;
using UnityEngine;

namespace DungeonCrawlers
{
	public static class LanguagePack 
	{
		public const string DefaultPath = "Localization/";
		public const string DefaultFileName = "/strings";
		private const string DefaultLanguage = "pt_BR";

		public static JSON strings = new JSON();
		
		static LanguagePack() {
			ChangeLang(DefaultLanguage);
			Logger.Register("Localization initialized with default language " + DefaultLanguage);
		}

		public static event Action LocalizationChanged;

		public static string GetString(params string[] keys) {
			string result = string.Empty;
			foreach (string key in keys) {
				result += strings.ContainsKey(key) ? strings.GetString(key) : key;
			}
			return result;
		}

		public static void ChangeLang(string langCode, string fileName = DefaultFileName) {
			string path = DefaultPath + langCode + DefaultFileName;
			TextAsset stringsFile = Resources.Load<TextAsset>(path);
			if (stringsFile == null) {
				Logger.Register("Failed to find strings file for lang code " + langCode + " at path " + path, "LANGUAGE", debug:true);
				return;
			}

			JSON rootJSON = JSON.ParseString(stringsFile.text);
			foreach (string categoryKey in rootJSON.Keys) {
				JSON category = rootJSON.GetJSON(categoryKey);
				foreach (string key in category.Keys) {
					strings.AddOrReplace(key, category.GetString(key));
				}
			}

			Logger.Register("Successfully loaded strings file for lang code " + langCode, "LANGUAGE");
			LocalizationChanged?.Invoke();
		}
	}
}