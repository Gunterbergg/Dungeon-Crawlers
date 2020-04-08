﻿using System.Collections.Generic;
using DungeonCrawlers.UI;
using DungeonCrawlers.Data;
using UnityEngine;
using UnityEngine.Networking;
using Leguar.TotalJSON;

namespace DungeonCrawlers.Systems
{
	public class UserDataLoaderSystem : MonoBehaviour
	{
		public WebRequestInfo userDataRequest;
		public UserData userData;
		public UserView credentialsForm;
		public UserView statusOutputView;
		public int nextSceneIndex;

		protected IForm formInput;
		protected IOutputHandler<TextInfo> outputView;

		protected virtual void Awake() {
			formInput = credentialsForm.GetInterface<IForm>();
			outputView = statusOutputView.GetInterface<IOutputHandler<TextInfo>>();
			formInput.Input += LoginRequest;
		}

		protected void LoginRequest(FormInputInfo formData) {

			//Logs any missing form input
			userDataRequest.requestParams.FindAll((param) => !formData.Entries.ContainsKey(param)).ForEach(
				(param) => Logger.Register(
					"Form " + credentialsForm.name + " does not contain the '" + param + "' paramater", debug: true)
			);
			userDataRequest.requestHeaders.FindAll((param) => !formData.Entries.ContainsKey(param)).ForEach(
				(param) => Logger.Register(
					"Form " + credentialsForm.name + " does not contain the '" + param + "' header", debug: true)
			);

			//Quits if the form is missing an input
			if (!formInput.ContainsEntries(userDataRequest.requestParams)) return;
			if (!formInput.ContainsEntries(userDataRequest.requestHeaders)) return;

			//Display form input errors to user
			if (!formData.IsValidInput) {
				foreach (string message in formData.StatusMessages)
					outputView.Output(new TextInfo(LanguagePack.GetString("error"), message));
				return;
			}

			//Creates and assign parameters
			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			Dictionary<string, string> requestHeaders = new Dictionary<string, string>();

			userDataRequest.requestParams.ForEach((param) => requestParams.Add(param, formData.Entries[param].ToString()));
			userDataRequest.requestHeaders.ForEach((param) => requestParams.Add(param, formData.Entries[param].ToString()));

			//Sends request and outputs results
			HTTPClient.GetRequest(
				userDataRequest.RequestURL, requestParams, requestHeaders, LoginCallback);

		}

		protected virtual void LoginCallback(UnityWebRequest webRequest) {
			
			JSON loaderUserData = JSON.ParseString(webRequest.downloadHandler.text);
			userData.Copy(
				loaderUserData.Deserialize<UserData>( new DeserializeSettings() {
					RequireAllFieldsArePopulated = false
				})
			);

			string requestResponseData =
				"Error: " + webRequest.error + "\n" +
				"Response code:" + webRequest.responseCode + "\n" +
				"Response: \n" + webRequest.downloadHandler.text;
			outputView.Output(new TextInfo("Request", requestResponseData));
			outputView.Output(new TextInfo("Resulting object", userData.ToString()));
			
			statusOutputView.GetInterface<IClosable>().Closed += () => {
				SceneLoader.LoadSceneAsync(nextSceneIndex).allowSceneActivation = true;
			};
		}
	}
}
