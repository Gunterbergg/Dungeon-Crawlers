namespace DungeonCrawlers.Data
{ 
	public struct TextInfo
	{
		public TextInfo(string content) {
			Title = LanguagePack.GetString("alert");
			Content = content;
		}

		public TextInfo(string title, string message) {
			Title = title;
			Content = message;
		}

		public string Title { get; }
		public string Content { get; }
	}
}
