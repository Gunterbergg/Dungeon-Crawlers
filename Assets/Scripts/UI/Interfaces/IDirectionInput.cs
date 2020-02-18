using DungeonCrawlers.Data;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public interface IDirectionInput : IUserInput<EventArgs<Vector2>>
	{
		Vector2 GetInput();
		Vector2 GetInputUnclamped();
		Vector2 GetInputRaw();
	}
}