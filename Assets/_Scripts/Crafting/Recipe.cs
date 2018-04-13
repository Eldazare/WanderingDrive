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
		
}
