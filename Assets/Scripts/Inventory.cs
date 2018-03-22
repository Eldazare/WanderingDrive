using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<int> [] items_In_Inventory = new List <int> [5];
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
    public void RemoveItem(int ItemType, int ItemID)
    {
        foreach (int i in items_In_Inventory[ItemType])
        {
            if(i == ItemID)
            {
                items_In_Inventory[ItemType].RemoveAt(i);
            }
        }
        

    }
    
    //Laita uusi Itemi inventoryyn
    public void PutItem(int ItemType, int ItemID)
    {
        switch (ItemType)
        {
            case 0:
                items_In_Inventory[0].Add(ItemID);
                break;
            case 1:
                items_In_Inventory[1].Add(ItemID);
                break;
            case 2:
                items_In_Inventory[2].Add(ItemID);
                break;
            case 3:
                items_In_Inventory[3].Add(ItemID);
                break;
            case 4:
                items_In_Inventory[4].Add(ItemID);
                break;
            case 5:
                items_In_Inventory[5].Add(ItemID);
                break;
        }
        
    }

}
