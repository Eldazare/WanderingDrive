using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecipeContainer {

	// TODO: Differentiate between upgrade recipes



	public static List<Recipe> weaponCrafts = new List<Recipe>();
	public static List<Recipe> consumableCrafts = new List<Recipe>();

	public static List<Recipe> swordUpgrades = new List<Recipe>();
	public static List<Recipe> axeUpgrades = new List<Recipe>();
	public static List<Recipe> spearUpgrades = new List<Recipe>();
	public static List<Recipe> daggerUpgrades = new List<Recipe>();
	public static List<Recipe> pistolUpgrades = new List<Recipe>();
	public static List<Recipe> bowUpgrades = new List<Recipe>();
	public static List<Recipe> greatbowUpgrades = new List<Recipe>();
	public static List<Recipe> bucklerUpgrades = new List<Recipe>();
	public static List<Recipe> towershieldUpgrades = new List<Recipe>();

	public static List<Recipe> armorHelmUpgrades = new List<Recipe>();
	public static List<Recipe> armorChestUpgrades = new List<Recipe>();
	public static List<Recipe> armorArmsUpgrades = new List<Recipe>();
	public static List<Recipe> armorLegsUpgrades = new List<Recipe>();
	public static List<Recipe> armorBootsUpgrades = new List<Recipe>();


	public static void GenerateRecipes(){
		// Read name data and use RecipeCreator
	}

	public static List<Recipe> GetWeaponCraftRecipes(){
		return null;

	}

	public static List<Recipe> GetConsumableCraftRecipes(){
		return null;
	}

	public static List<Recipe> GetWeaponUpgradeRecipes (string subtype, int id){
		return null;
	}

	public static List<Recipe> GetArmorUpgradeRecipes (string subtype, int id){
		return null;
	}
}
