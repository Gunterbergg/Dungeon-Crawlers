using DungeonCrawlers.Data;
using System;

namespace DungeonCrawlers.UI 
{
	public interface IProgressBar : IOutputHandler<float>
	{
		bool Enabled { get; set; }
		float Progress { get; set; }

		event EventHandler<EventArgs<float>> OnValueChanged; 
	}
}