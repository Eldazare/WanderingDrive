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
		foreach (RecipeMaterial item in materialList) {
			ItemSubType itemSubType = item.subtype;
            if (itemSubType == ItemSubType.NonCom) {
                if (nonCombatConsumables[item.itemId] < item.amount) {
                    result = false;
                }
            }
            else if (itemSubType == ItemSubType.ComCon) {
                if (combatConsumables[item.itemId] < item.amount) {
                    result = false;
                    
                }
            }
            else if (itemSubType == ItemSubType.Mat) {
                if (inventoryMaterials[item.itemId] < item.amount) {
                    result = false;
                }
            }
            temp++;
        }
        return result;
    }

	public static bool CheckIfExists(RecipeMaterial mat){
		ItemType type = mat.type;
		if (type == ItemType.Wep) {
			foreach (InventoryWeapon invWep in inventoryWeapons) {
				if (invWep.subType == mat.subtype.ToString ()) {
					return true;
				}
			}
		} else if (type == ItemType.Arm) {
			foreach (InventoryArmor invArm in inventoryArmor) {
				if (invArm.subType == mat.subtype.ToString ()) {
					return true;
				}
			}
		} else {
			Debug.LogError ("NonEquipment asked from single (CHECKIFEXISTS)");
		}
		return false;
	}

    //hae itemi inventorysta ja poista se
	public static bool RemoveItem(ItemType itemType, ItemSubType subType, int itemId, int amount) {
        bool Success = false;
        switch (itemType) {
            case ItemType.Wep:
                for (int i = 0; i <= inventoryWeapons.Count; i++) {
                    if (itemId == inventoryWeapons[i].itemID) {
                        inventoryWeapons.RemoveAt(i);
                        capacity--;
                        Success = true;
                    }
                }
                break;
            case ItemType.Arm:
                for (int i = 0; i <= inventoryArmor.Count; i++) {
				if(itemId == inventoryArmor[i].itemID && subType.ToString() == inventoryArmor[i].subType) {
                        inventoryArmor.RemoveAt(i);
                        capacity--;
                        Success = true;
                    }
                }
                break;
            case ItemType.Cons:
                if (subType == ItemSubType.NonCom) {
                    if (nonCombatConsumables[itemId] < amount) {
                        break;
                    }
                    else {
                        nonCombatConsumables[itemId] -= amount;
                        Success = true;
                    }
                }
                else if (subType == ItemSubType.ComCon) {
                    if ( combatConsumables[itemId] < amount) {
                        break;
                    }
                    else {
                        combatConsumables[itemId] -= amount;
                        Success = true;
                    }
                }
                break;
            case ItemType.Mat:
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
	public static bool PutItem(ItemType itemType, ItemSubType subType, int itemId, int amount) {
        bool Success = false;
        switch (itemType) {
            case ItemType.Wep:
                if (capacity + 1 > maxCapacity) {
                    break;
                }
                else {
					InventoryWeapon newWeapon = new InventoryWeapon(itemId, subType.ToString());
                    inventoryWeapons.Add(newWeapon);
                    capacity++;
                    Success = true;
                    break;
                }
            case ItemType.Arm:
                if (capacity + 1 > maxCapacity) {
                    break;
                }
                else {
					InventoryArmor newArmor = new InventoryArmor(itemId, subType.ToString());
                    inventoryArmor.Add(newArmor);
                    capacity++;
                    Success = true;
                    break;
                }
            case ItemType.Cons:
                if(subType == ItemSubType.NonCom) {
                    if (nonCombatConsumables[itemId] + amount > maxStack) {
                        break;
                    }
                    else {
                        nonCombatConsumables[itemId] += amount;
                        Success = true;
                    }
                }
                else if (subType == ItemSubType.ComCon){
                    if (combatConsumables[itemId] + amount > maxStack){
                        break;
                    }
                    else {
                        combatConsumables[itemId] += amount;
                        Success = true;
                    }    
                }
                break;
            case ItemType.Mat:
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


	public static int GetAmountInInventory (ItemType type, ItemSubType subtype, int id){
		switch (type) {
		case ItemType.Mat:
			return inventoryMaterials [id];
		case ItemType.Cons:
			if (subtype == ItemSubType.ComCon) {
				return combatConsumables [id];
			} else if (subtype == ItemSubType.NonCom) {
				return nonCombatConsumables [id];
			} else {
				Debug.LogError ("False Subtype");
			}
			break;
		default:
			break;
		}
		return -1;
	}

	public static int GetAmountInInventoryRecipMat(RecipeMaterial mat){
		return GetAmountInInventory (mat.type, mat.subtype, mat.itemId);
	}



	public static bool InsertRecipeMaterial(RecipeMaterial recMat){
		return PutItem (recMat.type, recMat.subtype, recMat.itemId, recMat.amount);
	}

	public static bool RemoveRecipeMaterial(RecipeMaterial recMat){
		return RemoveItem (recMat.type, recMat.subtype, recMat.itemId, recMat.amount);
	}
     

}
