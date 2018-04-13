﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InfoBoxCreator  {

	public static string GetRecipeInfoString(Recipe recipe){
		string returnee = "";
		for (int i = 0; i < 4; i++) {
			if (i < recipe.materialList.Count) {
				RecipeMaterial mat = recipe.materialList [i];
				string materialName = mat.GetName ();
				int inventoryAmount = Inventory.GetAmountInInventoryRecipMat (mat);
				returnee += i + ": " + string.Format ("{0,-20}", materialName) + string.Format ("{0:00}", inventoryAmount) + "/" +
					string.Format ("{0:00}", mat.amount);
			}
			returnee += "\n";
		}
		string resultName = recipe.resultItem.GetName ();
		returnee += ">> " + resultName;
		return returnee;
	}

	public static string GetRecipeUpgradeInfoString(RecipeUpgrade recipe){
		string returnee = "";
		returnee += "Upgrading: " + recipe.baseEquipment.GetName ();
		for (int i = 0; i < 4; i++) {
			if (i < recipe.materialList.Count) {
				RecipeMaterial mat = recipe.materialList [i];
				string materialName = mat.GetName ();
				int inventoryAmount = Inventory.GetAmountInInventoryRecipMat (mat);
				returnee += i + ": " + string.Format ("{0,-20}", materialName) + string.Format ("{0:00}", inventoryAmount) + "/" +
					string.Format ("{0:00}", mat.amount);
			}
			returnee += "\n";
		}
		string resultName = recipe.result.GetName ();
		returnee += ">> " + resultName;
		return returnee;
	}

	public static string GetMaterialInfoString(RecipeMaterial mat){
		string returnee = "";
		switch (mat.type) {
		case ItemType.Wep:
			WeaponStats block = WeaponCreator.CreateWeaponStatBlock ((WeaponType)System.Enum.Parse (typeof(WeaponType), mat.subtype.ToString ()), mat.itemId);

			break;
		case ItemType.Mat:
			string matDesc = NameDescContainer.GetDescription (NameType.Mat, mat.itemId);

			break;
		case ItemType.Cons:
			string consDesc = NameDescContainer.GetDescription((NameType)System.Enum.Parse(typeof(NameType), mat.subtype.ToString()), mat.itemId);

			break;
		case ItemType.Arm:
			ArmorType aType = (ArmorType)System.Enum.Parse(typeof(ArmorType), mat.subtype.ToString());
			if (aType == ArmorType.Accessory){
				Accessory accessory = ArmorCreator.CreateAccessory(mat.itemId);

			} else {
				Armor armor = ArmorCreator.CreateArmor(aType, mat.itemId);

			}
			break;
		}
		return returnee;
	}
}
