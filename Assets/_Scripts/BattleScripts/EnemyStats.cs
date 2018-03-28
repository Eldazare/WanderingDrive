using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element{none, fire, ice, thunder, earth, light, shadow};
public enum ArmorType{swordW, axeW, daggerW, rangedW, magicW};

public class EnemyStats {

	public int ID;
	public float health;
	public float maxHealth;
	public int damage;
	public Element element;
	public Element elementWeakness;
	public int elementDamage;
	public int armor;
	public ArmorType armorType;
	public float hitDistance;
	public float quickness;

	/*
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
	*/
}
