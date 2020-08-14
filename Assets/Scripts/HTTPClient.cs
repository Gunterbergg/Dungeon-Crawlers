using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DungeonCrawlers
{
	public static class HTTPClient  
	{
		public static UnityWebRequestAsyncOperation GetRequest(
			string requestURL,
			Action<UnityWebRequest> requestHandler,
			Dictionary<string, string> requestHeaders = null)
		{
			UnityWebRequest webRequest = UnityWebRequest.Get(requestURL);
			return SendWebRequest(webRequest, requestHandler, requestHeaders);
		}

		public static UnityWebRequestAsyncOperation PostRequest(
			string requestURL,
			WWWForm postData,
			Action<UnityWebRequest> requestHandler,
			Dictionary<string, string> requestHeaders = null) 
		{
			UnityWebRequest webRequest = UnityWebRequest.Post(requestURL, postData ?? new WWWForm());
			return SendWebRequest(webRequest, requestHandler, requestHeaders);
		}

		public static UnityWebRequestAsyncOperation SendWebRequest(
			UnityWebRequest webRequest,
			Action<UnityWebRequest> requestHandler,
			Dictionary<string, string> requestHeaders = null)
		{
			if (requestHeaders != null)
				foreach (KeyValuePair<string, string> keyValue in requestHeaders)
					webRequest.SetRequestHeader(keyValue.Key, keyValue.Value);

			webRequest.chunkedTransfer = false;
			UnityWebRequestAsyncOperation requestOperation = webRequest.SendWebRequest();
			if (requestHandler != null) requestOperation.completed +=
					(operation) => requestHandler(((UnityWebRequestAsyncOperation)operation).webRequest);

			return requestOperation;
		}
	}
}
