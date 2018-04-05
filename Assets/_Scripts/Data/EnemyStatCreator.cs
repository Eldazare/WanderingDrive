using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element{none, fire, ice, thunder, earth, light, shadow};
public enum WeaknessType{slash, stab, smash, ranged, magic};

public static class EnemyStatCreator {

	public static EnemyStats LoadStatBlockData(int enemyIndex, string type){

		string begin = type+"_";
		string identifier = begin + enemyIndex + "_";
		EnemyStats createe = new EnemyStats ();
		createe.subtype = type;
		createe.ID = enemyIndex;

		createe.health = DataManager.ReadDataFloat(identifier + "health");
		createe.maxHealth = createe.health;
		createe.armor = DataManager.ReadDataInt(identifier + "armor");
		createe.weaknessType = (WeaknessType)DataManager.ReadDataInt(identifier + "weaknessType");
		createe.hitDistance= DataManager.ReadDataFloat(identifier + "hitDistance");
		createe.quickness= DataManager.ReadDataFloat(identifier + "quickness");

		string elementWeaknessData = DataManager.ReadDataString(identifier + "elementWeakness");
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
			string partData = DataManager.ReadDataString (identifier + "p" + i);
			if (partData != null) {
				string[] partDataSplit = partData.Split ("_".ToCharArray ());
				EnemyPart aPart = new EnemyPart ();
				aPart.name = partDataSplit [0];
				aPart.percentageHit = int.Parse (partDataSplit [1]);
				aPart.damageMod = float.Parse (partDataSplit [2]);
				aPart.hp = float.Parse (partDataSplit [3]);
				aPart.maxHP = aPart.hp;
				createe.partList.Add (aPart);
				i++;
			} else {
				break;
			}
		}

		createe.attackList = new List<EnemyAttack> ();
		i = 1;
		while (true) {
			string eAttackData = DataManager.ReadDataString (identifier + "a" + i);
			if (eAttackData != null) {
				createe.attackList.Add (CreateEnemyAttack (eAttackData));
			} else {
				break;
			}
			i++;
		}

		return createe;
	}

	public static EnemyAttack CreateEnemyAttack(string identifierString){
		string[] idStrSplit = identifierString.Split (";".ToCharArray ());
		EnemyAttack createe = new EnemyAttack ();
		createe.damage = int.Parse(idStrSplit [0]);
		createe.elementDamage = int.Parse(idStrSplit [1]);
		createe.element = (Element)System.Enum.Parse (typeof(Element), idStrSplit [2]);
		createe.damageType = int.Parse(idStrSplit [3]);
		createe.animationSpeed = float.Parse(idStrSplit [4]);
		return createe;
	}
}
