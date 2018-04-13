using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Recipe {

	public CraftingRecipeType subtype;
    public int recipeId;
	public List<RecipeMaterial> materialList;
	public RecipeMaterial resultItem;

	public Recipe(CraftingRecipeType recipeType) {
		this.subtype = recipeType;
		materialList = new List<RecipeMaterial>{ };
	}

	public string GetRecipeInfo(){
		string returnee = "";
		for (int i = 0; i < 4; i++) {
			if (i < materialList.Count) {
				string materialName = materialList [i].GetName ();
				int inventoryAmount = Inventory.GetAmountInInventoryRecipMat (materialList [i]);
				returnee += i + ": " + string.Format ("{0,-20}", materialName) + string.Format ("{0:00}", inventoryAmount) + "/" +
				string.Format ("{0:00}", materialList [i].amount);
			}
			returnee += "\n";
		}
		string resultName = resultItem.GetName ();
		returnee += ">> " + resultName;
		return returnee;
	}
}
