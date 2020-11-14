using UnityEngine.Events;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class TriggerSystem : MonoBehaviour
	{
		public bool triggerOnStart = false;
		public UnityEvent onTrigger;

		protected virtual void Start() {
			if (triggerOnStart) Trigger();
		}

		public void Trigger() {
			onTrigger?.Invoke();
		}
	}
}
