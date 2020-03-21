using System;
using System.Collections.Generic;
using DungeonCrawlers.UI;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace DungeonCrawlers.Systems
{
	class FormRequestSystem : MonoBehaviour
	{
		public UserView formInput;
		public UserView outputView;
		public WebRequestInfo requestInfo;

		private void Awake() {
			formInput.GetInterface<IForm>().UserInput += (sender, args) => FormRequest(args);
		}

		private void FormRequest(FormEventArgs formData) {
			requestInfo.requestParams.FindAll((param) => !formData.Entries.ContainsKey(param)).ForEach(
				(param) => Logger.Register("Form " + formInput.name + " does not contain the '" + param + "' paramater", debug: true)
			);
			requestInfo.requestHeaders.FindAll((param) => !formData.Entries.ContainsKey(param)).ForEach(
				(param) => Logger.Register("Form " + formInput.name + " does not contain the '" + param + "' header", debug: true)
			);
			if (!formInput.GetInterface<IForm>().ContainsEntries(requestInfo.requestParams)) return;
			if (!formInput.GetInterface<IForm>().ContainsEntries(requestInfo.requestHeaders)) return;

			if (!formData.IsValidInput) {
				foreach (string message in formData.StatusMessages)
					outputView.GetInterface<IDialogBox<EventArgs>>().Output(new DialogBoxOutput(LanguagePack.GetString("error"), message));
				return;
			}

			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			Dictionary<string, string> requestHeaders = new Dictionary<string, string>();

			requestInfo.requestParams.ForEach((param) => requestParams.Add(param, formData.Entries[param].ToString()));
			requestInfo.requestHeaders.ForEach((param) => requestParams.Add(param, formData.Entries[param].ToString()));

			HTTPClient.GetRequest(requestInfo.RequestURL, requestParams, requestHeaders, (sender,args) => {
				UnityWebRequest webRequest = args.Data;
				string requestResponseData =
					"Error: " + webRequest.error + "\n" +
					"Response code:" + webRequest.responseCode + "\n" +
					"Response: \n" + webRequest.downloadHandler.text;
				outputView.GetInterface<IDialogBox<EventArgs>>().Output(new DialogBoxOutput("Request",requestResponseData));
			});
		}
	}
}
