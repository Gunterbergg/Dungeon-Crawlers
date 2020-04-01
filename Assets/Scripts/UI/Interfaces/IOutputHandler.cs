namespace DungeonCrawlers.UI 
{
	public interface IOutputHandler<OutputFormat>
	{
		void OutputDefault();
		void Output(OutputFormat output);
		void Clear();
	}
}