using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaRegen : _Buff {

	float staminaRegen;
	float returnDelay = 0;
	public StaminaRegen(float regen){
		staminaRegen = regen;
		turnsRemaining = -1;
		helpful = true;
	}
	public StaminaRegen(float regen, int turns){
		staminaRegen = regen;
		turnsRemaining = turns;
		helpful = true;
	}
	override public float DoYourThing(){
		if(player != null){
			if(player.staminaRegen<staminaRegen){
				player.staminaRegen = staminaRegen;
			}
		}
		return returnDelay;
	}
}
