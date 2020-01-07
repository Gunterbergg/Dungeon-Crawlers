namespace DungeonCrawlers.UI 
{
	public interface IUserOutput<OutputFormat>
	{
		void Output(OutputFormat output);
		void Clear();
	}
}