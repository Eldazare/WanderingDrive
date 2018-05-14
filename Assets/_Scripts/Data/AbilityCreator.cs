using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityEnum{
	Focus, Overload, FireBall, AccuracyBuff
}
public static class AbilityCreator  {

	public static Ability CreateAbility(AbilityAbstraction abilAbs){
		string begin = "Ability_" + abilAbs.abilityClassName.ToString() + "_" + abilAbs.lvl;
		string data = DataManager.ReadDataString (begin);
		Ability returnee = (Ability)System.Activator.CreateInstance (System.Type.GetType (abilAbs.abilityClassName.ToString()));
		returnee.dataString = data;
		return returnee;
	}
}
