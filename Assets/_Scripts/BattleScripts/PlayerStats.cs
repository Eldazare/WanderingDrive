using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {

	public int damage;
	public float health;
	public float maxHealth;
	public int stamina;
	public Element element;
	public int damageReduction;

	public int elementalDamage;
	public Element elementalWeakness;
	public int weaponType;
	public WeaponStats weapon;

	public int ability1ID;
	public int ability2ID;
	public int ability3ID;
	public int ability4ID;
	
	public int consumable1ID;
	public int consumable2ID;
	public int consumable3ID;
	public int consumable4ID;

	public PlayerStats(){
		maxHealth = 100;
		health = maxHealth;
	}
	public int abilityDamage(int abilityID){
		return damage;
	}
	public int abilityElementDamage(int abilityID){
		return elementalDamage;
	}
	public Element abilityElement(int abilityID){
		return element;
	}
}
