using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType{sword, axe, spear, dagger, pistol, bow, Gbow, shieldS, shieldL};

public static class WeaponCreator  {

	private static List<int> handednessList = new List<int>(){2,1,1,1,
															1,2,2,
															1,1};

	public static WeaponStats CreateWeaponStatBlock(WeaponType wepType, int id){
		string begin = "weapon_" + System.Enum.GetName(typeof(WeaponType), wepType) + "_" + id + "_";
		WeaponStats createe = new WeaponStats ();
		createe.id = id;
		createe.subtype = wepType;

		string[] baseStr = DataManager.ReadDataString (begin + "base").Split ("/".ToCharArray ());
		createe.damage = int.Parse (baseStr [0]);
		createe.accuracyBonus = int.Parse (baseStr [1]);
		createe.weaknessType = (WeaknessType)int.Parse (baseStr [2]);

		string[] elementStr = DataManager.ReadDataString (begin + "element").Split ("/".ToCharArray ());
		createe.elementDamage = int.Parse(elementStr [0]);
		createe.elementDamageBonus = int.Parse(elementStr [2]);
		createe.element = (Element)int.Parse(elementStr [1]);

		string[] bonusStr = DataManager.ReadDataString (begin + "bonuses").Split ("/".ToCharArray ());
		createe.damageBonus = int.Parse (bonusStr [0]);
		createe.magicDamageBonus = int.Parse (bonusStr [1]);
		createe.armorBonus = int.Parse (bonusStr [2]);
		createe.magicArmorBonus = int.Parse (bonusStr [3]);

		string[] defenseModStr = DataManager.ReadDataString (begin + "defenseMods").Split ("/".ToCharArray ());
		createe.dodgeModifier = float.Parse (defenseModStr [0]);
		createe.blockModifier = float.Parse (defenseModStr [1]);
		return createe;
	}

	public static int GetHandedness(WeaponType wepType){
		return handednessList [System.Convert.ToInt32 (wepType)];
	}
}
