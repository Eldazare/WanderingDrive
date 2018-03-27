using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecipeContainer {

	public static List<Recipe> weaponCrafts = new List<Recipe>();
	public static List<Recipe> consumableCrafts = new List<Recipe>();
    public static List<Recipe> armorCrafts = new List<Recipe>();
	// TODO: Upgrade recipe & lists

	public static void GenerateRecipes(){
		// Read name data and use RecipeCreator
	}

	public static List<Recipe> GetWeaponCraftRecipes(){
		return null;

	}

	public static List<Recipe> GetConsumableCraftRecipes(){
		return null;
	}

    public static List<Recipe> GetArmorCraftRecipes()
    {
        return null;
    }
}
