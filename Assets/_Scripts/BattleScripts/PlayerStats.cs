using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {

	public int damage;
	public float health, maxHealth, stamina, speed;
	public Element element;
	public int damageReduction, elementalDamage, weaponType;
	public List<int> elementalWeakness = new List<int>{0, 0, 0, 0, 0, 0};
	public WeaponStats mainHand, offHand;

	//Player buffs that reset every turn and buffs apply them everyturn
	public List<Buff> playerBuffs = new List<Buff>();
	public float buffDamageMultiplier, buffElementDamageMultiplier, healthRegen, staminaRegen, blind;
	public int buffFlatDamage, buffFlatElementDamage, buffArmor;
	public bool stunned, confused, frozen, paralyzed;
	public List<int> buffElementalWeakness = new List<int>{0, 0, 0, 0, 0, 0};

	//Buffs and debuffs end here!
	public int ability1ID, ability2ID, ability3ID, ability4ID;
	
	public int consumable1ID, consumable2ID, consumable3ID, consumable4ID;

	public PlayerStats(){
		maxHealth = 100;
		health = maxHealth;
	}

	public void ApplyPlayerBuffs(){
		buffDamageMultiplier = 0;
		buffArmor = 0;
		buffElementDamageMultiplier = 0;
		buffFlatDamage = 0;
		buffFlatElementDamage = 0;
		healthRegen = 0;
		staminaRegen = 0;
		for (int i = 0;i<buffElementalWeakness.Count;i++){
			buffElementalWeakness[i] = 0;
		}
		foreach (var item in playerBuffs)
		{
			if(item != null){
				item.DoYourThing();
			}
		}
	}
	public int abilityDamage(int abilityID){
		return damage;
	}
	public int abilityElementDamage(int abilityId){
		return elementalDamage;
	}
	public Element abilityElement(int abilityID){
		return element;
	}
}
