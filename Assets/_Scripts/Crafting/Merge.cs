using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Merge {

	public static bool CombineRecipe(Recipe recipe){
		//katso löytyykö tarvittavat tavarat
		if (!Inventory.CheckIfExists(recipe.materialList)) {
			return false;
		}
		//Poistetaan tarvittavat materiaalit inventorysta
		for (int i = 0; i < recipe.materialList.Count; i++) {
			Inventory.RemoveRecipeMaterial(recipe.materialList[i]);
		}
		//luodaan uusi item inventoryyn
		Inventory.InsertRecipeMaterial(recipe.resultItem);
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
