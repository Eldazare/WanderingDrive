using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability{
	public int staminaCost;
	public string abilityName;
	public int damage, elementDamage;
	public float potency;
	public Element element;
	public Enemy enemy;
	public bool offensive;
	public PlayerCombatScript player;
	public virtual void UseAbility(){}
}
