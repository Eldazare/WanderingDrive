﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDamageMultiplier : _Buff {
	float multi;
	public ElementDamageMultiplier(float multiplier){
		multi = multiplier;
		turnsRemaining = -1;
	}
	public ElementDamageMultiplier(float multiplier, int turns){
		multi = multiplier;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		
		if(player.buffElementDamageMultiplier<multi){
			player.buffElementDamageMultiplier = multi;
		}
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerBuffs.Remove(this);
		}
	}
}
