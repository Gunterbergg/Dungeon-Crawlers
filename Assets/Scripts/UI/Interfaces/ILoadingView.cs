using System;
using DungeonCrawlers.Data;
using UnityEngine;

namespace DungeonCrawlers.UI 
{
	public interface ILoadingView : IUserOutput<LoadingScreenEventArgs> 
	{
		IProgressBar ProgressBar { get; }
		void NextStatus();
		void ListenTo(Func<float> function);
	}
}