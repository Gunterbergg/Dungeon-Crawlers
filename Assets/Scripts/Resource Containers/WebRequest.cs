using UnityEngine;

namespace DungeonCrawlers
{
	[CreateAssetMenu(fileName="WebRequest",menuName="DungeonCrawlers/WebRequest")]
	public class WebRequest : ScriptableObject
	{
		public const string HOST = "localhost:3000";

		public string href;
		public string method;
		
		public string Url { get => HOST + href; }
	}
}
