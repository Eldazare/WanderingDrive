﻿using System.Collections;
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
		if(player.staminaRegen<staminaRegen)
		player.staminaRegen = staminaRegen;
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerBuffs.Remove(this);
		}
	}
}