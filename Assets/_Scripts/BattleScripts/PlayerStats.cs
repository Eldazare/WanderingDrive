using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {

	public int damage;
	public float health;
	public float maxHealth;
	public float stamina;
	public float speed;
	public Element element;
	public int damageReduction;
	public int elementalDamage;
	public List<int> elementalWeakness = new List<int>{0, 0, 0, 0, 0, 0};
	public int weaponType;
	public WeaponStats mainHand, offHand;

	//Player buffs that reset every turn and buffs apply them everyturn
	public List<Buff> playerBuffs = new List<Buff>();
	public float buffDamageMultiplier;
	public float buffElementDamageMultiplier;
	public float buffFlatDamage;
	public float buffFlatElementDamage;
	public List<int> buffElementalWeakness = new List<int>{0, 0, 0, 0, 0, 0};
	
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

	public void ApplyPlayerBuffs(){
		buffDamageMultiplier = 0;
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
