using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecipeCreator {

	//TODO: Material / Consumable name and description implementation to RecipeMaterial

	public static Recipe CreateRecipe(int id){
		Recipe createe = new Recipe ();
		createe.recipeId = id;
		string begin = "recipe_" + id + "_";
		for (int i = 0; i < 4; i++) {
			string matData = DataManager.ReadDataString (begin + "m" + (i + 1));
			if (matData != "") {
				RecipeMaterial mat = new RecipeMaterial (matData);
				createe.materialList.Add (mat);
			}
		}
		string resultData = DataManager.ReadDataString (begin + "result");
		RecipeMaterial resultMat = new RecipeMaterial (resultData);
		createe.resultItem = resultMat;
		return createe;
	}

	public static RecipeUpgrade CreateUpgradeRecipe(string subtype, int id){
		RecipeUpgrade createe = new RecipeUpgrade ();
		createe.id = id;
		string begin = "recipeUp_" + subtype + "_" + id + "_";
		string baseMatStr = DataManager.ReadDataString (begin + "main");
		createe.baseEquipment = new RecipeMaterial (baseMatStr);
		for (int i = 0; i < 4; i++) {
			string matData = DataManager.ReadDataString (begin + "m" + (i + 1));
			if (matData != "") {
				RecipeMaterial mat = new RecipeMaterial (matData);
				createe.materialList.Add (mat);
			}
		}
		string resultData = DataManager.ReadDataString (begin + "result");
		RecipeMaterial resultMat = new RecipeMaterial (resultData);
		createe.result = resultMat;
		return createe;
	}
}
