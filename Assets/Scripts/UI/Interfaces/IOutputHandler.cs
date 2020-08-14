using System;

namespace DungeonCrawlers.UI 
{
	public interface IOutputHandler<OutputFormat>
	{
		OutputFormat CurrentOutput { get; }

		void Output(OutputFormat output, Action callback = null);
		void OutputDefault();
		void Clear();
	}
}