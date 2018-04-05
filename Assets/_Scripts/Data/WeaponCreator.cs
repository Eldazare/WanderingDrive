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
		createe.damage = DataManager.ReadDataInt (begin + "damage");
		createe.elementDamage = DataManager.ReadDataInt (begin + "elementDamage");
		createe.element = (Element)DataManager.ReadDataInt (begin + "element");
		return createe;
	}

	public static int GetHandedness(WeaponType wepType){
		return handednessList [System.Convert.ToInt32 (wepType)];
	}
}
