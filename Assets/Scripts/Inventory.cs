using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public List<List<int>> items_In_Inventory;
    public List<int> inventory_Armor;
    public List<int> inventory_Accessory;
    public List<int> inventory_Weapons;
    public List<int> inventory_Materials;
    public List<int> combat_consumables;
    public List<int> non_combat_consumables;

    private void Start()
    {
        items_In_Inventory[0] = non_combat_consumables;
        items_In_Inventory[1] = combat_consumables;
        items_In_Inventory[2] = inventory_Armor;
        items_In_Inventory[3] = inventory_Accessory;
        items_In_Inventory[4] = inventory_Weapons;
        items_In_Inventory[5] = inventory_Materials;
    }

    //hae itemi inventorysta ja poista se
    public void GetItem(int ItemType, int ItemID)
    {
        
        

    }

    public void PutItem(int ItemType, int ItemID)
    {
        //Laita uusi Itemi inventoryyn
    }

}
