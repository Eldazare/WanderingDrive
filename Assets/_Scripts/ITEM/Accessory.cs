using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory {

	public float damage; // bonus damage to all attacks (both melee and ability)
	public float elementDamage; // bonus element damage to all attacks, regardless of element
	public float magicDefense;
	public List<int> elementResists;

	public Accessory(int elementCount){
		elementResists = new List<int>(){};
		for (int i = 0; i < elementCount; i++) {
			elementResists.Add (0);
		}
	}

}
