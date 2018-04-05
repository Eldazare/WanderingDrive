using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropData {

	public List<RecipeMaterial> drops; // material id
	public List<int> percentageList;

	public DropData(){
		drops = new List<RecipeMaterial> ();
		percentageList = new List<int> ();
	}

	virtual
	public List<DropData> GetPartDrops(){
		return null;
	
	}
}
