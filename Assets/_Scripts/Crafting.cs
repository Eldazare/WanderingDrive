using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{


    public Image recipeImage;

	List<List<int>> recipeList = new List<List<int>> ();
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
    public int currentRecipeType;
    public int currentRecipe;
    public int spriteCounter;

    public int recipeTypeWeapon = 0;
    public int recipeTypeConsumable = 1;
    public int recipeTypeArmor = 2;


    public void Start() {
        Inventory.inventoryMaterials[0] = 2;
        Inventory.nonCombatConsumables[6] = 1;
        FillLists();
		recipeImage.sprite = sprites[0][0];
		currentRecipe = recipeList[0][0];
        currentRecipeType = recipeTypeWeapon;
        currentItemSpriteList = 0;
        spriteCounter = 0;
        
    }

    
    public void FillLists() {
		int recipeTypes = System.Enum.GetNames (typeof(CraftingRecipeType)).Length;
		for (int i = 0; i < recipeTypes; i++) {
			List<int> newRecipeList = new List<int>();
			for (int j = 0; j < RecipeContainer.GetCraftRecipes((CraftingRecipeType)i).Count;j++) {
				newRecipeList.Add (j);
			}
			recipeList.Add (newRecipeList);
		}
    }

    public void ChangeRecipeOnwards() {
        spriteCounter++;
		if (spriteCounter >= recipeList [currentRecipeType].Count) {
			recipeImage.sprite = sprites [currentRecipeType] [0];
			currentRecipe = recipeList [currentRecipeType] [0];
			spriteCounter = 0;
		}
        else {
			recipeImage.sprite = sprites[currentRecipeType][spriteCounter];
			currentRecipe = recipeList[currentRecipeType][spriteCounter];
        }
    }

    public void ChangeRecipeBackwards() {
		spriteCounter--;
		int lastIndex = recipeList [currentRecipeType].Count - 1;
		if (spriteCounter <= 0) {
			recipeImage.sprite = sprites [currentRecipeType] [lastIndex];
			currentRecipe = recipeList [currentRecipeType] [lastIndex];
			spriteCounter = 0;
		}
		else {
			recipeImage.sprite = sprites[currentRecipeType][spriteCounter];
			currentRecipe = recipeList[currentRecipeType][spriteCounter];
		}
    }

	public void ChangeRecipeListTo(CraftingRecipeType recipetype) {
		int recipeIndex = System.Convert.ToInt32 (recipetype);
		recipeImage.sprite = sprites[recipeIndex][0];
		currentRecipe = recipeList[recipeIndex][0];
		currentRecipeType = recipeIndex;
        currentItemSpriteList = 0;
        spriteCounter = 0;
    }

    public void LetsMerge() {
		if (Merge.CombineRecipe ((CraftingRecipeType)currentRecipeType, recipeList [currentRecipeType] [currentRecipe])){

		}
        else {
            Debug.Log("Error, combining failed");
        }
        Debug.Log("onnistui");
        Debug.Log(Inventory.inventoryWeapons[0].subType);

    }
}
