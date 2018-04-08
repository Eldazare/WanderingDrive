using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Merge {


    //RecipeType_Weapon = 0; 
    //RecipeType_CombatConsumable = 1;
    //RecipeType_Armor = 2;

    //TODO: Convert switch to instead use the enum

	public static  bool Combine(CraftingRecipeType recipeType, int recipeID) {
        bool success = true;
        List<Recipe> newRecipeList = new List<Recipe>();
		Recipe newRecipe = new Recipe(recipeType);
        //katsotaan ylhäällä määrättyjen tyyppien mukaan mitä listaa käytetään
        switch (recipeType) {
		case (CraftingRecipeType)0:
                //Hae resepti
				newRecipeList = RecipeContainer.GetCraftRecipes(CraftingRecipeType.Weapon);
                foreach (Recipe i in newRecipeList){
                    if (recipeID == i.recipeId) {
                        newRecipe = i;
                        Debug.Log("resepti löyty~");
                    }
                }
                //katso löytyykö tarvittavat tavarat
                if (!Inventory.CheckIfExists(newRecipe.materialList)) {
                    return false;
                }
                //Poistetaan tarvittavat materiaalit inventorysta
                for (int i = 0; i < newRecipe.materialList.Count; i++) {
                    Inventory.RemoveItem(newRecipe.materialList[i].type, newRecipe.materialList[i].subtype, newRecipe.materialList[i].itemId, newRecipe.materialList[i].amount);
                }
                //luodaan uusi item inventoryyn
                Inventory.PutItem(newRecipe.resultItem.type, newRecipe.resultItem.subtype, newRecipe.resultItem.itemId, newRecipe.resultItem.amount);
                break;
		case (CraftingRecipeType)1:
			    newRecipeList = RecipeContainer.GetCraftRecipes(CraftingRecipeType.Armor);
                foreach (Recipe i in newRecipeList) {
                    if (recipeID == i.recipeId) {
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
		case (CraftingRecipeType)2:
			newRecipeList = RecipeContainer.GetCraftRecipes(CraftingRecipeType.NonCom);
                foreach (Recipe i in newRecipeList) {
                    if (recipeID == i.recipeId) {
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
		case (CraftingRecipeType)3:
                newRecipeList = RecipeContainer.GetCraftRecipes(CraftingRecipeType.ComCon);
                foreach (Recipe i in newRecipeList)
                {
                    if (recipeID == i.recipeId)
                    {
                        newRecipe = i;
                    }
                }
                if (!Inventory.CheckIfExists(newRecipe.materialList))
                {
                    return false;
                }
                for (int i = 0; i <= newRecipe.materialList.Count; i++)
                {
                    Inventory.RemoveItem(newRecipe.materialList[i].type, newRecipe.materialList[i].subtype, newRecipe.materialList[i].itemId, newRecipe.materialList[i].amount);
                }
                Inventory.PutItem(newRecipe.resultItem.type, newRecipe.resultItem.subtype, newRecipe.resultItem.itemId, newRecipe.resultItem.amount);
                break;
        }

        return success;
    }
}
