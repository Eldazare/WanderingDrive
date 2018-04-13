﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour{
    public Image recipeImage;
	public GameObject topPanel; // includes left/right/merge buttons.
	public Text recipeInfo;
	public Text resultInfo;

	// TODO: Sprites will be downloaded per type and per id from Resources. Their id's and counts will match the recipes.

	List<List<Recipe>> recipeList = new List<List<Recipe>> ();
	List<List<Sprite>> sprites = new List<List<Sprite>>();

	/*
    public List<int> weaponRecipeList = new List<int>();
    public List<int> consRecipeList = new List<int>();
    public List<int> armorRecipeList = new List<int>();
    public List<Sprite> weaponSprites;
    public List<Sprite> consumableSprites;
    public List<Sprite> armorSprites;
	*/

    public int currentItemSpriteList;
	public int spriteCounter;

    public int currentRecipeType;
    public int currentRecipe;


    public void Start() {
        LoadRecipes();
		//recipeImage.sprite = sprites[0][0]; // Put sprites to Resources and load from there
		currentRecipe = -1;
        currentRecipeType = -1;
        currentItemSpriteList = -1;
        spriteCounter = -1;
    }

    
    public void LoadRecipes() {
		/*
		int recipeTypes = System.Enum.GetNames (typeof(CraftingRecipeType)).Length;
		for (int i = 0; i < recipeTypes; i++) {
			List<int> newRecipeList = new List<int>();
			for (int j = 0; j < RecipeContainer.GetCraftRecipes((CraftingRecipeType)i).Count;j++) {
				newRecipeList.Add (j);
			}
			recipeList.Add (newRecipeList);
		}
		*/
		recipeList = RecipeContainer.GetAllCraftRecipes ();
    }

    public void ChangeRecipeOnwards() {
		/*
        spriteCounter++;
		if (spriteCounter >= recipeList [currentRecipeType].Count) {
			//recipeImage.sprite = sprites [currentRecipeType] [0];
			currentRecipe = recipeList [currentRecipeType] [0];
			spriteCounter = 0;
		}
        else {
			recipeImage.sprite = sprites[currentRecipeType][spriteCounter];
			currentRecipe = recipeList[currentRecipeType][spriteCounter];
        }
        */
		currentRecipe++;
		if (currentRecipe >= recipeList [currentRecipeType].Count) {
			currentRecipe = 0;
		}
		UpdateInfoTexts ();
    }

    public void ChangeRecipeBackwards() {
		/*
		spriteCounter--;
		int lastIndex = recipeList [currentRecipeType].Count - 1;
		if (spriteCounter < 0) {
			//recipeImage.sprite = sprites [currentRecipeType] [lastIndex];
			currentRecipe = recipeList [currentRecipeType] [lastIndex];
			spriteCounter = 0;
		}
		else {
			recipeImage.sprite = sprites[currentRecipeType][spriteCounter];
			currentRecipe = recipeList[currentRecipeType][spriteCounter];
		}
		*/
		currentRecipe--;
		if (currentRecipe < 0) {
			currentRecipe = recipeList [currentRecipeType].Count - 1;
		}
		UpdateInfoTexts ();
    }

	public void ChangeRecipeListTo(int recipeIndex) {
		topPanel.SetActive (true);
		Debug.Log ("Recipes changed to " + ((CraftingRecipeType)recipeIndex).ToString ());
		//recipeImage.sprite = sprites[recipeIndex][0];
		currentRecipe = 0;
		currentRecipeType = recipeIndex;

        currentItemSpriteList = 0;
        spriteCounter = 0;
		UpdateInfoTexts ();
    }

    public void LetsMerge() {
		if (Merge.CombineRecipe (recipeList[currentRecipeType] [currentRecipe])){
			Debug.Log ("Merge'd");
		}
        else {
            Debug.Log("Error, combining failed");
        }
    }

	private void UpdateInfoTexts(){
		recipeInfo.text = InfoBoxCreator.GetRecipeInfoString (recipeList [currentRecipeType] [currentRecipe]);
		resultInfo.text = InfoBoxCreator.GetMaterialInfoString (recipeList [currentRecipeType] [currentRecipe].resultItem);
	}
}
