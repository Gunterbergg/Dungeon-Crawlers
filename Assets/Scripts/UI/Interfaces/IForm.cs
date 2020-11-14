using System.Collections.Generic;

namespace DungeonCrawlers.UI 
{
	public struct FormData
	{
		public FormData(Dictionary<string, object> entries, List<string> statusMessages, bool isValid = true)
		{
			Entries = entries;
			StatusMessages = statusMessages;
			IsValidInput = isValid;
		}

		public bool IsValidInput { get; }
		public List<string> StatusMessages { get; }
		public Dictionary<string, object> Entries { get; }
	}

	public interface IForm : IInputHandler<FormData> 
	{
		bool Enabled { get; set; }

		void Submit();
		bool IsValid();
		List<string> GetStatusMessages();
		bool ContainsEntries(in IEnumerable<string> entriesList);
	}
}