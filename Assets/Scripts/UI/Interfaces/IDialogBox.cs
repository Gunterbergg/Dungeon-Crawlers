using System;
using DungeonCrawlers.Data;

namespace DungeonCrawlers.UI
{
	public interface IDialogBox<InputType> : IUserOutput<DialogBoxOutputData>, IUserInput<InputType>
	where InputType : EventArgs
	{
		event EventHandler<EventArgs> Closed;
		void Close();
	}
}