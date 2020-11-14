using System;
using UnityEngine;

namespace DungeonCrawlers.Data
{
	public class EquipSlotComponent : MonoBehaviour
	{
		new public string name = "default";
		[SerializeField] private EntityComponent owner;
		[SerializeField] private GameObject equippedItem;

		//TODO change signature to include parameters
		public event Action OwnerChanged;
		public event Action EquipmentChanged;

		public GameObject EquippedItem { 
			get => equippedItem;
			set { 
				equippedItem = value;
				EquipmentChanged?.Invoke();
			}
		}
		public EntityComponent Owner {
			get => owner;
			set {
				owner = value;
				OwnerChanged?.Invoke();
			}
		}
	}
}
