using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {

	public float damage, elementDamage, magicDamageBonus;
	public float health, maxHealth, stamina, maxStamina, speed, physicalArmor, magicArmor, blockModifier = 1, dodgeModifier = 1;
	public Element element;
	public int damageReduction;
	public List<int> elementWeakness = new List<int>{0, 0, 0, 0, 0, 0, 0}; // Need element for "NONE" also, for indexing purposes
	public WeaponStats mainHand, offHand;
	public List<Ability> abilities = new List<Ability> ();
	public List<CombatItem> combatItems = new List<CombatItem>();

	public PlayerStats(){
		maxHealth = 100;
		health = maxHealth;
	}
	public float AbilityDamage(int abilityID){
		return abilities[abilityID].damage;
	}
	public float AbilityElementDamage(int abilityID){
		return abilities[abilityID].elementDamage;
	}
	public Element AbilityElement(int abilityID){
		return abilities[abilityID].element;
	}
}
