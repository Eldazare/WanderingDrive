using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorType{Helm, Chest, Arms, Legs, Boots, Accessory};
// IMPORTANT: Copy to ->
//				NameType			(in NameDescContainer)
//				ItemSubType 		(in RecipeContainer)
//				EquipmentSubtype 	(in RecipeContainer)

public static class ArmorCreator {

	public static Armor CreateArmor(ArmorType subtype, int id){
		if (subtype == ArmorType.Accessory) {
			Debug.LogError ("Wrong armor creator used, use accessory creator instead");
			return null;
		}
		string begin = "Armor_" + id + "_";
		Armor createe = new Armor (System.Enum.GetNames(typeof(Element)).Length, subtype);
		createe.defense = DataManager.ReadDataFloat (begin + "defense");
		createe.magicDefense = DataManager.ReadDataFloat (begin + "magicDefense");
		createe.speed = DataManager.ReadDataFloat (begin + "speed");
		string elementResStr = DataManager.ReadDataString (begin + "elementResist");
		string[] elementResStrSplit = elementResStr.Split (";".ToCharArray());
		int i = 1;
		foreach (string str in elementResStrSplit) {
			int parsedInt = int.Parse (str);
			createe.elementResists [i] = parsedInt;
			i++;
		}
		return createe;
	}

	public static Accessory CreateAccessory(int id){
		string begin = "Accessory_" + id + "_";
		Accessory createe = new Accessory (System.Enum.GetNames (typeof(Element)).Length);

		string[] accessoryBonuses = DataManager.ReadDataString (begin + "bonuses").Split("/".ToCharArray());
		createe.damage = int.Parse(accessoryBonuses [0]);
		createe.elementDamage = int.Parse (accessoryBonuses [1]);
		createe.magicDefense = int.Parse(accessoryBonuses [2]);

		string elementResStr = DataManager.ReadDataString (begin + "elementResist");
		string[] elementResStrSplit = elementResStr.Split (";".ToCharArray());
		int i = 1;
		foreach (string str in elementResStrSplit) {
			int parsedInt = int.Parse (str);
			createe.elementResists [i] = parsedInt;
			i++;
		}
		return createe;
	}
}
