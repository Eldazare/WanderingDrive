using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType{ConsumableUniversal, ConsumableCombat, ConsumableWorld, ConsumableDungeon};

public static class ConsumableCreator {





	public static Consumable CreateConsumable(int id, ConsumableType conType){
		string begin = conType.ToString()+"_" + id;
		string[] data = DataManager.ReadDataString (begin).Split("_".ToCharArray());
		string classType = data [0];
		Consumable returnee = (Consumable)System.Activator.CreateInstance (System.Type.GetType (classType));
		returnee.type = conType;
		returnee.id = id;
		returnee.potency = float.Parse (data [1]);
		return returnee;
	}
}
