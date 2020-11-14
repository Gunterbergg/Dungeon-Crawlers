using System;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public interface IDirectionInput : IInputHandler<Vector2>
	{
		event Action<Vector2> InputRelease;
		event Action<Vector2> InputStart;

		Vector2 GetInput();
		Vector2 GetInputUnclamped();
		Vector2 GetInputRaw();
	}
}