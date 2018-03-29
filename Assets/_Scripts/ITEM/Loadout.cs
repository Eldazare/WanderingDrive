using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ArmorTypes{helm, chest, arms, legs, boots};

public class Loadout {
	// This is: Container for equipment to be loaded at start of map

	public int accessoryCount;
	private int combatConsumableCount;

	public Inventory_Weapon mainHand = null;
	public Inventory_Weapon offHand = null;
	public Inventory_Armor[] wornArmor; // excludes accessories
	public List<Inventory_Armor> wornAccessories; // 
	public List<int> combatConsumableIndexes;

	public Loadout(int accessoryAmount){
		accessoryCount = accessoryAmount;
		combatConsumableCount = 4;
		for (int i = 0; i < combatConsumableCount; i++) {
			combatConsumableIndexes.Add (-1);
		}
		wornArmor = new Inventory_Armor[5];
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

	public void AddMainHand(Inventory_Weapon wep){
		if (wep.subType == "sword") {
			offHand = null;
		}
		mainHand = wep;
	}

	public void AddOffHand(Inventory_Weapon wep){
		if (mainHand != null) {
			if (mainHand.subType == "sword") {
				return;
			}
		}
		offHand = wep;
	}

	public void AddArmor(Inventory_Armor armor, ArmorTypes type){
		wornArmor [System.Convert.ToInt32 (type)] = armor;
	}

	public void AddAccessory(Inventory_Armor accessory, int slot){
		if (slot >= 0 && slot < accessoryCount) {
			wornAccessories [slot] = accessory;
		} else {
			Debug.LogError ("Invalid slot given to AddAccessory in Loadout");
		}
	}


}
