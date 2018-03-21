using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour {

    public Inventory inventory;
    public List<Recipe> ItemRecipeList;
    public List<Recipe> ConsRecipeList;
    public List<Sprite> ItemSprites;
    public List<Sprite> ConsumableSprites;
    private List<Sprite> CurrentItemSpriteList;
    private List<Sprite> CurrentConsumableSpriteList;
    public Merge merger;
    private Recipe CurrentItemRecipe;
    private Recipe CurrentConsumableRecipe;
    public Image RecipeImage;
    private int spriteCounter;

    public void Start()
    {
        RecipeImage.sprite = ItemSprites[0];
        CurrentItemRecipe = ItemRecipeList[0];
        CurrentItemSpriteList = ItemSprites;
        spriteCounter = 0;
    }


    public void ChangeRecipeOnwards()
    {

        spriteCounter++;
        if (spriteCounter >= ItemSprites.Count)
        {
            RecipeImage.sprite = ItemSprites[0];
            spriteCounter = 0;
        }
        else
        {   
            RecipeImage.sprite = ItemSprites[spriteCounter];
        }
    }

    public void ChangeRecipeBackwards()
    {

        spriteCounter--;
        if (spriteCounter < 0)
        {
            RecipeImage.sprite = ItemSprites[ItemSprites.Count - 1];
            spriteCounter = ItemSprites.Count - 1;
        }
        else
        {
            RecipeImage.sprite = ItemSprites[spriteCounter];
        }
    }

    public void ChangeRecipeListToCrafting()
    {
        RecipeImage.sprite = ItemSprites[0];
        CurrentItemRecipe = ItemRecipeList[0];
        CurrentItemSpriteList = ItemSprites;
        spriteCounter = 0;
    }

    public void ChangeRecipeListToConsumable()
    {
        RecipeImage.sprite = ConsumableSprites[0];
        CurrentItemRecipe = ConsRecipeList[0];
        CurrentItemSpriteList = ConsumableSprites;
        spriteCounter = 0;
    }


    public void LetsMerge()
    {

    }
}
