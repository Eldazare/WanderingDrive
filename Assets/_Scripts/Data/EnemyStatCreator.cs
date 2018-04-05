using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyStatCreator {

	public static EnemyStats LoadStatBlockData(int enemyIndex, string type){

		string begin = "enemy";
		switch (type) {
		case "small":
			begin += "Small_";
			break;
		case "large":
			begin += "Large_";
			break;
		}
		string indentifier = begin + enemyIndex + "_";
		EnemyStats createe = new EnemyStats ();
		createe.ID = enemyIndex;
		createe.health = DataManager.ReadDataFloat(indentifier + "health");
		createe.maxHealth = createe.health;
		createe.damage= DataManager.ReadDataInt(indentifier + "damage");
		createe.element= (Element)DataManager.ReadDataInt(indentifier + "element");
		createe.elementDamage= DataManager.ReadDataInt(indentifier + "elementDamage");
		createe.armor = DataManager.ReadDataInt(indentifier + "armor");
		createe.armorType= (WeaknessType)DataManager.ReadDataInt(indentifier + "armorType");
		createe.hitDistance= DataManager.ReadDataFloat(indentifier + "hitDistance");
		createe.quickness= DataManager.ReadDataFloat(indentifier + "quickness");

		string elementWeaknessData = DataManager.ReadDataString(indentifier + "elementWeakness");
		string[] elementWeaknessSplit = elementWeaknessData.Split (";".ToCharArray ());
		createe.elementWeakness = new List<int> ();
		createe.elementWeakness.Add (0);
		foreach (string str in elementWeaknessSplit) {
			createe.elementWeakness.Add (int.Parse (str));
		}
		if (createe.elementWeakness.Count != System.Enum.GetNames (typeof(Element)).Length) {
			Debug.LogError ("Disparity in enemy config and Element length, ID: " + type + "_" + enemyIndex);
		}

		createe.partList = new List<EnemyPart> ();
		int i = 1;
		while (true) {
			string partData = DataManager.ReadDataString (indentifier + "p" + i);
			if (partData != null) {
				string[] partDataSplit = partData.Split ("_".ToCharArray ());
				EnemyPart aPart = new EnemyPart ();
				aPart.name = partDataSplit [0];
				aPart.percentageHit = int.Parse (partDataSplit [1]);
				aPart.damageMod = float.Parse (partDataSplit [2]);
				aPart.hp = float.Parse (partDataSplit [3]);
				createe.partList.Add (aPart);
				i++;
			} else {
				break;
			}
		}

		return createe;
	}
}
