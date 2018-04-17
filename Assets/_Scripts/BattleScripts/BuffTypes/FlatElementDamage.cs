using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatElementDamage : _Buff {
	int flatDamage;
	float returnDelay = 0;
	public FlatElementDamage(int flat){
		flatDamage = flat;
		turnsRemaining = -1;
	}
	public FlatElementDamage(int flat, int turns){
		flatDamage = flat;
		turnsRemaining = turns;
	}
	override public float DoYourThing(){
		if(player != null){
			if(player.buffFlatElementDamage<flatDamage){
				player.buffFlatElementDamage = flatDamage;
			}
		}else{
			if(enemy.buffFlatElementDamage<flatDamage){
				enemy.buffFlatElementDamage = flatDamage;
			}
		}
		return returnDelay;
		
	}
}
