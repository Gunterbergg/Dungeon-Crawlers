namespace DungeonCrawlers.Data.Output
{
	public class Output<Type>
	{
		public Output(Type data) {
			Data = data;
		}

		public Type Data { get; }
	}
}