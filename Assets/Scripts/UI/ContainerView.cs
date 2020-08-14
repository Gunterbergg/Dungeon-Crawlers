using System.Collections.Generic;

namespace DungeonCrawlers.UI
{
	public class ContainerView : UserView
	{
		public bool triggerAll = false;
		public List<UserView> elements;

		public override void Activate() {
			foreach (UserView element in elements) element.Activate();
		}

		public override void DeActivate() {
			foreach (UserView element in elements) element.DeActivate();
		}

		public override void Destroy() {
			foreach (UserView element in elements) element.Destroy();
		}

		public override TypeCast GetInterface<TypeCast>() {
			return elements.Find((element) => element is TypeCast)?.GetInterface<TypeCast>();
		}
	}
}
