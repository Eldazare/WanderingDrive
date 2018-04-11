using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {

	public float damage, elementDamage, magicDamageBonus;
	public float health, maxHealth, stamina, maxStamina, speed, physicalArmor, magicArmor, blockModifier = 1, dodgeModifier = 1;
	public int damageReduction;
	public List<int> elementWeakness = new List<int>{0, 0, 0, 0, 0, 0};
	public WeaponStats mainHand, offHand;

	public List<Ability> abilities = new List<Ability>();
	
	public int ability1ID, ability2ID, ability3ID, ability4ID;
	
	public int consumable1ID, consumable2ID, consumable3ID, consumable4ID;

	public PlayerStats(){
		maxHealth = 100;
		health = maxHealth;
	}
	
	public float abilityDamage(int abilityID){
		return mainHand.damage;
	}
	public float abilityElementDamage(int abilityId){
		return mainHand.elementDamage;
	}
	public Element abilityElement(int abilityID){
		return mainHand.element;
	}
}
