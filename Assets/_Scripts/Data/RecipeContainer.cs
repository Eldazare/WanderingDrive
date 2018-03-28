using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecipeContainer {

	// TODO: Armor craft recipes
	// TODO: UPGRADE RECIPES


	static bool generated = false;


	public static List<Recipe> weaponCrafts = new List<Recipe>();
	public static List<Recipe> consumableCombatCrafts = new List<Recipe>();
	public static List<Recipe> consumableNonconCrafts = new List<Recipe> ();

	public static List<List<RecipeUpgrade>> equipmentUpgrades = new List<List<RecipeUpgrade>>(System.Enum.GetNames(typeof(EquipmentSubtypes)).Length){};

	/*
	public static List<RecipeUpgrade> swordUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> axeUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> spearUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> daggerUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> pistolUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> bowUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> greatbowUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> bucklerUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> towershieldUpgrades = new List<RecipeUpgrade>();

	public static List<RecipeUpgrade> armorHelmUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> armorChestUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> armorArmsUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> armorLegsUpgrades = new List<RecipeUpgrade>();
	public static List<RecipeUpgrade> armorBootsUpgrades = new List<RecipeUpgrade>();
	*/

	public enum EquipmentSubtypes{
		sword, axe, spear, dagger, pistol, bow, greatbow, buckler, towershield,
		helm, chest, arms, legs, boots,
		accessory
	};

	public static void GenerateRecipes(List<string> recipeNames){
		if (!generated){
			generated = true;
			foreach (string str in recipeNames) {
				string[] splitStr = str.Split ("_".ToCharArray ());
				if (splitStr[0] == "recipeUp") {
					RecipeUpgrade recipeUp = RecipeCreator.CreateUpgradeRecipe (splitStr [1], int.Parse (splitStr [2]));
					EquipmentSubtypes eqSubT = (EquipmentSubtypes)System.Enum.Parse (typeof(EquipmentSubtypes), splitStr [1]);
					equipmentUpgrades [System.Convert.ToInt32 (eqSubT)].Add (recipeUp);
				}else {
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
	}

	public static List<Recipe> GetWeaponCraftRecipes(){
		return weaponCrafts;
	}

	public static List<Recipe> GetConsumableCombatCraftRecipes(){
		return consumableCombatCrafts;
	}

	public static List<Recipe> GetArmorCraftRecipes(){
		return null;
	}

	public static List<Recipe> GetConsumableNonconCraftRecipes(){
		return consumableNonconCrafts;
	}

	public static List<Recipe> GetEquipmentUpgradeRecipes (EquipmentSubtypes eqSub, int id){
		return null;
	}

}
