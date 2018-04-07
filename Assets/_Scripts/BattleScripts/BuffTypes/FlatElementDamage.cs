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
