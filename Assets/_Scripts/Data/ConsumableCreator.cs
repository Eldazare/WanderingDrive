using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType{UniCon, ComCon, WorldCon, DungCon};

public static class ConsumableCreator {





	private static Consumable CreateConsumable(int id, ConsumableType conType){
		string begin = "Consumable_"+ conType.ToString()+"_" + id;
		string[] data = DataManager.ReadDataString (begin).Split("_".ToCharArray());
		string classType = data [0];
		Consumable returnee = (Consumable)System.Activator.CreateInstance (System.Type.GetType (classType));
		// TODO: ADD type and potency
		return returnee;
	}
}
