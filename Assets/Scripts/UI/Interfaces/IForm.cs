using System.Collections.Generic;
using DungeonCrawlers.Data;

namespace DungeonCrawlers.UI 
{
	public interface IForm : IUserInput<FormEventArgs> 
	{
		bool Enabled { get; set; }

		void Submit();
		bool IsValid();
		List<string> GetStatusMessages();
		bool ContainsEntries(in IEnumerable<string> entriesList);
	}
}