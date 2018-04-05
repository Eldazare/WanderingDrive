using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Loadout {
	// This is: Container for equipment to be loaded at start of map

	public int accessoryCount;
	private int combatConsumableCount;

	public InventoryWeapon mainHand = null;
	public InventoryWeapon offHand = null;
	private bool twoHandEquipped = false;

	public InventoryArmor[] wornArmor; // excludes accessories
	public List<InventoryArmor> wornAccessories = new List<InventoryArmor>(); // 
	public List<int> combatConsumableIndexes = new List<int>();

	public Loadout(int accessoryAmount){
		accessoryCount = accessoryAmount;
		combatConsumableCount = 4;
		for (int i = 0; i < combatConsumableCount; i++) {
			combatConsumableIndexes.Add (-1);
		}
		wornArmor = new InventoryArmor[5];
		for (int i = 0; i < accessoryCount; i++) {
			wornAccessories.Add (null);
		}
	}

	public void AddCombatConsumable(int slotIndex, int itemIndex){
		if (slotIndex >= 0 && slotIndex < combatConsumableCount) {
			combatConsumableIndexes [slotIndex] = itemIndex;
		} else {
			Debug.LogError ("Invalid slot index given at loadout consumable adding");
		}
	}

	public void AddMainHand(InventoryWeapon wep){
		int handed = WeaponCreator.GetHandedness ((WeaponType)System.Enum.Parse (typeof(WeaponType), wep.subType));
		if (handed == 2) {
			offHand = null;
		}
		mainHand = wep;
		twoHandEquipped = true;
	}

	public void AddOffHand(InventoryWeapon wep){
		int handed = WeaponCreator.GetHandedness ((WeaponType)System.Enum.Parse (typeof(WeaponType), wep.subType));
		if (handed == 2) {
			mainHand = wep;
			offHand = null;
			twoHandEquipped = true;
		} else if (!twoHandEquipped) {
			offHand = wep;
		} else {
			// return false;
		}
	}

	public void AddArmor(InventoryArmor armor, ArmorTypes type){
		wornArmor [System.Convert.ToInt32 (type)] = armor;
	}

	public void AddAccessory(InventoryArmor accessory, int slot){
		if (slot >= 0 && slot < accessoryCount) {
			wornAccessories [slot] = accessory;
		} else {
			Debug.LogError ("Invalid slot given to AddAccessory in Loadout");
		}
	}
}
