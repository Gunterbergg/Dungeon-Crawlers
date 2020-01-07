using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace DungeonCrawlers
{
	public static class Logger 
	{
		public const string LogFileName = "latest-log.txt";

		public static List<string> logList = new List<string>();

		static Logger() {
			Register("Logger system initialized at:" + LogFilePath, "LOGGER", addDivisory:true, debug:true);
		}

		public static string DirectoryPath { get => Application.persistentDataPath; }
		public static string LogFilePath { get => DirectoryPath + LogFileName; }
		public static char DivisoryChar { get; set; } = '=';
		public static int DivisoryCount { get; set; } = 80;
		public static string Divisory { get => new string(DivisoryChar, DivisoryCount); }

		public static void Register(string message, string tag = "", bool addDivisory = false, bool addTime = true, bool debug = false) {
			string log = string.Empty;
			string extras = string.Empty;
			extras += addDivisory ? Divisory + Environment.NewLine : string.Empty;
			extras += addTime ? "[" + System.DateTime.Now.ToString() + "] " : string.Empty;
			log += !string.IsNullOrEmpty(tag) ? "[" + tag + "]: " : ": ";
			log += message + Environment.NewLine;

			File.AppendAllText(LogFilePath,extras + log);
			if (debug) Debug.Log(log);
			logList.Add(log);
		}

		public static void DisplayLogs() {
			logList.ForEach((message) => Debug.Log(message));
		}
	}
}