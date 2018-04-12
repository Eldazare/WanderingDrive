using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaRegen : _Buff {

	float staminaRegen;
	public StaminaRegen(float regen){
		staminaRegen = regen;
		turnsRemaining = -1;
	}
	public StaminaRegen(float regen, int turns){
		staminaRegen = regen;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		if(player != null){
			if(player.staminaRegen<staminaRegen){
				player.staminaRegen = staminaRegen;
			}
		}
	}
}
