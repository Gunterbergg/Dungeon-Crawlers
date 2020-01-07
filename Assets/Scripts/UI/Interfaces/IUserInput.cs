using System;

namespace DungeonCrawlers.UI 
{
	public interface IUserInput<InputFormat> where InputFormat : System.EventArgs 
	{
		event EventHandler<InputFormat> UserInput;
	}	
}