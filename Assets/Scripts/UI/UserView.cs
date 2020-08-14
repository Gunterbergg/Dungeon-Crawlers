using UnityEngine;

namespace DungeonCrawlers.UI 
{
	public abstract class UserView : MonoBehaviour
	{
		public UserView Instance { get; private set; }
		
		public static UserView CreateView(GameObject view) {
			return Instantiate(view, GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<UserView>();
		}

		public static UserView GetInstance(GameObject view) {
			UserView viewComponent = view.GetComponent<UserView>();
			if (viewComponent.Instance == null) 
				viewComponent.Instance = CreateView(view);
			
			return viewComponent.Instance;
		}

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

		public virtual TypeCast GetInterface<TypeCast>() where TypeCast : class {
			TypeCast castObj = this as TypeCast;
			if (castObj == null)
				Logger.Register(
					$"User view '{gameObject.name}' view does not contains '{typeof(TypeCast).Name}' functionality",
					"ERROR", debug:true);
			return castObj;
		}
	}
}