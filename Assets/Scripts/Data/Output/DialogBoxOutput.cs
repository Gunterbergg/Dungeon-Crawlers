namespace DungeonCrawlers.Data
{ 
	public class DialogBoxOutput
	{
		public DialogBoxOutput(string message) {
			Title = LanguagePack.GetString("alert");
			Message = message;
		}

		public DialogBoxOutput(string title, string message) {
			Title = title;
			Message = message;
		}

		public string Title { get; }
		public string Message { get; }
	}
}