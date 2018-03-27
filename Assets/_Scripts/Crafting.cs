using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour {

    public Merge merger;
    public Image RecipeImage;

    public List<int> ItemRecipeList;
    public List<int> ConsRecipeList;
    public List<Sprite> ItemSprites;
    public List<Sprite> ConsumableSprites;
    private int CurrentItemSpriteList;
    private List<Sprite> CurrentConsumableSpriteList;
   
    public int CurrentRecipeType;
    public int CurrentRecipe;
    private int spriteCounter;

    public int RecipeType_Item = 0;
    public int RecipeType_Consumable = 1;
    public int RecipeType_Armor = 2;
    public int RecipeType_Weapon = 3;
    public int RecipeType_Accessory = 4;

    public void Start()
    {
        RecipeImage.sprite = ItemSprites[0];
        CurrentRecipe = ItemRecipeList[0];
        CurrentRecipeType = RecipeType_Item;
        CurrentItemSpriteList = 0;
        spriteCounter = 0;
        
    }


    public void ChangeRecipeOnwards()
    {

        
        spriteCounter++;
        if (spriteCounter >= ItemSprites.Count)
        {
            switch (CurrentItemSpriteList)
            {
                case 0:
                    RecipeImage.sprite = ItemSprites[0];
                    CurrentRecipe = ItemRecipeList[0];
                    spriteCounter = 0;
                    break;
                case 1:
                    RecipeImage.sprite = ConsumableSprites[0];
                    CurrentRecipe = ConsRecipeList[0];
                    spriteCounter = 0;
                    break;
            }
        }
        else
        {
            switch (CurrentItemSpriteList)
            {
                case 0:
                    RecipeImage.sprite = ItemSprites[spriteCounter];
                    CurrentRecipe = ItemRecipeList[spriteCounter];
                    break;
                case 1:
                    RecipeImage.sprite = ConsumableSprites[spriteCounter];
                    CurrentRecipe = ConsRecipeList[spriteCounter];
                    break;
            }
        }
    }

    public void ChangeRecipeBackwards()
    {

        spriteCounter--;
        if (spriteCounter < 0)
        {
            switch (CurrentItemSpriteList)
            {
                case 0:
                    RecipeImage.sprite = ItemSprites[ItemSprites.Count - 1];
                    CurrentRecipe = ItemRecipeList[ItemRecipeList.Count - 1];
                    spriteCounter = ItemSprites.Count - 1;
                    break;
                case 1:
                    RecipeImage.sprite = ConsumableSprites[ConsumableSprites.Count - 1];
                    CurrentRecipe = ConsRecipeList[ConsRecipeList.Count - 1];
                    spriteCounter = ConsumableSprites.Count - 1;
                    break;
            }
        }
        else
        {
            switch (CurrentItemSpriteList)
            {
                case 0:
                    RecipeImage.sprite = ItemSprites[spriteCounter];
                    CurrentRecipe = ItemRecipeList[spriteCounter];
                    break;
                case 1:
                    RecipeImage.sprite = ConsumableSprites[spriteCounter];
                    CurrentRecipe = ConsRecipeList[spriteCounter];
                    break;
            }
        }
    }

    public void ChangeRecipeListToCrafting()
    {
        RecipeImage.sprite = ItemSprites[0];
        CurrentRecipe = ItemRecipeList[0];
        CurrentRecipeType = RecipeType_Item;
        CurrentItemSpriteList = 0;
        spriteCounter = 0;
    }

    public void ChangeRecipeListToConsumable()
    {
        RecipeImage.sprite = ConsumableSprites[0];
        CurrentRecipe = ConsRecipeList[0];
        CurrentRecipeType = RecipeType_Consumable;
        CurrentItemSpriteList = 1;
        spriteCounter = 0;
    }


    public void LetsMerge()
    {
        //merger.Conbine(CurrentRecipeType, CurrentRecipe);

    }
}
