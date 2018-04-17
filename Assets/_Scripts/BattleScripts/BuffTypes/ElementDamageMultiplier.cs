using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDamageMultiplier : _Buff {
	float multi;

	float returnDelay = 0;
	public ElementDamageMultiplier(float multiplier){
		multi = multiplier;
		turnsRemaining = -1;
		helpful = true;
	}
	public ElementDamageMultiplier(float multiplier, int turns){
		multi = multiplier;
		turnsRemaining = turns;
		helpful = true;
	}
	override public float DoYourThing(){
		if(player != null){
			if(player.buffElementDamageMultiplier<multi){
				player.buffElementDamageMultiplier = multi;
			}
		}else{
			if(enemy.buffElementDamageMultiplier<multi){
				enemy.buffElementDamageMultiplier = multi;
			}
		}
		return returnDelay;
	}
}
