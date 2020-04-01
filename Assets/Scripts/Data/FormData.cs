using System.Collections.Generic;

namespace DungeonCrawlers.Data 
{
	public struct FormData
	{
		public FormData(Dictionary<string, object> entries, List<string> statusMessages, bool isValid = true) {
			Entries = entries;
			StatusMessages = statusMessages;
			IsValidInput = isValid;	
		}

		public bool IsValidInput { get; }
		public List<string> StatusMessages { get; }
		public Dictionary<string, object> Entries { get; }
	}
}