using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMultiplier : _Buff {

	float multi;
	bool buffGiven;
	public DamageMultiplier(float multiplier){
		multi = multiplier;
		turnsRemaining = -1;
	}
	public DamageMultiplier(float multiplier, int turns){
		multi = multiplier;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		if(player.buffDamageMultiplier<multi){
			player.buffDamageMultiplier = multi;
		}
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerBuffs.Remove(this);
		}
	}
}
