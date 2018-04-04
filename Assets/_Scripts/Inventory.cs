using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {


    public List<Inventory_Armor> inventoryArmor = new List<Inventory_Armor>();
    public List<Inventory_Weapon> inventoryWeapons = new List<Inventory_Weapon>();
    public List<int> inventoryMaterials = new List<int>();
    public List<int> combatConsumables = new List<int>();
    public List<int> nonCombatConsumables = new List<int>();
    public int capacity;
    public int maxCapacity;
    public int maxStack = 255;

	// TODO: Read counts from config file through DataManager

	public Inventory(){
		int materialCount = 1000;
		int comConCount = 10;
		int nonConCount = 10;
		for (int i = 0; i < materialCount; i++) {
			inventoryMaterials.Add (0);
		}
		for (int i = 0; i < comConCount; i++){
			combatConsumables.Add (0);
		}
		for (int i = 0; i < nonConCount;i++){
			nonCombatConsumables.Add (0);
		}
	}


    //hae itemi inventorysta ja poista se
    public bool RemoveItem(string itemType, string subType, int itemId, int amount)
    {
        bool Success = false;
        switch (itemType)
        {
            case "wep":
                for (int i = 0; i <= inventoryWeapons.Count; i++) {
                    if (itemId == inventoryWeapons[i].ItemID) {
                        inventoryWeapons.RemoveAt(i);
                        capacity--;
                        Success = true;
                    }
                }
                break;
            case "arm":
                for (int i = 0; i <= inventoryArmor.Count; i++) {
                    if(itemId == inventoryArmor[i].itemID && subType == inventoryArmor[i].subType) {
                        inventoryArmor.RemoveAt(i);
                        capacity--;
                        Success = true;
                    }
                }
                break;
            case "cons":
                if (subType == "world") {
                    if (nonCombatConsumables[itemId] < amount) {
                        break;
                    }
                    else {
                        nonCombatConsumables[itemId] -= amount;
                        Success = true;
                    }
                }
                else if (subType == "comb") {
                    if ( combatConsumables[itemId] < amount) {
                        break;
                    }
                    else {
                        combatConsumables[itemId] -= amount;
                        Success = true;
                    }
                }
                break;
            case "mat":
                if(inventoryMaterials[itemId] < amount) {
                    break;
                }
                else {
                    inventoryMaterials[itemId] -= amount;
                    Success = true;
                }
                break;
        }
        return Success;
    }
    
    //Laita uusi Itemi inventoryyn
    public bool PutItem(string itemType, string subType, int itemId, int amount)
    {
        bool Success = false;
        switch (itemType) {
            case "wep":
                if (capacity + 1 > maxCapacity) {
                    break;
                }
                else {
                    Inventory_Weapon newWeapon = new Inventory_Weapon(itemId, subType);
                    inventoryWeapons.Add(newWeapon);
                    capacity++;
                    Success = true;
                    break;
                }
            case "arm":
                if (capacity + 1 > maxCapacity) {
                    break;
                }
                else {
                    Inventory_Armor newArmor = new Inventory_Armor(itemId, subType);
                    inventoryArmor.Add(newArmor);
                    capacity++;
                    Success = true;
                    break;
                }
            case "cons":
                if(subType == "world") {
                    if (nonCombatConsumables[itemId] + amount > maxStack) {
                        break;
                    }
                    else {
                        nonCombatConsumables[itemId] += amount;
                        Success = true;
                    }
                }
                else if (subType == "comb"){
                    if (combatConsumables[itemId] + amount > maxStack){
                        break;
                    }
                    else {
                        combatConsumables[itemId] += amount;
                        Success = true;
                    }    
                }
                break;
            case "mat":
                if (inventoryMaterials[itemId] + amount > maxStack) {
                    break;
                }
                else {
                    inventoryMaterials[itemId] += amount;
                    break;
                }
        }
        return Success;  
    }



	public bool InsertRecipeMaterial(RecipeMaterial recMat){
		return PutItem (recMat.type, recMat.subtype, recMat.itemId, recMat.amount);
	}
     

}
