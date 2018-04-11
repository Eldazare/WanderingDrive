using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ENUMIT saatana

public static class Merge {
	public static bool CombineRecipe(CraftingRecipeType recipeType, int recipeID) {
		List<Recipe> newRecipeList = RecipeContainer.GetCraftRecipes(recipeType);
		Recipe newRecipe = newRecipeList [recipeID];
		if (newRecipe.recipeId != recipeID) {
			Debug.LogError ("FUCKED UP SON (Recipe)");
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
		return true;
	}

	public static bool CombineUpgradeRecipe(RecipeUpgrade upgradeRecipe){
		if (!Inventory.CheckIfExists(upgradeRecipe.materialList)){
			return false;
		}
		if (!Inventory.CheckIfExists (upgradeRecipe.baseEquipment)) {
			return false;
		}
		//Poistetaan tarvittavat materiaalit inventorysta
		Inventory.RemoveRecipeMaterial(upgradeRecipe.baseEquipment);
		for (int i = 0; i < upgradeRecipe.materialList.Count; i++) {
			Inventory.RemoveRecipeMaterial(upgradeRecipe.materialList[i]);
		}
		//luodaan uusi item inventoryyn
		Inventory.InsertRecipeMaterial(upgradeRecipe.result);
		return true;
	}
}
