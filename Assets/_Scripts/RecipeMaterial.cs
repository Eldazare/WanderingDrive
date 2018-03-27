using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeMaterial  {

	public string type; // wep | mat | cons | arm
	public string subtype; // mat | world | comb | (sword | axe | dagg | pist | bow | lbow | shis | shil) | (head | torso | arms | legs | boots)
    public int itemId;
	public string itemName; // TODO
	public int amount;



	public RecipeMaterial(string identifier){
		string[] matArr = identifier.Split ("_".ToCharArray ());
		type = matArr [0];
		subtype = matArr [1];
		itemId = int.Parse (matArr [2]);
		amount = int.Parse (matArr [3]);
	}


	public string GetIdentifier(){
		return type + "_" + subtype + "_" + itemId;
	}

	public string GetName(){
		return itemName;
	}

    //Efekti

}
