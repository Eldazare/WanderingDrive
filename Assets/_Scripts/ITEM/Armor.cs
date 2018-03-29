using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor {

	public ArmorTypes armorType;
	public int[] elementResists;
	public float defense;
	public float magicDefense;
	public float speed;

	public Armor(int elementCount, ArmorTypes type){
		elementResists = new int[elementCount];
		armorType = type;
	}
}
