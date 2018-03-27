using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merge {

    Inventory MyInventory;
    //RecipeType_Weapon = 0; 
    //RecipeType_Consumable = 1;
    //RecipeType_Armor = 2;

    public Merge() { }
	
    public void Conbine(int RecipeType, int RecipeID) {
        List<Recipe> newRecipeList = new List<Recipe>();
        Recipe newRecipe = new Recipe();
        //katsotaan ylhäällä määrättyjen tyyppien mukaan mitä listaa käytetään
        switch (RecipeType) {
            case 0:
                //Hae resepti
                newRecipeList = RecipeContainer.GetWeaponCraftRecipes();
                foreach (Recipe i in newRecipeList){
                    if (RecipeID == i.recipeId) {
                        newRecipe = i;
                    }
                }
                //Poistetaan tarvittavat materiaalit inventorysta
                for (int i = 0; i <= newRecipe.materialList.Count; i++) {
                    MyInventory.RemoveItem(newRecipe.materialList[i].type, newRecipe.materialList[i].subtype, newRecipe.materialList[i].itemId, newRecipe.materialList[i].amount);
                }
                //luodaan uusi item inventoryyn
                MyInventory.PutItem(newRecipe.resultItem.type, newRecipe.resultItem.subtype, newRecipe.resultItem.itemId, newRecipe.resultItem.amount);
                break;
            case 1:
                newRecipeList = RecipeContainer.GetConsumableCraftRecipes();
                foreach (Recipe i in newRecipeList) {
                    if (RecipeID == i.recipeId) {
                        newRecipe = i;
                    }
                }
                for (int i = 0; i <= newRecipe.materialList.Count; i++) {
                    MyInventory.RemoveItem(newRecipe.materialList[i].type, newRecipe.materialList[i].subtype, newRecipe.materialList[i].itemId, newRecipe.materialList[i].amount);
                }
                MyInventory.PutItem(newRecipe.resultItem.type, newRecipe.resultItem.subtype, newRecipe.resultItem.itemId, newRecipe.resultItem.amount);
                break;
            case 2:
                newRecipeList = RecipeContainer.GetArmorCraftRecipes();
                foreach (Recipe i in newRecipeList) {
                    if (RecipeID == i.recipeId) {
                        newRecipe = i;
                    }
                }
                for (int i = 0; i <= newRecipe.materialList.Count; i++) {
                    MyInventory.RemoveItem(newRecipe.materialList[i].type, newRecipe.materialList[i].subtype, newRecipe.materialList[i].itemId, newRecipe.materialList[i].amount);
                }
                MyInventory.PutItem(newRecipe.resultItem.type, newRecipe.resultItem.subtype, newRecipe.resultItem.itemId, newRecipe.resultItem.amount);
                break;
        }

    }
}
