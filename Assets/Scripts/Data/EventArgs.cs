using System;

namespace DungeonCrawlers.Data 
{
	public class EventArgs<Type> : EventArgs 
	{
		public EventArgs(Type data) {
			Data = data;
		}

		public Type Data { get; }
	}
}