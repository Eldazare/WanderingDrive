using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArmorCreator {

	public static Armor CreateArmor(ArmorTypes subtype, int id){
		string begin = "armor_" + id + "_";
		Armor createe = new Armor (System.Enum.GetNames(typeof(Element)).Length, subtype);
		createe.defense = DataManager.ReadDataFloat (begin + "defense");
		createe.magicDefense = DataManager.ReadDataFloat (begin + "magicDefense");
		createe.speed = DataManager.ReadDataFloat (begin + "speed");
		string elementResStr = DataManager.ReadDataString (begin + "elementResist");
		string[] elementResStrSplit = elementResStr.Split (";".ToCharArray());
		int i = 0;
		foreach (string str in elementResStrSplit) {
			int parsedInt = int.Parse (str);
			createe.elementResists [i] = parsedInt;
			i++;
		}
		return createe;
	}
}
