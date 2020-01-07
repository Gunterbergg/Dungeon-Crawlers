using System;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace DungeonCrawlers
{
	public static class HTTPClient  
	{
		public static AsyncOperation GetRequest(string requestURL) {
			UnityWebRequest serverRequest = UnityWebRequest.Get(requestURL);
			UnityWebRequestAsyncOperation requestOperation = serverRequest.SendWebRequest();
			return requestOperation;
		}

		public static AsyncOperation GetRequest(string requestURL, EventHandler<HTTPResponseEventArgs> requestHandler) {
			AsyncOperation requestOperation = GetRequest(requestURL);
			requestOperation.completed +=
				(operation) => requestHandler(operation, new HTTPResponseEventArgs(((UnityWebRequestAsyncOperation)operation).webRequest.downloadHandler.text));
			return requestOperation;
		}
	}
}