namespace DungeonCrawlers.UI 
{
	public interface IOutputHandler<OutputFormat>
	{
		OutputFormat CurrentOutput { get; }

		void OutputDefault();
		void Output(OutputFormat output);
		void Clear();
	}
}