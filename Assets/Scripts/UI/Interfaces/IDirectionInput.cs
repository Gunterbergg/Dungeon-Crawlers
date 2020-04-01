using System;
using DungeonCrawlers.Data;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public interface IDirectionInput : IInputHandler<Vector2>
	{
		event EventHandler<EventArgs<Vector2>> InputRelease;

		Vector2 GetInput();
		Vector2 GetInputUnclamped();
		Vector2 GetInputRaw();
	}
}