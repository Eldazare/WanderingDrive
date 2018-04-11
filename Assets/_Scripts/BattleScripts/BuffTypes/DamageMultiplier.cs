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
	override public void DoYourThing(){
		if(player != null){
			if(player.buffDamageMultiplier<multi){
				player.buffDamageMultiplier = multi;
			}
		}else{
			if(enemy.buffDamageMultiplier<multi){
				enemy.buffDamageMultiplier = multi;
			}
		}
	}
}
