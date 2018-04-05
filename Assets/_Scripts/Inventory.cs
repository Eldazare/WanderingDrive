using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {


    public static List<InventoryArmor> inventoryArmor = new List<InventoryArmor>();
    public static List<InventoryWeapon> inventoryWeapons = new List<InventoryWeapon>();
    public static List<int> inventoryMaterials = new List<int>();
    public static List<int> combatConsumables = new List<int>();
    public static List<int> nonCombatConsumables = new List<int>();
    public static int capacity;
    public static int maxCapacity = 30;
    public static int maxStack = 255;

	// TODO: Read counts from config file through DataManager
	// ^^ Initialize script

	/*
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
	*/

	public static void Initialize(){
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

    //katso löytyykö tavarat inventorysta
    public static  bool CheckIfExists(List<RecipeMaterial> materialList) {
        bool result = true;
        int temp = 0;
        foreach (RecipeMaterial i in materialList) {
            if (i.subtype == "mat") {
                if (inventoryMaterials[i.itemId] < i.amount) {
                    result = false;
                }
            }
            else if (i.subtype == "nonCom") {
                if (nonCombatConsumables[i.itemId] < i.amount) {
                    result = false;
                }
            }
            else if (i.subtype == "comCom") {
                if (combatConsumables[i.itemId] < i.amount) {
                    result = false;
                    
                }
            }
            temp++;
        }
        return result;
    }

    //hae itemi inventorysta ja poista se
    public static bool RemoveItem(string itemType, string subType, int itemId, int amount) {
        bool Success = false;
        switch (itemType) {
            case "weapon":
                for (int i = 0; i <= inventoryWeapons.Count; i++) {
                    if (itemId == inventoryWeapons[i].itemID) {
                        inventoryWeapons.RemoveAt(i);
                        capacity--;
                        Success = true;
                    }
                }
                break;
            case "armor":
                for (int i = 0; i <= inventoryArmor.Count; i++) {
                    if(itemId == inventoryArmor[i].itemID && subType == inventoryArmor[i].subType) {
                        inventoryArmor.RemoveAt(i);
                        capacity--;
                        Success = true;
                    }
                }
                break;
            case "cons":
                if (subType == "nonCom") {
                    if (nonCombatConsumables[itemId] < amount) {
                        break;
                    }
                    else {
                        nonCombatConsumables[itemId] -= amount;
                        Success = true;
                    }
                }
                else if (subType == "comCom") {
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
    public static bool PutItem(string itemType, string subType, int itemId, int amount) {
        bool Success = false;
        switch (itemType) {
            case "weapon":
                if (capacity + 1 > maxCapacity) {
                    break;
                }
                else {
                    InventoryWeapon newWeapon = new InventoryWeapon(itemId, subType);
                    inventoryWeapons.Add(newWeapon);
                    capacity++;
                    Success = true;
                    break;
                }
            case "armor":
                if (capacity + 1 > maxCapacity) {
                    break;
                }
                else {
                    InventoryArmor newArmor = new InventoryArmor(itemId, subType);
                    inventoryArmor.Add(newArmor);
                    capacity++;
                    Success = true;
                    break;
                }
            case "cons":
                if(subType == "nonCom") {
                    if (nonCombatConsumables[itemId] + amount > maxStack) {
                        break;
                    }
                    else {
                        nonCombatConsumables[itemId] += amount;
                        Success = true;
                    }
                }
                else if (subType == "comCom"){
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



	public static bool InsertRecipeMaterial(RecipeMaterial recMat){
		return PutItem (recMat.type, recMat.subtype, recMat.itemId, recMat.amount);
	}
     

}
