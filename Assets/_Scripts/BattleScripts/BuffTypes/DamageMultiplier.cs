using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMultiplier : Buff {

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
	public DamageMultiplier(float multiplier, int turns, int encounters){
		multi = multiplier;
		turnsRemaining = turns;
		encountersRemaining = encounters;
	}
	override public void DoYourThing(){
		player.playerStats.buffDamageMultiplier += multi;
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerStats.playerBuffs.Remove(this);
		}
	}
}
