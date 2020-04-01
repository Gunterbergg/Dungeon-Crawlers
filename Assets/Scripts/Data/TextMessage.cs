namespace DungeonCrawlers.Data
{ 
	public struct TextMessage
	{
		public TextMessage(string content) {
			Title = LanguagePack.GetString("alert");
			Content = content;
		}

		public TextMessage(string title, string message) {
			Title = title;
			Content = message;
		}

		public string Title { get; }
		public string Content { get; }
	}
}