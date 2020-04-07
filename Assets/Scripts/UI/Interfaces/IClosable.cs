using System;

namespace DungeonCrawlers.UI
{
	public interface IClosable
	{
		event Action Closed;
		void Close();
	}
}