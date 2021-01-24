using DungeonCrawlers.Data;
using DungeonCrawlers.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using Leguar.TotalJSON;

namespace DungeonCrawlers.Systems
{
	public class UserDataLoaderSystem : MonoBehaviour
	{
		public UserView statusOutputView;
		public UnityEvent onSerialized;

		private IOutputHandler<TextMessageInfo> statusOutput;
		//private IClosable closePrompt;

		protected virtual void Awake() {
			LoaderSetup();
		}

		public void RefreshData() {
			//TODO add logging and exception handling
			Debug.Log("called");
			if (!Session.Instance.links.ContainsKey("userData")) {
				statusOutput.Output(new TextMessageInfo("@operation_error", "@session_error"));
				return;
			}
			Debug.Log(Session.Instance.links["userData"].Url);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Authorization", "Bearer " + Session.Instance.token);
			UnityWebRequestAsyncOperation loadRequest = 
				HTTPClient.GetRequest(Session.Instance.links["userData"].Url, SerializeUserData, headers);
		}

		private void SerializeUserData(UnityWebRequest request) {
			Debug.Log("Serialize");
			if (request.responseCode != 200) {
				statusOutput.Output(new TextMessageInfo("@operation_error", "@session_error"));
				return;
			}
			Session.Instance.User = 
				JSON.ParseString(request.downloadHandler.text)
				.Deserialize<User>(DataUtility.deserializationSettings);
			onSerialized?.Invoke();
		}

		private void LoaderSetup() {
			//TODO add logging and exception handling
			statusOutput = statusOutputView?.GetInterface<IOutputHandler<TextMessageInfo>>();
			//closePrompt = statusOutputView?.GetInterface<IClosable>();
		}
	}
}
