using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatDamage : _Buff {

	int flatDamage;
	public FlatDamage(int flat){
		flatDamage = flat;
		turnsRemaining = -1;
	}
	public FlatDamage(int flat, int turns){
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
