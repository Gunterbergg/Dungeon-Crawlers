using System;

namespace DungeonCrawlers.Data
{ 
	public class DialogBoxOutputData
	{
		public DialogBoxOutputData(string message) {
			Title = LanguagePack.GetString("alert");
			Message = message;
		}

		public DialogBoxOutputData(string title, string message) {
			Title = title;
			Message = message;
		}

		public string Title { get; }
		public string Message { get; }
	}
}