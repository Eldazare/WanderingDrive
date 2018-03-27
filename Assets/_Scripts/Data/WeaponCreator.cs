using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponCreator  {
	
	// subtypes: sword | axe | spear | dagger | pistol | bow | greatbow | buckler | towershield


	public static WeaponStats CreateWeaponStatBlock(string subtype, int id){
		string begin = "weapon_" + subtype + "_" + id + "_";
		WeaponStats createe = new WeaponStats ();
		createe.id = id;
		createe.subtype = subtype;
		createe.damage = DataManager.ReadDataInt (begin + "damage");
		createe.elementDamage = DataManager.ReadDataInt (begin + "elementDamage");
		createe.element = (Element)DataManager.ReadDataInt (begin + "element");
		return createe;
	}
}
