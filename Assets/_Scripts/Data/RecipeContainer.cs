using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CraftingRecipeType{
	Weapon, Armor, Accessory, ComCon, NonCom
};

public enum EquipmentSubtype{
	Sword, Mace, Spear, Dagger, Pistol, Bow, GBow, ShieldS, ShieldL, Talisman,
	Helm, Chest, Arms, Legs, Boots,
	Accessory
};

public enum ItemType{
	Wep, Arm, Cons, Mat
};

public enum ItemSubType {
	NonCom, ComCon, Mat, 
	Sword, Mace, Spear, Dagger, Pistol, Bow, GBow, ShieldS, ShieldL, Talisman,
	Helm, Chest, Arms, Legs, Boots,
	Accessory
};

public static class RecipeContainer {


	static bool generated = false;

	private static List<List<Recipe>> craftingRecipes = new List<List<Recipe>> (){ };
	private static List<List<RecipeUpgrade>> equipmentUpgrades = new List<List<RecipeUpgrade>>(){};


	public static void GenerateRecipes(List<string> recipeNames){
		if (!generated){
			for (int i = 0; i < System.Enum.GetNames (typeof(EquipmentSubtype)).Length; i++) {
				equipmentUpgrades.Add (new List<RecipeUpgrade> ());
			} 
			for (int i = 0; i < System.Enum.GetNames (typeof(CraftingRecipeType)).Length; i++) {
				craftingRecipes.Add (new List<Recipe> ());
			}
			generated = true;
			// TODO HERE
			foreach (string recipeName in recipeNames) {
				string[] splitStr = recipeName.Split ("_".ToCharArray ());
				if (splitStr [0] == "RecipeUp") {
					EquipmentSubtype eqSubT = (EquipmentSubtype)System.Enum.Parse (typeof(EquipmentSubtype), splitStr [1]);
					RecipeUpgrade recipeUp = RecipeCreator.CreateUpgradeRecipe (eqSubT, int.Parse (splitStr [2]));
					equipmentUpgrades [System.Convert.ToInt32 (eqSubT)].Add (recipeUp);
				} else if (splitStr [0] == "Recipe") {
					CraftingRecipeType recipetype = (CraftingRecipeType)System.Enum.Parse (typeof(CraftingRecipeType), splitStr [1]);
					Recipe recipe = RecipeCreator.CreateRecipe (int.Parse (splitStr [2]), recipetype);
					craftingRecipes [System.Convert.ToInt32 (recipetype)].Add (recipe);
				} else {
					Debug.LogError ("Recipe skipped: " + recipeName);
				}
			}
		}
	}


	public static List<Recipe> GetCraftRecipes(CraftingRecipeType recipType){
		return craftingRecipes [System.Convert.ToInt32 (recipType)];
	}

	public static List<Recipe> GetSpecificWeaponCraftingRecipes(string weaponSubtype){
		System.Enum.Parse (typeof(WeaponType), weaponSubtype); // DEBUG
		List<Recipe> createe = new List<Recipe> ();
		foreach (Recipe recip in craftingRecipes[(int)CraftingRecipeType.Weapon]) {
			if (recip.resultItem.subtype.ToString() == weaponSubtype) {
				createe.Add (recip);
			}
		}
		return createe;
	}

	public static List<Recipe> GetSpecificArmorSlotCraftingRecipes(string armorSubtype){
		System.Enum.Parse (typeof(ArmorType), armorSubtype); // DEBUG
		List<Recipe> createe = new List<Recipe> ();
		foreach (Recipe recip in craftingRecipes[(int)CraftingRecipeType.Armor]) {
			if (recip.resultItem.subtype.ToString() == armorSubtype) {
				createe.Add (recip);
			}
		}
		return createe;
	}

	public static List<List<Recipe>> GetAllCraftRecipes(){
		return craftingRecipes;
	}


	// equipment subtype and the item id in that subtype that you want improvements FOR.
	public static List<RecipeUpgrade> GetEquipmentUpgradeRecipes (EquipmentSubtype eqSub, int id){
		List<RecipeUpgrade> returnee = new List<RecipeUpgrade> (){ };
		List<RecipeUpgrade> subtypeList = equipmentUpgrades [System.Convert.ToInt32 (eqSub)];
		foreach (RecipeUpgrade upgrade in subtypeList) {
			if (upgrade.baseEquipment.itemId == id) {
				returnee.Add (upgrade);
			}
		}
		return returnee;
	}

}
