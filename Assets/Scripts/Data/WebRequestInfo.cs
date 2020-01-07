using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data 
{
	[CreateAssetMenu( fileName = "RequestInfo", menuName = "InfoContainer/RequestInfo" )]
	public class WebRequestInfo : ScriptableObject 
	{
		public string requestURL;
		public List<string> requestParams;
	}
}