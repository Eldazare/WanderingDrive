using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merge {

    int id;
    List<int> list;
    Inventory inventory;
    private Item_Recipes itemRecipes;
    private Consumable_Recipe consumableRecipes;


    public Merge() { }

    public Merge(List<int> _list, int _id) {
        this.list = _list;
        this.id = _id;

    }

    public void Conbine(List<int> _list, int _RecipeID)
    {
        _list = list;
        _RecipeID = id;
        if (id == 0) {
            if (true)
            {
                

                //Hae ja poista tavarat inventorysta
                foreach (int i in _list)
                {
                    inventory.GetItem(i);
                }
                //Luo uusi tavara ja laita se inventoryyn
                itemRecipes.GetData(_RecipeID);
                Item _item = new Item();
                inventory.PutItem(_item.GetID());
            }

            
            
        }
        if (id == 1)
        {
            //Hae ja poista tavarat inventorysta
            foreach (int i in _list)
            {
                inventory.GetItem(i);
            }
            //Luo uusi tavara ja laita se inventoryyn
            itemRecipes.GetData(_RecipeID);
            Item _item = new Item();
            inventory.PutItem(_item.GetID());

        }

        
    }

}
