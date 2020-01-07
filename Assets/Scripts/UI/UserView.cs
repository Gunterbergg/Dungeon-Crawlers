using System;
using UnityEngine;

namespace DungeonCrawlers.UI 
{
	[Serializable]
	public abstract class UserView : MonoBehaviour
	{
		protected virtual void Awake() {
			if (!enabled) DeActivate();
		}

		public virtual void Activate() {
			gameObject.SetActive(true);
		}

		public virtual void DeActivate() {
			gameObject.SetActive(false);
		}

		public virtual void Destroy() {
			Destroy(gameObject);
		}

		public TypeCast GetInterface<TypeCast>() where TypeCast : class {
			TypeCast castObj = this as TypeCast;
			if (castObj == null)
				Logger.Register(
					$"User view '{gameObject.name}' view does not contains '{typeof(TypeCast).Name}' functionality",
					"ERROR",debug:true);
			return castObj;
		}
	}
}