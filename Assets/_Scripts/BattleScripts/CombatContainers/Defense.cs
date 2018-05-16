using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense {
	public float physicalArmor, magicArmor, damageTakenMod, chanceToHit;
	public List<int> elementWeakness = new List<int>{0, 0, 0, 0, 0, 0, 0};
	public WeaknessType weaknessType;


	public Defense(float armor, float _magicArmor, List<int> _elementWeakness, WeaknessType _weaknesstype, float _damageTakenMod, float chance){
		physicalArmor = armor;
		magicArmor = _magicArmor;
		elementWeakness = _elementWeakness;
		weaknessType = _weaknesstype;
		damageTakenMod = _damageTakenMod;
		chanceToHit = chance;
	}
}
