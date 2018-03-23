using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public int damage;
	public int health;
	public int stamina;
	public int element;
	public int damageReduction;

	public int elementalDamage;
	public int elementalWeakness;
	public int weaponType;
	public GameObject weapon;

	public int ability1ID;
	public int ability2ID;
	public int ability3ID;
	public int ability4ID;
	
	public int consumable1ID;
	public int consumable2ID;
	public int consumable3ID;
	public int consumable4ID;

	void Start(){
		damage = 10;
		health = 100;
		element = 1;
	}
	public void loadStats(){

		// Load stats
	}
}
