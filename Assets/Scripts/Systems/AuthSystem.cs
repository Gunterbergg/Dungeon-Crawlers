using DungeonCrawlers.UI;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Leguar.TotalJSON;
using UnityEngine.Events;

namespace DungeonCrawlers.Systems
{
	public class AuthSystem : MonoBehaviour
	{
		public WebRequestInfo createSessionRequest;
		public UserView credentialsFormView;
		public UserView statusDisplayView;
		public UnityEvent OnAuth;

		private bool isPerformingRequest = false;

		protected IOutputHandler<TextMessageInfo> statusDisplay;

		protected virtual void Awake() {
			HandlersReferenceSetup();
		}

		protected void AuthRequest(FormInputInfo formData) {
			if (!formData.IsValidInput) {
				foreach (string message in formData.StatusMessages)
					statusDisplay.Output(new TextMessageInfo(LanguagePack.GetString("error"), message));
				return;
			}
			if (isPerformingRequest) return;
			isPerformingRequest = true;
			WWWForm postData = new WWWForm();
			postData.AddField("email", formData.Entries["email"].ToString());
			postData.AddField("passw", formData.Entries["passw"].ToString());
			HTTPClient.PostRequest(createSessionRequest.href, postData, LoginCallback);
		}

		protected virtual void LoginCallback(UnityWebRequest webRequest) {
			if (webRequest.responseCode != 200 && webRequest.responseCode != 201) {
				TextMessageInfo statusOutput = new TextMessageInfo(
					LanguagePack.GetString("operation_error"),
					LanguagePack.GetString("login_failed"));
				statusDisplay.Output(statusOutput);
				return;
			}

			try { HandleResponse(webRequest.downloadHandler.text); }
			catch (Exception e) {
				TextMessageInfo statusOutput = new TextMessageInfo(
					LanguagePack.GetString("operation_error"),
					LanguagePack.GetString(e.Message));
				statusDisplay.Output(statusOutput);
				return;
			}

			isPerformingRequest = false;
			OnAuth.Invoke();
		}

		private void HandleResponse(string response) {
			try {
				//TODO create method to overwrite and deserialize
				JSON jsonResponse = JSON.ParseString(response);
				Session.Instance.tokenType = jsonResponse.GetString("tokenType");
				Session.Instance.token = jsonResponse.GetString("token");
				Session.Instance.expiresIn = jsonResponse.GetInt("expiresIn");
				Session.Instance.created = jsonResponse.GetString("created");
				Session.Instance.refreshToken = jsonResponse.GetString("refreshToken");

				JSON links = jsonResponse.GetJSON("links");
				foreach (string linkKey in links.Keys)
					Session.Instance.links[linkKey] = links.GetJSON(linkKey).Deserialize<WebRequestInfo>();
			}
			//TODO handle each exception properly
			catch (JSONKeyNotFoundException notFoundException) { throw new Exception("unexpected_response_error_1"); }
			catch (JValueTypeException wrongTypeException) { throw new Exception("unexpected_response_error_2"); }
			catch (DeserializeException wrongMappingException) { throw new Exception("unexpected_response_error_3"); }
			catch (Exception e) { throw new Exception("unknow_error"); }
		}

		private void HandlersReferenceSetup() {
			//TODO add logging and exception handling
			if (credentialsFormView != null) credentialsFormView.GetInterface<IForm>().Input += AuthRequest;
			statusDisplay = statusDisplayView?.GetInterface<IOutputHandler<TextMessageInfo>>();
		}
	}
}
