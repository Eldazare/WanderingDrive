using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory {

	public float damage;
	public float magicDefense;
	public List<int> elementResists;

	public Accessory(int elementCount){
		elementResists = new List<int>(){};
		for (int i = 0; i < elementCount; i++) {
			elementResists.Add (0);
		}
	}

}
