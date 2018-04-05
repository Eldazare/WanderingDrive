using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats {

	public WeaponType subtype;
	public int id;

	public int damage; // if damage is 0, this cannot attack
	public WeaknessType weaknessType;

	public int damageBonus; // added to other hand attacks
	public int magicDamageBonus; // added to ability damage
	public int armorBonus; // added to armor
	public int magicArmorBonus; // added to magic armor

	public int elementDamage; 
	public int elementDamageBonus; // added to other hand attacks if same element
	public Element element;

	public int dodgeModifier; // may be negative, from -100 to 100?
	public int blockModifier; // may be negative


}
