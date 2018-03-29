﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor {

	public ArmorTypes armorType;
	public List<int> elementResists;
	public float defense;
	public float magicDefense;
	public float speed;

	public Armor(int elementCount, ArmorTypes type){
		elementResists = new List<int>(){};
		for (int i = 0; i < elementCount; i++) {
			elementResists.Add (0);
		}
		armorType = type;
	}
}