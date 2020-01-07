using System.Collections.Generic;

namespace DungeonCrawlers.Data 
{
	public class LoadingScreenEventArgs : System.EventArgs
	{
		private readonly float progress;
		private readonly IEnumerable<string> status;

		public LoadingScreenEventArgs(float progress, IEnumerable<string> status) {
			Progress = progress;
			Status = status;
		}

		public LoadingScreenEventArgs(float progress, params string[] status) {
			Progress = progress;
			Status = status;
		}

		public float Progress { get; }
		public IEnumerable<string> Status { get; }
	}
}