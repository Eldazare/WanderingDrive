using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element{none, fire, ice, thunder, earth, light, shadow};
public enum WeaknessType{swordW, axeW, daggerW, rangedW, magicW};

public class EnemyStats {

	public int ID;
	public float health;
	public float maxHealth;
	public int damage;
	public Element element;
	public int elementDamage;
	public int armor;
	public WeaknessType armorType;
	public float hitDistance;
	public float quickness;

	public List<int> elementWeakness;
	public List<EnemyPart> partList;  //0 is always the main body
	
}
