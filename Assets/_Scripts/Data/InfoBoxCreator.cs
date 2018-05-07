using System.Collections;
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
				returnee += string.Format ("{0,-20}{1,2}/{2,2}", materialName, inventoryAmount, mat.amount);
			}
			returnee += "\n";
		}
		string resultName = recipe.resultItem.GetName ();
		returnee += ">> " + resultName;
		return returnee;
	}

	public static string GetRecipeUpgradeInfoString(RecipeUpgrade recipe){
		string returnee = "";
		returnee += "Upgrading: " + recipe.baseEquipment.GetName ()+"\n\n";
		for (int i = 0; i < 4; i++) {
			if (i < recipe.materialList.Count) {
				RecipeMaterial mat = recipe.materialList [i];
				string materialName = mat.GetName ();
				int inventoryAmount = Inventory.GetAmountInInventoryRecipMat (mat);
				returnee += string.Format ("{0,-20}{1,2}/{2,2}", materialName, inventoryAmount, mat.amount);
			}
			returnee += "\n";
		}
		string resultName = recipe.result.GetName ();
		returnee += ">> " + resultName;
		return returnee;
	}

	public static string GetMaterialInfoString(RecipeMaterial mat){
		string returnee = mat.GetName() + "\n";
		switch (mat.type) {
		case ItemType.Wep:
			WeaponStats block = WeaponCreator.CreateWeaponStatBlock ((WeaponType)System.Enum.Parse (typeof(WeaponType), mat.subtype.ToString ()), mat.itemId);
			returnee += string.Format("{0,-18} {1,7} \n","Damage Type", block.weaknessType.ToString());
			returnee += string.Format("{0,-18} {1,7} \n","Damage", block.damage); 
			returnee += string.Format("{0,-18} {1,7} \n","Accuracy Bonus", block.accuracyBonus); 
			returnee += string.Format("{0,-18} {1,7} \n","Element", block.element.ToString()); 
			returnee += string.Format("{0,-18} {1,7} \n","Element Damage", block.elementDamage); 
			returnee += string.Format("{0,-18} {1,7} \n","Armor", block.armorBonus); 
			returnee += string.Format("{0,-18} {1,7} \n","Magic Armor", block.magicArmorBonus);  
			returnee += string.Format("{0,-18} {1,7} \n","Block Modifier", block.blockModifier); 
			returnee += string.Format("{0,-18} {1,7} \n","Dodge Modifier", block.dodgeModifier); 
			break;
		case ItemType.Mat:
			string matDesc = NameDescContainer.GetDescription (NameType.Mat, mat.itemId);
			returnee += matDesc + "\n";
			break;
		case ItemType.Cons:
			string consDesc = NameDescContainer.GetDescription((NameType)System.Enum.Parse(typeof(NameType), mat.subtype.ToString()), mat.itemId);
			returnee += consDesc + "\n";
			break;
		case ItemType.Arm:
			ArmorType aType = (ArmorType)System.Enum.Parse(typeof(ArmorType), mat.subtype.ToString());
			if (aType == ArmorType.Accessory){
				Accessory accessory = ArmorCreator.CreateAccessory(mat.itemId);
				returnee += string.Format("{0,-18} {1,7} \n","Damage Bonus", accessory.damage);
				returnee += string.Format("{0,-18} {1,7} \n","Element damage", accessory.elementDamage);
				returnee += string.Format("{0,-18} {1,7} \n","Magic defense", accessory.magicDefense);
				for (int i = 1; i<accessory.elementResists.Count;i++) {
					returnee += string.Format("{0,-18} {1,7} \n", ((Element)i).ToString() + " resist", accessory.elementResists[i]);
				}
			} else {
				Armor armor = ArmorCreator.CreateArmor(aType, mat.itemId);
				returnee += string.Format("{0,-18} {1,7} \n","Slot: ", armor.armorType);
				returnee += string.Format("{0,-18} {1,7} \n","Armor: ", armor.defense);
				returnee += string.Format("{0,-18} {1,7} \n","Magic Defense: ", armor.magicDefense);
				returnee += string.Format("{0,-18} {1,7} \n","Armor Speed: ", armor.speed);
				for (int i = 1; i<armor.elementResists.Count;i++) {
					returnee += string.Format("{0,-18} {1,7} \n", ((Element)i).ToString() + " resist", armor.elementResists[i]);
				}
			}
			break;
		}
		return returnee;
	}

	public static string GetLoadoutNamesString(Loadout loadout) {
		string returnee = "";
		if (loadout.mainHand != null) {
			returnee += string.Format ("{0,-18} {1,7} \n", "Main-hand: ", GetAName(loadout.mainHand.subType, loadout.mainHand.itemID));
		}
		if (loadout.mainHand != null) {
			returnee += string.Format ("{0,-18} {1,7} \n", "Off-hand:  ", GetAName(loadout.offHand.subType, loadout.offHand.itemID));
		}
		foreach (InventoryArmor invArm in loadout.wornArmor) {
			if (invArm != null) {
				returnee += string.Format("{0,-18} {1,7} \n",invArm.subType + ": ", GetAName(invArm.subType, invArm.itemID));
			}
		}
		foreach (InventoryArmor invArm in loadout.wornAccessories) {
			if (invArm != null) {
				returnee += string.Format ("{0,-18} {1,7} \n", invArm.subType + ": ", GetAName (invArm.subType, invArm.itemID));
			}
		}
		return returnee;
	}

	public static string GetLoadoutTotalArmorStats(Loadout loadout){
		List<int> elementTotals = new List<int> ();
		int elementCount = System.Enum.GetNames (typeof(Element)).Length;
		for (int i = 0; i < elementCount; i++) {
			elementTotals.Add (0);
		}
		float totalMagicArmor = 0;
		float totalArmor = 0;
		float speedAvg = 0;
		int speedsCount = 0;

		foreach (InventoryArmor invArmor in loadout.wornArmor) {
			if (invArmor != null) {
				Armor armor = ArmorCreator.CreateArmor ((ArmorType)System.Enum.Parse (typeof(ArmorType), invArmor.subType), invArmor.itemID);
				if (armor != null) {
					totalArmor += armor.defense;
					totalMagicArmor += armor.magicDefense;
					for (int i = 0; i < elementCount; i++) {
						elementTotals [i] += armor.elementResists [i];
					}
					speedAvg += armor.speed;
				} else {
					speedAvg += 1;
				}
			}else {
				speedAvg += 1;
			}
			speedsCount++;
		}
		speedAvg /= (float)speedsCount;

		string returnee = "";
		returnee += string.Format ("{0,-18} {1,7} \n", "Armor: ", totalArmor);
		returnee += string.Format ("{0,-18} {1,7} \n", "MagicArmor: ", totalMagicArmor);
		for (int i = 1; i < elementCount; i++){
			returnee += string.Format ("{0,-18} {1,7} \n", ((Element)i).ToString() + " resist: ", elementTotals[i]);
		}
		returnee += string.Format ("{0,-18} {1,7} \n", "Speed: ", speedAvg);
		return returnee;
	}
	
	private static string GetAName(string subtype, int id){
		return NameDescContainer.GetName ((NameType)System.Enum.Parse (typeof(NameType), subtype), id);
	}

	public static string GetMatListInfo(List<RecipeMaterial> list){
		string returnee = "";
		foreach (RecipeMaterial mat in list) {
			returnee += string.Format ("{0,-18} {1,7} \n", mat.GetName (), mat.amount);
		}
		return returnee;
	}
}
