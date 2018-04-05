using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{


    public Image recipeImage;

    public List<int> weaponRecipeList = new List<int>();
    public List<int> consRecipeList = new List<int>();
    public List<int> armorRecipeList = new List<int>();
    public List<Sprite> weaponSprites;
    public List<Sprite> consumableSprites;
    public List<Sprite> armorSprites;


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
        recipeImage.sprite = weaponSprites[0];
        currentRecipe = weaponRecipeList[0];
        currentRecipeType = recipeTypeWeapon;
        currentItemSpriteList = 0;
        spriteCounter = 0;
        
    }

    
    public void FillLists() {
        List<Recipe> newRecipeList = new List<Recipe>();
        newRecipeList = RecipeContainer.GetCraftRecipes(CraftingRecipeTypes.weapon);
        for (int i = 0; i < newRecipeList.Count; i++) {
            weaponRecipeList.Add(i);
        }
        newRecipeList = RecipeContainer.GetCraftRecipes(CraftingRecipeTypes.conCon);
        for (int i = 0; i < newRecipeList.Count; i++) {
            consRecipeList.Add(i);
        }
        newRecipeList = RecipeContainer.GetCraftRecipes(CraftingRecipeTypes.armor);
        for (int i = 0; i < newRecipeList.Count; i++) {
            armorRecipeList.Add(i);
        }
    }

    public void ChangeRecipeOnwards() {
        spriteCounter++;
        if (spriteCounter >= weaponRecipeList.Count) {
            switch (currentItemSpriteList) {
                case 0:
                    recipeImage.sprite = weaponSprites[0];
                    currentRecipe = weaponRecipeList[0];
                    spriteCounter = 0;
                    break;
                case 1:
                    recipeImage.sprite = consumableSprites[0];
                    currentRecipe = consRecipeList[0];
                    spriteCounter = 0;
                    break;
                case 2:
                    recipeImage.sprite = armorSprites[0];
                    currentRecipe = armorRecipeList[0];
                    spriteCounter = 0;
                    break;
            }
        }
        else {
            switch (currentItemSpriteList) {
                case 0:
                    recipeImage.sprite = weaponSprites[spriteCounter];
                    currentRecipe = weaponRecipeList[spriteCounter];
                    break;
                case 1:
                    recipeImage.sprite = consumableSprites[spriteCounter];
                    currentRecipe = consRecipeList[spriteCounter];
                    break;
                case 2:
                    recipeImage.sprite = armorSprites[spriteCounter];
                    currentRecipe = armorRecipeList[spriteCounter];
                    break;
            }
        }
        
    }

    public void ChangeRecipeBackwards() {
        spriteCounter--;
        if (spriteCounter < 0) {
            switch (currentItemSpriteList) {
                case 0:
                    recipeImage.sprite = weaponSprites[weaponSprites.Count - 1];
                    currentRecipe = weaponRecipeList[weaponRecipeList.Count - 1];
                    spriteCounter = weaponSprites.Count - 1;
                    break;
                case 1:
                    recipeImage.sprite = consumableSprites[consumableSprites.Count - 1];
                    currentRecipe = consRecipeList[consRecipeList.Count - 1];
                    spriteCounter = consumableSprites.Count - 1;
                    break;
                case 2:
                    recipeImage.sprite = armorSprites[armorSprites.Count - 1];
                    currentRecipe = armorRecipeList[armorRecipeList.Count - 1];
                    spriteCounter = armorSprites.Count - 1;
                    break;
            }
        }
        else {
            switch (currentItemSpriteList) {
                case 0:
                    recipeImage.sprite = weaponSprites[spriteCounter];
                    currentRecipe = weaponRecipeList[spriteCounter];
                    break;
                case 1:
                    recipeImage.sprite = consumableSprites[spriteCounter];
                    currentRecipe = consRecipeList[spriteCounter];
                    break;
                case 2:
                    recipeImage.sprite = armorSprites[spriteCounter];
                    currentRecipe = armorRecipeList[spriteCounter];
                    break;
            }
        }
    }

    public void ChangeRecipeListToCrafting() {
        recipeImage.sprite = weaponSprites[0];
        currentRecipe = weaponRecipeList[0];
        currentRecipeType = recipeTypeWeapon;
        currentItemSpriteList = 0;
        spriteCounter = 0;
    }

    public void ChangeRecipeListToConsumable() {
        recipeImage.sprite = consumableSprites[0];
        currentRecipe = consRecipeList[0];
        currentRecipeType = recipeTypeConsumable;
        currentItemSpriteList = 1;
        spriteCounter = 0;
    }

    public void ChangeRecipeListToArmor() {
        recipeImage.sprite = armorSprites[0];
        currentRecipe = armorRecipeList[0];
        currentRecipeType = recipeTypeArmor;
        currentItemSpriteList = 2;
        spriteCounter = 0;
    }

    public void LetsMerge() {
        if (currentRecipeType == recipeTypeWeapon) {
            if (!Merge.Combine(currentRecipeType, weaponRecipeList.IndexOf(weaponRecipeList[currentRecipe]))) {
                Debug.Log("No sufficient materials");
            }
        }
        else if (currentRecipeType == recipeTypeConsumable) {
           if (!Merge.Combine(currentRecipeType, consRecipeList.IndexOf(consRecipeList[currentRecipe]))) {
                Debug.Log("No sufficient materials");
            }
        }
        else if (currentRecipeType == recipeTypeArmor) {
            if (!Merge.Combine(currentRecipeType, armorRecipeList.IndexOf(armorRecipeList[currentRecipe]))) {
                Debug.Log("No sufficient materials");
            }
        }
        else {
            Debug.Log("Error, combining failed");
        }
        Debug.Log("onnistui");
        Debug.Log(Inventory.inventoryWeapons[0].subType);

    }
}
