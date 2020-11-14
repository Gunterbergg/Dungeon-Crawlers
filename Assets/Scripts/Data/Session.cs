using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	[CreateAssetMenu(fileName = "Session", menuName = "DungeonCrawlers/Session")]
	public class Session : ScriptableObject 
	{
		private static Session instance = null;

		public string tokenType = "Bearer";
		public string token;
		public string created;
		public int expiresIn;
		public string refreshToken;
		public Dictionary<string, WebRequest> links = new Dictionary<string, WebRequest>();

		public User user;

		public event Action SessionUserChanged;

		public static Session Instance {
			get {
				if (instance == null)
					instance = (Session)Resources.Load("Session");
				return instance;
			}
		}
		public User User {
			get => user;
			set {
				bool hasChanged = value != user;
				user = value;
				if (hasChanged) SessionUserChanged?.Invoke(); 
			}
		}

		public bool HasTokenExpired { 
			//TODO implement
			get { return false; }
		}
	}
}
