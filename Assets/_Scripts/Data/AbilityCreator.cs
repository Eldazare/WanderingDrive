using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityEnum{
	Focus, Overload, FireBall, AccuracyBuff
}
public static class AbilityCreator  {

	public static string[] CreateAbility(AbilityAbstraction abilAbs){
		string begin = "Ability_" + abilAbs.abilityClassName.ToString() + "_" + abilAbs.lvl;
		string[] data = DataManager.ReadDataString (begin).Split("_".ToCharArray());
		return data;
	}
}
