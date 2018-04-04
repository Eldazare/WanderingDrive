using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {


    public static List<Inventory_Armor> inventoryArmor = new List<Inventory_Armor>();
    public  static List<Inventory_Weapon> inventoryWeapons = new List<Inventory_Weapon>();
    public  static List<int> inventoryMaterials = new List<int>();
    public  static List<int> combatConsumables = new List<int>();
    public  static List<int> nonCombatConsumables = new List<int>();
    public static int capacity;
    public  static int maxCapacity;
    public  static int maxStack = 255;

    //katso löytyykö tavarat inventorysta
    public static  bool CheckIfExists(List<RecipeMaterial> materialList) {
        bool result = true;
        foreach(RecipeMaterial i in materialList) {
            if (materialList[0].subtype == "mat") {
                if (inventoryMaterials[i.itemId] < i.amount) {
                    result = false;
                }
            }
            else if (materialList[0].subtype == "world") {
                if (nonCombatConsumables[i.itemId] < i.amount) {
                    result = false;
                }
            }
            else if (materialList[0].subtype == "comb") {
                if (combatConsumables[i.itemId] < i.amount) {
                    result = false;
                }
            }
        }
        return result;
    }

    //hae itemi inventorysta ja poista se
    public static bool RemoveItem(string itemType, string subType, int itemId, int amount) {
        bool Success = false;
        switch (itemType) {
            case "wep":
                for (int i = 0; i <= inventoryWeapons.Count; i++) {
                    if (itemId == inventoryWeapons[i].itemID) {
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
    public static bool PutItem(string itemType, string subType, int itemId, int amount) {
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

     

}
