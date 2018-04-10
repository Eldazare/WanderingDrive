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
		if(player.buffFlatDamage<flatDamage){
			player.buffFlatDamage = flatDamage;
		}
		
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerBuffs.Remove(this);
		}
	}
}
