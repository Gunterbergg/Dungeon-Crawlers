using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data 
{
	[CreateAssetMenu( fileName = "RequestInfo", menuName = "InfoContainer/RequestInfo" )]
	public class WebRequestInfo : ScriptableObject 
	{
		public string baseURL;
		public string resourcePath;
		public List<string> requestParams;
		public List<string> requestHeaders;

		public string RequestURL { get => baseURL + resourcePath; }
	}
}