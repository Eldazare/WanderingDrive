using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : _Buff {
	float damageovertime;
	public DamageOverTime(float damage){
		damageovertime = damage;
		turnsRemaining = -1;
	}
	public DamageOverTime(float damage, int turns){
		damageovertime = damage;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		player.health -= damageovertime;
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerBuffs.Remove(this);
		}
	}
}
