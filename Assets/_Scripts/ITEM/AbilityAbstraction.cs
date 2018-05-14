using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAbstraction  {

	public AbilityEnum abilityClassName;
	public int lvl;

	public AbilityAbstraction(AbilityEnum abilityType, int lvl){
		this.abilityClassName = abilityType;
		this.lvl = lvl;
	}
}
