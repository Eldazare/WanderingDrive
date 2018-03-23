using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

	public int ID;
	public float health;
	public int damage;
	public int element;
	public int elementWeakness;
	public int elementDamage;
	public int armorType;


	public EnemyStats(int id){
		ID = id;
		health = 100;
		damage = 10;
		element = 1;
		elementWeakness = 2;
		elementDamage = 5;
		armorType = 1;
		// Generate stats from the enemy ID
	}



}
