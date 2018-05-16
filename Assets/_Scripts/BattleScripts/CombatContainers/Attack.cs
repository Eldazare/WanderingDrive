using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {

	public float damage, elementDamage, damageMod, eleDamageMod, accuracy;
	public Element element;
	public WeaponStats weapon;
	public WeaknessType weaknessType;
	public int damageType;
	public bool area, dodged;


	public Attack(float _damage, float _elementDamage, Element _element){
		damage = _damage;
		elementDamage = _elementDamage;
		element = _element;
	}
	public Attack(float _damage, float _elementDamage, Element _element, float _damageMod, float _eleDamageMod, float accu, int _damageType){
		damage = _damage;
		elementDamage = _elementDamage;
		element = _element;
		damageMod = _damageMod;
		eleDamageMod = _eleDamageMod;
		accuracy = accu;
		damageType = _damageType;
	}
}
