using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {

	public float damage, elementDamage;
	public Element element;

	public Attack(float _damage, float _elementDamage, Element _element){
		damage = _damage;
		elementDamage = _elementDamage;
		element = _element;
	}
}
