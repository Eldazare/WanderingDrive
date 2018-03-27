using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecipeCreator {

	//TODO: Material / Consumable name and description container

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
}
