namespace DungeonCrawlers.Data
{ 
	public struct TextMessageInfo
	{
		public TextMessageInfo(string content) {
			Title = LanguagePack.GetString("alert");
			Content = content;
		}

		public TextMessageInfo(string title, string message) {
			Title = title;
			Content = message;
		}

		public string Title { get; }
		public string Content { get; }
	}
}
