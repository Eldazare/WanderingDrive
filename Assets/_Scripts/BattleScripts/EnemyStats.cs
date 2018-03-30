using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element{none, fire, ice, thunder, earth, light, shadow};
public enum weaknessType{swordW, axeW, daggerW, rangedW, magicW};

public class EnemyStats {

	public int ID;
	public float health;
	public float maxHealth;
	public int damage;
	public Element element;
	public Element elementWeakness;
	public int elementDamage;
	public int armor;
	public weaknessType armorType;
	public float hitDistance;
	public float quickness;

	public List<EnemyPart> partList;
	
}
