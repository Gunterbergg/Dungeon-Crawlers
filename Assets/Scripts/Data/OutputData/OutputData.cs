namespace DungeonCrawlers.Data.Output
{
	public class OutputData<Type>
	{
		public OutputData(Type data) {
			Data = data;
		}

		public Type Data { get; }
	}
}