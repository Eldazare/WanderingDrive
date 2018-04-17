using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReduction : _Buff {

	float reduction;
	float returnDelay = 0;

	public DamageReduction(int redu){
		reduction = redu;
		turnsRemaining = -1;
		helpful = true;
	}
	public DamageReduction(int redu, int turns){
		reduction = redu;
		turnsRemaining = turns;
		helpful = true;
	}
	override public float DoYourThing(){
		if(player != null){
			if(player.buffDamageReduction<reduction){
				player.buffDamageReduction = reduction;
			}
		}else{
			if(enemy.buffDamageReduction<reduction){
				enemy.buffDamageReduction = reduction;
			}
		}
		return returnDelay;
	}
}
