using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats {


	public string subtype;
	public int ID;
	public float health;
	public float maxHealth;
	public int armor;
	public WeaknessType weaknessType;
	public float hitDistance;
	public float quickness;

	public List<int> elementWeakness;
	public List<EnemyPart> partList;  //0 is always the main body
	public List<EnemyAttack> attackList;
}
