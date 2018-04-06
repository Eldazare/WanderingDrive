using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CraftingRecipeType{
	weapon, armor, accessory, conCon, nonconCon
};

public enum EquipmentSubtype{
	sword, axe, spear, dagger, pistol, bow, greatbow, buckler, towershield,
	helm, chest, arms, legs, boots,
	accessory
};

public enum ItemType{
	wep, armor, cons, mat
};

public enum ItemSubType {
	nonCom, comCon, mat, 
	sword, axe, spear, dagger, pistol, bow, Gbow, shieldS, shieldL,
	helm, chest, arms, legs, boots,
	accessory
};

public static class RecipeContainer {


	static bool generated = false;

	public static List<List<Recipe>> craftingRecipes = new List<List<Recipe>> (){ };
	public static List<List<RecipeUpgrade>> equipmentUpgrades = new List<List<RecipeUpgrade>>(){};


	public static void GenerateRecipes(List<string> recipeNames){
		if (!generated){
			for (int i = 0; i < System.Enum.GetNames (typeof(EquipmentSubtype)).Length; i++) {
				craftingRecipes.Add (new List<Recipe> ());
				equipmentUpgrades.Add (new List<RecipeUpgrade> ());
			}
			generated = true;
			foreach (string recipeName in recipeNames) {
				string[] splitStr = recipeName.Split ("_".ToCharArray ());
				if (splitStr [0] == "recipeUp") {
					RecipeUpgrade recipeUp = RecipeCreator.CreateUpgradeRecipe (splitStr [1], int.Parse (splitStr [2]));
					EquipmentSubtype eqSubT = (EquipmentSubtype)System.Enum.Parse (typeof(EquipmentSubtype), splitStr [1]);
					equipmentUpgrades [System.Convert.ToInt32 (eqSubT)].Add (recipeUp);
				} else if (splitStr [0] == "recipe") {
					Recipe recipe = RecipeCreator.CreateRecipe (int.Parse (splitStr [2]));
					CraftingRecipeType recipetype = (CraftingRecipeType)System.Enum.Parse (typeof(CraftingRecipeType), splitStr [1]);
					craftingRecipes [System.Convert.ToInt32 (recipetype)].Add (recipe);
				} else {
					Debug.LogError ("Recipe skipped: " + recipeName);
				}
			}
		}
	}


	public static List<Recipe> GetCraftRecipes(CraftingRecipeType recipType){
		return craftingRecipes [System.Convert.ToInt32 (recipType)];
	}


	// equipment subtype and the item id in that subtype that you want improvements FOR.
	public static List<RecipeUpgrade> GetEquipmentUpgradeRecipes (EquipmentSubtype eqSub, int id){
		List<RecipeUpgrade> returnee = new List<RecipeUpgrade> (){ };
		List<RecipeUpgrade> subtypeList = equipmentUpgrades [System.Convert.ToInt32 (eqSub)];
		foreach (RecipeUpgrade upgrade in subtypeList) {
			if (upgrade.baseEquipment.itemId == id) {
				returnee.Add (upgrade);
			}
		}
		return returnee;
	}

}
