using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBuff : Buff {
	int armor;
	public ArmorBuff(int armorgain){
		armor = armorgain;
		turnsRemaining = -1;
	}
	public ArmorBuff(int armorgain, int turns){
		armor = armorgain;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerBuffs.Remove(this);
		}
	}
}
