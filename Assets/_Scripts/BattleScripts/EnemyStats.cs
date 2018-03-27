using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats {

	public int ID;
	public float health;
	public float maxHealth;
	public int damage;
	public int element;
	public int elementWeakness;
	public int elementDamage;
	public int armorType;
	public float hitDistance;


	public EnemyStats(int id){
		ID = id;
		hitDistance = 1;
		maxHealth = 100;
		health = maxHealth;
		damage = 10;
		element = 1;
		elementWeakness = 2;
		elementDamage = 5;
		armorType = 1;
		// Generate stats from the enemy ID
	}



}
