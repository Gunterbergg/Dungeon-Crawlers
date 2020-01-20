using System.Collections.Generic;

namespace DungeonCrawlers.UI 
{
	public interface IDataEntry
	{
		string EntryName { get; }
		object EntryData { get; }
		bool Enabled { get; set; }

		bool IsEmpty();
		bool IsValid();
		IEnumerable<string> GetStatusMessages();
	}
}