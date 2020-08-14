using System;

namespace DungeonCrawlers.UI 
{
	public interface IInputHandler<InputFormat>
	{
		bool InputEnabled { get; set; }
		event Action<InputFormat> Input;
	}	
}