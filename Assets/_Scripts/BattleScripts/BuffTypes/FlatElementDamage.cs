using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatElementDamage : Buff {
	int flatDamage;
	public FlatElementDamage(int flat){
		flatDamage = flat;
		turnsRemaining = -1;
	}
	public FlatElementDamage(int flat, int turns){
		flatDamage = flat;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		if(player != null){
			if(player.buffFlatDamage<flatDamage){
				player.buffFlatDamage = flatDamage;
			}
		}else{
			if(enemy.buffFlatDamage<flatDamage){
				enemy.buffFlatDamage = flatDamage;
			}
		}
		
	}
}
