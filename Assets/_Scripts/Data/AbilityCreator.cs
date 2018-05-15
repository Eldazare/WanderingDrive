using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sync with Names in AbilityConfig
public enum AbilityEnum{
	Focus, Overload, FireBall, AccuracyBuff
}
public static class AbilityCreator  {

	public static Ability CreateAbility(AbilityAbstraction abilAbs){
		string begin = "Ability_" + abilAbs.abilityClassName.ToString() + "_" + abilAbs.lvl;
		string[] data = DataManager.ReadDataString (begin).Split("/".ToCharArray());
		Ability returnee = (Ability)System.Activator.CreateInstance (System.Type.GetType (data[0]));
		returnee.SetStringArr(data, abilAbs.abilityClassName.ToString());
		return returnee;
	}
}
