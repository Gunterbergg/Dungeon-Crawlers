using Leguar.TotalJSON;
using System;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	public class JSONComponent : MonoBehaviour
	{
		protected JSON JSON;
		public event Action<JSON> Changed;

		public JSON JSONData {
			get => JSON;
			set {
				JSON = value;
				Changed?.Invoke(value);
			}
		}
	}
}
