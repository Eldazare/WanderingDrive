using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpell : Ability {

	public float damage, elementDamage;
	public Element element;
	public int id; //which effect to use
	public DamageSpell(float dmg, float eledmg, Element ele){
		damage = dmg;
		elementDamage = eledmg;
		element = ele;
	}
}
