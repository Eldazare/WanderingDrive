using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : Buff {
	float heal;
	public HealthRegen(float regen){
		heal = regen;
		turnsRemaining = -1;
	}
	public HealthRegen(float regen, int turns){
		heal = regen;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){

		player.health+=heal;

		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerBuffs.Remove(this);
		}
	}
	
}
