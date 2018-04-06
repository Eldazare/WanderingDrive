using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor {

	public ArmorType armorType;
	public List<int> elementResists;
	public float defense;
	public float magicDefense;
	public float speed; // needs averageing

	public Armor(int elementCount, ArmorType type){
		elementResists = new List<int>(){};
		for (int i = 0; i < elementCount; i++) {
			elementResists.Add (0);
		}
		armorType = type;
	}
}
