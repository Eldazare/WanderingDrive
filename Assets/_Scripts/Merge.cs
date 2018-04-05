using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Merge {


    //RecipeType_Weapon = 0; 
    //RecipeType_CombatConsumable = 1;
    //RecipeType_Armor = 2;

    

	//TODO: Convert to static
	//TODO: Convert switch to instead use the enum

    public static  bool Combine(int RecipeType, int RecipeID) {
        bool success = true;
        List<Recipe> newRecipeList = new List<Recipe>();
        Recipe newRecipe = new Recipe();
        //katsotaan ylhäällä määrättyjen tyyppien mukaan mitä listaa käytetään
        switch (RecipeType) {
            case 0:
                //Hae resepti
				newRecipeList = RecipeContainer.GetCraftRecipes(RecipeContainer.CraftingRecipeTypes.weapon);
                foreach (Recipe i in newRecipeList){
                    if (RecipeID == i.recipeId) {
                        newRecipe = i;
                    }
                }
                //katso löytyykö tarvittavat tavarat
                if (!Inventory.CheckIfExists(newRecipe.materialList)) {
                    return false;
                }
                //Poistetaan tarvittavat materiaalit inventorysta
                for (int i = 0; i <= newRecipe.materialList.Count; i++) {
                    Inventory.RemoveItem(newRecipe.materialList[i].type, newRecipe.materialList[i].subtype, newRecipe.materialList[i].itemId, newRecipe.materialList[i].amount);
                }
                //luodaan uusi item inventoryyn
                Inventory.PutItem(newRecipe.resultItem.type, newRecipe.resultItem.subtype, newRecipe.resultItem.itemId, newRecipe.resultItem.amount);
                break;
            case 1:
			    newRecipeList = RecipeContainer.GetCraftRecipes(RecipeContainer.CraftingRecipeTypes.conCon);
                foreach (Recipe i in newRecipeList) {
                    if (RecipeID == i.recipeId) {
                        newRecipe = i;
                    }
                }
                if (!Inventory.CheckIfExists(newRecipe.materialList)) {
                    return false;
                }
                for (int i = 0; i <= newRecipe.materialList.Count; i++) {
                    Inventory.RemoveItem(newRecipe.materialList[i].type, newRecipe.materialList[i].subtype, newRecipe.materialList[i].itemId, newRecipe.materialList[i].amount);
                }
                Inventory.PutItem(newRecipe.resultItem.type, newRecipe.resultItem.subtype, newRecipe.resultItem.itemId, newRecipe.resultItem.amount);
                break;
            case 2:
			    newRecipeList = RecipeContainer.GetCraftRecipes(RecipeContainer.CraftingRecipeTypes.armor);
                foreach (Recipe i in newRecipeList) {
                    if (RecipeID == i.recipeId) {
                        newRecipe = i;
                    }
                }
                if (!Inventory.CheckIfExists(newRecipe.materialList)) {
                    return false;
                }
                for (int i = 0; i <= newRecipe.materialList.Count; i++) {
                    Inventory.RemoveItem(newRecipe.materialList[i].type, newRecipe.materialList[i].subtype, newRecipe.materialList[i].itemId, newRecipe.materialList[i].amount);
                }
                Inventory.PutItem(newRecipe.resultItem.type, newRecipe.resultItem.subtype, newRecipe.resultItem.itemId, newRecipe.resultItem.amount);
                break;
        }

        return success;
    }
}
