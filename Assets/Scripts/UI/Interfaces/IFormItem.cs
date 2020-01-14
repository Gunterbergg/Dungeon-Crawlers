using System.Collections.Generic;

namespace DungeonCrawlers.UI 
{
	public interface IFormItem
	{
		string EntryName { get; }
		object EntryData { get; }
		bool Enabled { get; set; }

		bool IsEmpty();
		bool IsValid();
		IEnumerable<string> GetStatusMessages();
	}
}