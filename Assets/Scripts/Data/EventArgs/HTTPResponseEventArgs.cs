namespace DungeonCrawlers.Data 
{
	public enum HTTPRequestStatus 
	{
		Success, NetworkError, HTTPError, DatabaseError
	}

	public class HTTPResponseEventArgs : System.EventArgs
	{
		private readonly string rawData;
		private readonly HTTPRequestStatus status;

		public HTTPResponseEventArgs(string rawData) {
			this.rawData = rawData;
			this.status = 
				rawData.StartsWith("failed") ? HTTPRequestStatus.DatabaseError :
				rawData.StartsWith("error") ?  HTTPRequestStatus.HTTPError :
				rawData.StartsWith("timeout") ? HTTPRequestStatus.NetworkError :
				HTTPRequestStatus.Success;
		}

		public string RawData { get => rawData; }
		public HTTPRequestStatus Status { get => status; }
		public bool IsError { get => Status == HTTPRequestStatus.Success; }
	}
}