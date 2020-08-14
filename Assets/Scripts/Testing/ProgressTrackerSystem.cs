using DungeonCrawlers.UI;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.Events;

namespace DungeonCrawlers.Systems
{
	public class ProgressTrackerSystem : MonoBehaviour
	{
		public UserView progressView;
		public ProgressTrackerComponent progress;
		public UnityEvent onProgressFinished;

		protected virtual void Awake() {
			IProgressHandler progressHandler = progressView.GetInterface<IProgressHandler>();
			progress.Changed += (progress) => progressHandler?.Output(progress);
			progressHandler.Completed += onProgressFinished.Invoke;
		}
	}
}
