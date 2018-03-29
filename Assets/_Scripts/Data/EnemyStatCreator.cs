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
			begin += "large_";
			break;
		}
		string indetifier = begin + enemyIndex + "_";
		EnemyStats createe = new EnemyStats ();
		createe.ID = enemyIndex;
		createe.health = DataManager.ReadDataFloat(indetifier + "health");
		createe.maxHealth = createe.health;
		createe.damage= DataManager.ReadDataInt(indetifier + "damage");
		createe.element= (Element)DataManager.ReadDataInt(indetifier + "element");
		createe.elementWeakness= (Element)DataManager.ReadDataInt(indetifier + "elementWeakness");
		createe.elementDamage= DataManager.ReadDataInt(indetifier + "elementDamage");
		createe.armor = DataManager.ReadDataInt(indetifier + "armor");
		createe.armorType= (weaknessType)DataManager.ReadDataInt(indetifier + "armorType");
		createe.hitDistance= DataManager.ReadDataFloat(indetifier + "hitDistance");
		createe.quickness= DataManager.ReadDataFloat(indetifier + "quickness");

		return createe;
	}
}
