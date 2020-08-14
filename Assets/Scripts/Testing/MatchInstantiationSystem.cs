using DungeonCrawlers.Data;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class MatchInstantiationSystem : MonoBehaviour
	{
		/*public PrefabCollectionInfo playerPrefabs;
		public PrefabCollectionInfo skillsPrefabs;

		public MatchmakingComponent match;		
		public ComponentCollection rooms;

		protected virtual void Awake() {
			ComponentsReferenceSetup();
		}

		//DELETE testing method
		protected virtual void Start() {
			rooms.AddNodeComponents<RoomComponent>(rooms.gameObject);
			SpawnPlayer();
		}

		protected void SpawnPlayer() {
			//TODO add logging and exception handling
			if (match == null || rooms.Count <= 0) return;
			RoomComponent firstRoom = rooms[0] as RoomComponent;

			GameObject playerObject = Instantiate(playerPrefabs[match.selectedSkin]);
			GameObject weaponObject = Instantiate(skillsPrefabs[match.selectedWeapon], playerObject.transform);

			firstRoom.entities.AddNodeComponents<EntityComponent>(playerObject);
			EntityComponent player = DataUtility.GetData<EntityComponent>(playerObject);
			SkillComponent weapon = DataUtility.GetData<SkillComponent>(weaponObject);

//			player.Weapon = weaponObject;
//			weapon.owner = playerObject;
		}

		private void ComponentsReferenceSetup() {
			//TODO add logging and exception handling
			if (match == null) return;
			match.MatchStarted += SpawnPlayer;
		}*/
	}
}
