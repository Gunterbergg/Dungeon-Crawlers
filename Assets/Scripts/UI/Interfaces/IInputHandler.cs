using System;
using DungeonCrawlers.Data;

namespace DungeonCrawlers.UI 
{
	public interface IInputHandler<InputFormat>
	{
		bool InputEnabled { get; set; }
		event EventHandler<EventArgs<InputFormat>> Input;
	}	
}