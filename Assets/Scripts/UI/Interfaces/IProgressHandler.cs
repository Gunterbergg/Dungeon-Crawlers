using System;

namespace DungeonCrawlers.UI 
{
	public interface IProgressHandler : IOutputHandler<float>
	{
		bool Enabled { get; set; }
		float LerpTime { get; set; }

		event Action<float> ValueChanged;
		event Action Completed;
	}
}