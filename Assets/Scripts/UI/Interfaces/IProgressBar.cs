using System;
using DungeonCrawlers.Data;

namespace DungeonCrawlers.UI 
{
	public interface IProgressBar : IUserOutput<float>
	{
		bool Enabled { get; set; }
		float Progress { get; set; }

		event EventHandler OnCompleted;
	}
}