﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeMaterial  {

	public ItemType type; // wep | mat | consumable | arm
	public ItemSubType subtype; // mat | nonCom | comCon | (sword | axe | spear | dagger | pistol | bow | Gbow | shield | shil) | (head | torso | arms | legs | boots)
    public int itemId;
	public string itemName; // TODO
	public int amount;



	public RecipeMaterial(string identifier){
		string[] matArr = identifier.Split ("_".ToCharArray ());
		type = (ItemType)System.Enum.Parse(typeof(ItemType),matArr [0]);
		subtype = (ItemSubType)System.Enum.Parse(typeof(ItemSubType),matArr [1]);
		itemId = int.Parse (matArr [2]);
		if (matArr.Length > 3) {
			amount = int.Parse (matArr [3]);
		} else {
			amount = 1;
		}
	}


	public string GetIdentifier(){
		return type + "_" + subtype + "_" + itemId;
	}

	public string GetName(){
		return itemName;
	}

    //Efekti

}
