using DungeonCrawlers.Data;
using System;

namespace DungeonCrawlers.UI 
{
	public interface IProgressBar : IUserOutput<float>
	{
		bool Enabled { get; set; }
		float Progress { get; set; }

		event EventHandler<EventArgs<float>> OnValueChanged; 
	}
}