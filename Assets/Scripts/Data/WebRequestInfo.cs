using System;
using System.Collections.Generic;

namespace DungeonCrawlers.Data 
{
	[Serializable]
	public struct WebRequestInfo
	{
		public string baseURL;
		public string resourcePath;
		public List<string> requestParams;
		public List<string> requestHeaders;

		public string RequestURL { get => baseURL + resourcePath; }
	}
}
