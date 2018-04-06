using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaRegen : Buff {

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
		player.playerStats.stamina += staminaRegen;
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerStats.playerBuffs.Remove(this);
		}
	}
}
