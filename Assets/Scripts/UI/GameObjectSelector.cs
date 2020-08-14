using DungeonCrawlers.Data;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.UI
{
	public class GameObjectSelector : SelectorView<GameObject>
	{
		public ComponentCollection selectableObjects;

		protected override IEnumerable<GameObject> GetCollection() 
			=> RoomsEnumerator();
		
		protected override bool IntersectsWith(GameObject gameObj, Vector2 cursorPosition) 
			=> DataUtility.GetObjectRectBounds(gameObj).Contains(cursorPosition);
		
		private IEnumerable<GameObject> RoomsEnumerator() {
			foreach (MonoBehaviour behaviour in selectableObjects) yield return behaviour.gameObject;
		}
	}
}
