namespace DungeonCrawlers.UI 
{
	public interface IUserOutput<OutputFormat>
	{
		void OutputDefault();
		void Output(OutputFormat output);
		void Clear();
	}
}