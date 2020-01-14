using System;
using DungeonCrawlers.Data;

namespace DungeonCrawlers.UI
{
	public interface IDialogBox<InputType> : IUserOutput<DialogBoxOutput>, IUserInput<InputType>
	where InputType : EventArgs
	{
		event EventHandler<EventArgs> Closed;
		void Close();
	}
}