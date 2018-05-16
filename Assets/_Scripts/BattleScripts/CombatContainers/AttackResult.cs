using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackResult{

	public float damage, elementDamage, accuracy;
	public bool area, dodged;
	public WeaknessType weaknessType;
	public Element element;
	public AttackResult(float _damage, float _elementDamage, Element _element, float accu, WeaknessType _weaknesstype){
		damage = _damage;
		elementDamage = _elementDamage;
		element = _element;
		accuracy = accu;
		weaknessType = _weaknesstype;
	}
}
