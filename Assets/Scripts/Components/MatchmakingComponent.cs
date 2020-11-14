using System;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	public class MatchmakingComponent : MonoBehaviour
	{
		public int defenderUserId;
		public int attackerUserId;

		public string selectedSkin;
		public string selectedWeapon;

		public event Action MatchStarted;
		public event Action MatchEnded;

		public void StartMatch() => MatchStarted?.Invoke();
		public void EndMatch() => MatchEnded?.Invoke();
	}
}
