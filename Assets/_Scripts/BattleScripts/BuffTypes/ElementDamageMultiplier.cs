using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDamageMultiplier : Buff {
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
		if(player != null){
			if(player.buffElementDamageMultiplier<multi){
				player.buffElementDamageMultiplier = multi;
			}
		}else{
			if(enemy.buffElementDamageMultiplier<multi){
				enemy.buffElementDamageMultiplier = multi;
			}
		}
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			if(player != null){
				player.playerBuffs.Remove(this);
			}else{
				enemy.enemyBuffs.Remove(this);
			}
		}
	}
}
