using UnityEngine;

namespace DungeonCrawlers.UI
{
	interface ISelectable : IInputHandler<Object>
	{
		Object Value { get; set; }
	}
}
