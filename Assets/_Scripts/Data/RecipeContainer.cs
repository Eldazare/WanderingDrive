using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecipeContainer {

	// TODO: UPGRADE RECIPES


	static bool generated = false;


	public static List<Recipe> weaponCrafts = new List<Recipe>();
	public static List<Recipe> consumableCombatCrafts = new List<Recipe>();
	public static List<Recipe> consumableNonconCrafts = new List<Recipe> ();

	/*
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
	*/

	public static void GenerateRecipes(List<string> recipeNames){
		if (!generated){
			generated = true;
			foreach (string str in recipeNames) {
				string[] splitStr = str.Split ("_".ToCharArray ());
				Recipe recipe = RecipeCreator.CreateRecipe (int.Parse (splitStr [2]));
				switch (splitStr [1]) {
				case "weapon":
					weaponCrafts.Add (recipe);
					break;
				case "combat":
					consumableCombatCrafts.Add (recipe);
					break;
				case "noncon":
					consumableNonconCrafts.Add (recipe);
					break;
				default:
					Debug.LogError ("False line ine recipe names (" + str + ").");
					break;
				}
			}
		}
	}

	public static List<Recipe> GetWeaponCraftRecipes(){
		return weaponCrafts;
	}

	public static List<Recipe> GetConsumableCombatCraftRecipes(){
		return consumableCombatCrafts;
	}

	public static List<Recipe> GetConsumableNonconCraftRecipes(){
		return consumableNonconCrafts;
	}

	public static List<Recipe> GetWeaponUpgradeRecipes (string subtype, int id){
		return null;
	}

	public static List<Recipe> GetArmorUpgradeRecipes (string subtype, int id){
		return null;
	}
}
