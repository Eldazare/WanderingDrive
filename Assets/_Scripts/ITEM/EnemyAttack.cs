using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack  {

	// container for attack data
	// Stored in a list, list index gives a number for the attack

	public int damage;
	public int elementDamage;
	public Element element;
	public int damageType; //PHYSICAL / MAGICAL = 0 / 1

	public float animationSpeed; // UNUSED for now, maybe find a way to implement

}
