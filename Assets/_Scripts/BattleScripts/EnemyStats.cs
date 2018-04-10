using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats {


	public string subtype;
	public int ID;
	public float health;
	public float maxHealth;
	public int armor;
	public int evasion;
	public WeaknessType weaknessType;
	public float hitDistance; // TODO: What was this? Simo: Depending on model,
							  // should be suited how close enemy gets to player before attacking
	public float quickness;

	public List<int> elementWeakness;
	public List<EnemyPart> partList;  //0 is always the main body
	public List<EnemyAttack> attackList;
}
