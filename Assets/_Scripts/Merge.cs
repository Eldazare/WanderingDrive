using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merge {

  
    List<int> list;
    Inventory MyInventory;
    Recipe_Manager recipe_manager;



    public Merge() { }

    public Merge(List<int> _list ) {
        this.list = _list;
       

    }

    public void Conbine(int RecipeType, int RecipeID)
    {
        
        //Hae resepti
        Recipe newRecipe = recipe_manager.GetRecipeData(RecipeType, RecipeID);
        //luo uusi tavara
        for (int i = 0; i <= newRecipe.MaterialList.Count; i++)
        {
            MyInventory.RemoveItem(newRecipe.MaterialList[i].GetItemtype(), newRecipe.MaterialList[i].GetItemID());
        }
        MyInventory.PutItem(newRecipe.newItemType, newRecipe.newItemID);

    }


}
