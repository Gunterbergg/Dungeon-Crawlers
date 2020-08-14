using UnityEngine;

namespace DungeonCrawlers.UI
{
	public class TriggerSelectable : UserView, ISelectable
	{
		public Object defaultObject;

		public bool InputEnabled { get; set; }
		public Object Value { get => defaultObject; set => defaultObject = Value; }

		public event System.Action<Object> Input;

		public void Selected() {
			Input?.Invoke(Value);
		}
	}
}
