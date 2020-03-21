using System;
using System.Collections.Generic;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace DungeonCrawlers
{
	public static class HTTPClient  
	{
		public static AsyncOperation GetRequest(
			string requestURL,
			Dictionary<string, string> requestHeaders = null,
			Dictionary<string, string> requestParams = null,
			EventHandler<EventArgs<UnityWebRequest>> requestHandler = null)
		{
			string url = requestURL + "?";
			if (requestParams != null)
				foreach (KeyValuePair<string, string> keyValue in requestParams)
					url += keyValue.Key + "=" + keyValue.Value + "&";
			url.Remove(url.Length - 1);

			UnityWebRequest serverRequest = UnityWebRequest.Get(url);
			if (requestHeaders != null)
				foreach (KeyValuePair<string, string> keyValue in requestHeaders)
					serverRequest.SetRequestHeader(keyValue.Key, keyValue.Value);

			UnityWebRequestAsyncOperation requestOperation = serverRequest.SendWebRequest();
			if (requestHandler != null)
				requestOperation.completed +=
					(operation) => requestHandler(operation, new EventArgs<UnityWebRequest>(((UnityWebRequestAsyncOperation)operation).webRequest));

			return requestOperation;
		}
	}
}