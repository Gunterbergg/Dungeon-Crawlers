using DungeonCrawlers.UI;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace DungeonCrawlers.Systems
{
	public class RegisterSystem : MonoBehaviour
	{
		public WebRequestInfo userRegistrationRequest;
		public UserView credentialsFormView;
		public UserView statusDisplayView;

		protected IOutputHandler<TextMessageInfo> statusDisplay;

		protected virtual void Awake() {
			HandlersReferenceSetup();
		}

		protected void RequestAccountCreation(FormInputInfo formData) {
			if (!formData.IsValidInput) {
				foreach (string message in formData.StatusMessages)
					statusDisplay.Output(new TextMessageInfo(LanguagePack.GetString("error"), message));
				return;
			}
			WWWForm postData = new WWWForm();
			postData.AddField("email", formData.Entries["email"].ToString());
			postData.AddField("passw", formData.Entries["passw"].ToString());
			HTTPClient.PostRequest(userRegistrationRequest.href, postData, AccountCreationRequestCallback);
		}

		protected virtual void AccountCreationRequestCallback(UnityWebRequest webRequest) {
			//TODO handle output correctly
			TextMessageInfo statusOutput;	
			if (webRequest.responseCode == 200 || webRequest.responseCode == 201) { 
				statusOutput = new TextMessageInfo(
					LanguagePack.GetString("success"),
					LanguagePack.GetString("registration_success"));
			} else { 
				statusOutput = new TextMessageInfo(
					LanguagePack.GetString("failed"),
					LanguagePack.GetString("registration_failed"));
			}
			statusDisplay.Output(statusOutput);
		}

		private void HandlersReferenceSetup() {
			//TODO add logging and exception handling
			if (credentialsFormView != null) credentialsFormView.GetInterface<IForm>().Input += RequestAccountCreation;
			statusDisplay = statusDisplayView?.GetInterface<IOutputHandler<TextMessageInfo>>();
		}
	}
}
