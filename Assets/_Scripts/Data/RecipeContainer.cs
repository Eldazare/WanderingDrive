using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecipeContainer {


	static bool generated = false;

	public enum CraftingRecipeTypes{weapon, armor, accessory, conCon, nonconCon};
	public enum EquipmentSubtypes{
		sword, axe, spear, dagger, pistol, bow, greatbow, buckler, towershield,
		helm, chest, arms, legs, boots,
		accessory
	};

	public static List<List<Recipe>> craftingRecipes = new List<List<Recipe>> (){ };
	public static List<List<RecipeUpgrade>> equipmentUpgrades = new List<List<RecipeUpgrade>>(){};


	public static void GenerateRecipes(List<string> recipeNames){
		if (!generated){
			for (int i = 0; i < System.Enum.GetNames (typeof(EquipmentSubtypes)).Length; i++) {
				craftingRecipes.Add (new List<Recipe> ());
				equipmentUpgrades.Add (new List<RecipeUpgrade> ());
			}
			generated = true;
			foreach (string recipeName in recipeNames) {
				string[] splitStr = recipeName.Split ("_".ToCharArray ());
				if (splitStr [0] == "recipeUp") {
					RecipeUpgrade recipeUp = RecipeCreator.CreateUpgradeRecipe (splitStr [1], int.Parse (splitStr [2]));
					EquipmentSubtypes eqSubT = (EquipmentSubtypes)System.Enum.Parse (typeof(EquipmentSubtypes), splitStr [1]);
					equipmentUpgrades [System.Convert.ToInt32 (eqSubT)].Add (recipeUp);
				} else if (splitStr [0] == "recipe") {
					Recipe recipe = RecipeCreator.CreateRecipe (int.Parse (splitStr [2]));
					CraftingRecipeTypes recipetype = (CraftingRecipeTypes)System.Enum.Parse (typeof(CraftingRecipeTypes), splitStr [1]);
					craftingRecipes [System.Convert.ToInt32 (recipetype)].Add (recipe);
				} else {
					Debug.LogError ("Recipe skipped: " + recipeName);
				}
			}
		}
	}


	public static List<Recipe> GetCraftRecipes(CraftingRecipeTypes recipType){
		return craftingRecipes [System.Convert.ToInt32 (recipType)];
	}


	// equipment subtype and the item id in that subtype that you want improvements FOR.
	public static List<RecipeUpgrade> GetEquipmentUpgradeRecipes (EquipmentSubtypes eqSub, int id){
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
