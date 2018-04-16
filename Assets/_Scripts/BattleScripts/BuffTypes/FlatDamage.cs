using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatDamage : _Buff {

	int flatDamage;
	float returnDelay = 0;
	public FlatDamage(int flat){
		flatDamage = flat;
		turnsRemaining = -1;
	}
	public FlatDamage(int flat, int turns){
		flatDamage = flat;
		turnsRemaining = turns;
	}
	override public float DoYourThing(){
		if(player != null){
			if(player.buffFlatDamage<flatDamage){
				player.buffFlatDamage = flatDamage;
			}
		}else{
			if(enemy.buffFlatDamage<flatDamage){
				enemy.buffFlatDamage = flatDamage;
			}
		}
		return returnDelay;
		
	}
}
