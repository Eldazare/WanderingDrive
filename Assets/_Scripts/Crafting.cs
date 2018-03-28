using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour {

    public Merge Merger;
    public Image recipeImage;
    public int weaponRecipeNumber, consRecipeNumber, armorRecipeNumber;

    public List<int> weaponRecipeList;
    public List<int> consRecipeList;
    public List<int> armorRecipeList;
    public List<Sprite> weaponSprites;
    public List<Sprite> consumableSprites;
    public List<Sprite> armorSprites;
    private int currentItemSpriteList;
    List<Recipe> tempRecipeList = new List<Recipe>();


    public int currentRecipeType;
    public int currentRecipe;
    private int spriteCounter;

    public int recipeTypeWeapon = 0;
    public int recipeTypeConsumable = 1;
    public int recipeTypeArmor = 2;


    public void Start() {
        weaponRecipeList = new List<int>(weaponRecipeNumber);
        consRecipeList = new List<int>(consRecipeNumber);
        armorRecipeList = new List<int>(armorRecipeNumber);
        recipeImage.sprite = weaponSprites[0];
        currentRecipe = weaponRecipeList[0];
        currentRecipeType = recipeTypeWeapon;
        currentItemSpriteList = 0;
        spriteCounter = 0;
        
    }


    public void ChangeRecipeOnwards() {
        spriteCounter++;
        if (spriteCounter >= weaponSprites.Count) {
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
        if (currentRecipeType == recipeTypeWeapon){
            Merger.Combine(currentRecipeType, weaponRecipeList.IndexOf(weaponRecipeList[currentRecipe]));
        }
        else if (currentRecipeType == recipeTypeConsumable) {
            Merger.Combine(currentRecipeType, consRecipeList.IndexOf(consRecipeList[currentRecipe]));
        }
        else if (currentRecipeType == recipeTypeArmor) {
            Merger.Combine(currentRecipeType, armorRecipeList.IndexOf(armorRecipeList[currentRecipe]));
        }
    }
}
