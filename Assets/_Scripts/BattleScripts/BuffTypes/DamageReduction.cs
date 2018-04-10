using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReduction : Buff {

	float reduction;

	public DamageReduction(int redu){
		reduction = redu;
		turnsRemaining = -1;
	}
	public DamageReduction(int redu, int turns){
		reduction = redu;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		if(player != null){
			if(player.buffDamageReduction<reduction){
				player.buffDamageReduction = reduction;
			}
		}else{
			if(enemy.buffDamageReduction<reduction){
				enemy.buffDamageReduction = reduction;
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
